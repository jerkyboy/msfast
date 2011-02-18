//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Tasks.EmailsSender)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;
using EYF.Tasks;
using MySpace.MSFast.Automation.Providers.Collectors;
using EYF.Core.Configuration;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Entities.Collectors;
using MySpace.MSFast.Automation.Entities.Triggers;
using log4net;
using MySpace.MSFast.Automation.Providers.Results.Browse;
using System.IO;
using EYF.Providers.Notifications.NotificationTemplates;
using EYF.Providers.ThridParty.Twitter;
using EYF.Entities.Notifications;
using MySpace.MSFast.Automation.Entities.Results;
using System.Net.Mail;
using System.Net;

namespace MySpace.MSFast.Automation.Tasks.EmailsSender
{
    public class EmailSenderTask : BaseLoopTask
    {
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();
        public static String EmailTemplate = "";
        private String stateFile = "";
        private String DumpFolder = "";

        private String[] perMatrixRecep;
        private String[] matrixRecep;

        public EmailSenderTask()
        {
            if (log.IsDebugEnabled) log.Debug("Starting checking emails que");
            if (log.IsInfoEnabled) log.Info("Setting time interval to " + AppConfig.Instance["CheckInterval"]);
            base.Interval = long.Parse(AppConfig.Instance["CheckInterval"]);
            stateFile = AppConfig.Instance["StateFile"];
            DumpFolder = AppConfig.Instance["DumpFolder"];
            perMatrixRecep = AppConfig.Instance["Recipients_PerfTable"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            matrixRecep = AppConfig.Instance["Recipients_Matrix"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override TaskResults ExecuteTask()
        {
            if (log.IsDebugEnabled)
                log.Debug("Starting ExecuteTask");

            TaskResults results = new TaskResults();

            DateTime lastRunTime = DateTime.Now.Subtract(new TimeSpan(5,0,0,0));

            if (String.IsNullOrEmpty(stateFile) == false && File.Exists(stateFile))
            {
                String lastRun = File.ReadAllText(stateFile);
                if (String.IsNullOrEmpty(lastRun) == false)
                    DateTime.TryParse(lastRun, out lastRunTime);
            }
            
            if (log.IsDebugEnabled)
                log.Debug("Last Runtime - " + lastRunTime);

            //Check that we only send it at 13:00, once a day
            if (String.IsNullOrEmpty(AppConfig.Instance["RunAnyway"]) && (DateTime.Now.Hour != uint.Parse(AppConfig.Instance["RunTime"]) || DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0)) < lastRunTime))
            {
                if (log.IsDebugEnabled)
                    log.Debug("Skipping...");

                return results;
            }           

            if (log.IsDebugEnabled)
                log.Debug("Start Generating Email");


            if (log.IsDebugEnabled)
                log.Debug("Calling SendDailyMatrix...");
            
            if (SendDailyMatrix() == false)
            {
                results.Failed++;
                return results;
            }
            
            if (SendDailyTables() == false)
            {
                results.Failed++;
                return results;
            }

            results.Succeeded++;

            File.WriteAllText(stateFile, DateTime.Now.ToString());

            return results;
        }

        private bool SendDailyTables()
        {
            if (log.IsDebugEnabled)
                log.Debug("Getting Results...");

            BrowseAvgResultsEntities ClientResults = new BrowseAvgResultsEntities();
            ClientResults.LastHours = 244;
            ClientResults.Order = BrowseAvgResultsEntities.OrderBy.ClientTime;
            ClientResults.ResultsPerPage = 150;
            ClientResults.Load(null);

            BrowseAvgResultsEntities ServerResults = new BrowseAvgResultsEntities();
            ServerResults.LastHours = 244;
            ServerResults.Order = BrowseAvgResultsEntities.OrderBy.ServerTime;
            ServerResults.ResultsPerPage = 150;
            ServerResults.Load(null);

            if (ServerResults.Data == null || ServerResults.Data.Count == 0 || ClientResults.Data == null || ClientResults.Data.Count == 0)
            {
                if (log.IsErrorEnabled)
                    log.Debug("Received empty results");

                return false;
            }
            if (log.IsDebugEnabled)
                log.Debug("Received " + ServerResults.Data.Count + " Results.");

            NotificationWithExtendedDataObject data = new NotificationWithExtendedDataObject() { ExtendedData = new Dictionary<String, Object>() };

            data.ExtendedData.Add("ClientResults", ClientResults.Data);
            data.ExtendedData.Add("ServerResults", ServerResults.Data);

            return SendEmail(perMatrixRecep, "DailyTables", data);
        }


        private bool SendDailyMatrix()
        {
            if (log.IsDebugEnabled)
                log.Debug("Getting Results...");

            BrowseAvgResultsEntities bar = new BrowseAvgResultsEntities();
            bar.LastHours = 244;
            bar.Order = BrowseAvgResultsEntities.OrderBy.ClientTime;
            bar.ResultsPerPage = 150;
            bar.Load(null);

            if (bar.Data == null || bar.Data.Count == 0)
            {
                if (log.IsErrorEnabled)
                    log.Debug("Received empty results");

                return false;
            }
            if (log.IsDebugEnabled)
                log.Debug("Received " + bar.Data.Count + " Results.");

            NotificationWithExtendedDataObject data = new NotificationWithExtendedDataObject() { ExtendedData = new Dictionary<String, Object>() };
            data.ExtendedData.Add("Results", bar.Data);

            return SendEmail(matrixRecep, "DailyReport", data);
        }

        private bool SendEmail(String[] recep, String template, NotificationWithExtendedDataObject data)
        {
            
            EmailNotificationTemplate et = NotificationTemplatesRepository.GetNotificationTemplate(template, data, 0, NotificationType.Email) as EmailNotificationTemplate;

            if (et == null)
                return false;

            if(log.IsDebugEnabled)
                log.Debug("Save Dump to " + DumpFolder + template);

            File.WriteAllText(DumpFolder + template, et.CompiledTemplate);

            try
            {
                MailMessage mm = new MailMessage();

                mm.From = new MailAddress(AppConfig.Instance["EmailFrom"], AppConfig.Instance["From"]);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = Encoding.UTF8;
                
                foreach(String rec in recep)
                    mm.To.Add(new MailAddress(rec));

                mm.Subject = et.CompiledEmailSubject;
                mm.Body = et.CompiledTemplate;

                SmtpClient sc = GetSmtpClient();
                sc.Send(mm);
            }
            catch (Exception e)
            {
                if(log.IsErrorEnabled)
                    log.Error("Error Sending Email ", e);
            }
            return true;
        }

        private SmtpClient GetSmtpClient()
        {
            try
            {
                SmtpClient client = new SmtpClient(AppConfig.Instance["EmailSMTPServer"], int.Parse(AppConfig.Instance["EmailSMTPServerPort"]));

                client.EnableSsl = String.IsNullOrEmpty(AppConfig.Instance["EmailUseSSL"]) == false;
                client.UseDefaultCredentials = String.IsNullOrEmpty(AppConfig.Instance["EmailUseDefaultCredentials"]) == false;

                if (String.IsNullOrEmpty(AppConfig.Instance["EmailSMTPServerUsername"]) == false && String.IsNullOrEmpty(AppConfig.Instance["EmailSMTPServerPassword"]) == false)
                    client.Credentials = new NetworkCredential(AppConfig.Instance["EmailSMTPServerUsername"], AppConfig.Instance["EmailSMTPServerPassword"]);

                return client;
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled) log.Error("Error setting up smtp client", e);
            }

            return null;
        }
    }
}
