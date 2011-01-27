using System;
using System.Collections.Generic;
using System.Text;
using EYF.Tasks;
using BDika.Providers.Collectors;
using EYF.Core.Configuration;
using BDika.Entities.Tests;
using BDika.Entities.Collectors;
using BDika.Entities.Triggers;
using log4net;
using BDika.Providers.Results.Browse;
using System.IO;
using EYF.Providers.Notifications.NotificationTemplates;
using EYF.Providers.ThridParty.Twitter;
using EYF.Entities.Notifications;
using BDika.Entities.Results;
using System.Net.Mail;

namespace BDika.Tasks.EmailsSender
{
    public class EmailSenderTask : BaseLoopTask
    {
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();
        public static String EmailTemplate = "";
        private String stateFile = "";

        private String[] perMatrixRecep;
        private String[] matrixRecep;

        public EmailSenderTask()
        {
            if (log.IsDebugEnabled) log.Debug("Starting checking emails que");
            if (log.IsInfoEnabled) log.Info("Setting time interval to " + AppConfig.Instance["CheckInterval"]);
            base.Interval = long.Parse(AppConfig.Instance["CheckInterval"]);
            stateFile = AppConfig.Instance["StateFile"];
            
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

#if !DEBUG            
            //Check that we only send it at 13:00, once a day
            if (DateTime.Now.Hour != 13 || DateTime.Now.Subtract(new TimeSpan(0, 5, 0, 0)) < lastRunTime)
            {
                if (log.IsDebugEnabled)
                    log.Debug("Skipping...");

                return results;
            }           
#endif

            if (log.IsDebugEnabled)
                log.Debug("Start Generating Email");


            if (log.IsDebugEnabled)
                log.Debug("Calling SendDailyMatrix...");
            /*
            if (SendDailyMatrix() == false)
            {
                results.Failed++;
                return results;
            }
            */
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

            return SendEmail(matrixRecep, "DailyTables", data);
        }


        private bool SendDailyMatrix()
        {
            if (log.IsDebugEnabled)
                log.Debug("Getting Results...");

            BrowseAvgResultsEntities bar = new BrowseAvgResultsEntities();
            bar.LastHours = 244;
            bar.Order = BrowseAvgResultsEntities.OrderBy.ServerTime;
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

            try
            {
                MailMessage mm = new MailMessage();

                mm.From = new MailAddress("yramot@myspace-inc.com", "Yadid Ramot");
                mm.IsBodyHtml = true;
                mm.BodyEncoding = Encoding.UTF8;
                
                foreach(String rec in recep)
                    mm.To.Add(new MailAddress(rec));

                mm.Subject = et.CompiledEmailSubject;
                mm.Body = et.CompiledTemplate;

                SmtpClient sc = new SmtpClient("fegplmsexmb16.ffe.foxeg.com");
                sc.EnableSsl = false;
                sc.UseDefaultCredentials = true;
                sc.Send(mm);
            }
            catch (Exception e)
            {
                if(log.IsErrorEnabled)
                    log.Error("Error Sending Email ", e);
            }
            return true;
        }        
    }
}
