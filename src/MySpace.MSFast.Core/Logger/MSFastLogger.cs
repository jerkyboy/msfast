//=======================================================================
/* Project: MSFast (MySpace.MSFast.Core)
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
#if (DEBUG)
using log4net;
#endif
using System.Reflection;
using System.IO;

namespace MySpace.MSFast.Core.Logger
{
	public class MSFastLogger
	{

		static MSFastLogger() 
		{
#if (DEBUG)
            try
            {
                Stream configStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MySpace.MSFast.Core.LoggingConfig.xml");
                log4net.Config.XmlConfigurator.Configure(configStream);
                configStream.Close();
            }
            catch 
            {
            }
#endif
		}
		
#if (DEBUG)
        private ILog logger = null;
#endif
		public static MSFastLogger GetLogger(Type t) 
		{
			return new MSFastLogger(t);
		}

		private MSFastLogger(Type t){
#if (DEBUG)		
            this.logger =  LogManager.GetLogger(t);
#endif
		}


		#region Delegate to LOG4NET logger
        
#if (DEBUG)
		public bool IsDebugEnabled { get { return this.logger.IsDebugEnabled; } }
		public bool IsErrorEnabled { get { return this.logger.IsErrorEnabled; } }
		public bool IsFatalEnabled { get { return this.logger.IsFatalEnabled; } }
		public bool IsInfoEnabled { get { return this.logger.IsInfoEnabled; } }
		public bool IsWarnEnabled { get { return this.logger.IsWarnEnabled; } }

		public void Debug(object message) { this.logger.Debug(message) ;}
		public void Debug(object message, Exception exception){ this.logger.Debug(message, exception) ;}
		public void DebugFormat(string format, object arg0){ this.logger. DebugFormat( format,  arg0) ;}
		public void DebugFormat(string format, params object[] args){ this.logger.DebugFormat( format, args) ;}
		public void DebugFormat(IFormatProvider provider, string format, params object[] args){ this.logger.DebugFormat( provider,  format, args) ;}
		public void DebugFormat(string format, object arg0, object arg1){ this.logger.DebugFormat(format, arg0, arg1) ;}
		public void DebugFormat(string format, object arg0, object arg1, object arg2){ this.logger.DebugFormat(format, arg0, arg1, arg2) ;}
		public void Error(object message){ this.logger.Error(message) ;}
		public void Error(object message, Exception exception){ this.logger.Error(message, exception) ;}
		public void ErrorFormat(string format, object arg0){ this.logger.ErrorFormat(format, arg0) ;}
		public void ErrorFormat(string format, params object[] args){ this.logger.ErrorFormat(format, args) ;}
		public void ErrorFormat(IFormatProvider provider, string format, params object[] args){ this.logger.ErrorFormat(provider, format, args) ;}
		public void ErrorFormat(string format, object arg0, object arg1){ this.logger.ErrorFormat(format, arg0, arg1) ;}
		public void ErrorFormat(string format, object arg0, object arg1, object arg2){ this.logger.ErrorFormat(format, arg0, arg1, arg2) ;}
		public void Fatal(object message){ this.logger.Fatal(message) ;}
		public void Fatal(object message, Exception exception){ this.logger.Fatal(message, exception) ;}
		public void FatalFormat(string format, object arg0){ this.logger.FatalFormat(format, arg0) ;}
		public void FatalFormat(string format, params object[] args){ this.logger.FatalFormat(format, args) ;}
		public void FatalFormat(IFormatProvider provider, string format, params object[] args){ this.logger.FatalFormat(provider, format, args) ;}
		public void FatalFormat(string format, object arg0, object arg1){ this.logger.FatalFormat(format, arg0, arg1) ;}
		public void FatalFormat(string format, object arg0, object arg1, object arg2){ this.logger.FatalFormat(format, arg0, arg1, arg2) ;}
		public void Info(object message){ this.logger.Info(message) ;}
		public void Info(object message, Exception exception){ this.logger.Info(message, exception) ;}
		public void InfoFormat(string format, object arg0){ this.logger.InfoFormat(format, arg0) ;}
		public void InfoFormat(string format, params object[] args){ this.logger.InfoFormat(format, args) ;}
		public void InfoFormat(IFormatProvider provider, string format, params object[] args){ this.logger.InfoFormat(provider, format, args) ;}
		public void InfoFormat(string format, object arg0, object arg1){ this.logger.InfoFormat(format, arg0, arg1) ;}
		public void InfoFormat(string format, object arg0, object arg1, object arg2){ this.logger.InfoFormat(format, arg0, arg1, arg2) ;}
		public void Warn(object message){ this.logger.Warn(message) ;}
		public void Warn(object message, Exception exception){ this.logger.Warn(message, exception) ;}
		public void WarnFormat(string format, object arg0){ this.logger.WarnFormat(format, arg0) ;}
		public void WarnFormat(string format, params object[] args){ this.logger.WarnFormat(format, args) ;}
		public void WarnFormat(IFormatProvider provider, string format, params object[] args){ this.logger.WarnFormat(provider, format, args) ;}
		public void WarnFormat(string format, object arg0, object arg1){ this.logger.WarnFormat(format, arg0, arg1) ;}
		public void WarnFormat(string format, object arg0, object arg1, object arg2){ this.logger.WarnFormat(format, arg0, arg1, arg2) ; }
#endif
        #endregion
    }
}
