//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Core)
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
using System.Linq;
using System.Text;
using EYF.Web.Context;
using System.Web;
using EYF.Entities.StandardObjects;
using MySpace.MSFast.Automation.Entities.Users;
using System.Text.RegularExpressions;
using EYF.Core.Configuration;
using EYF.Web.Common.StaticFilesManagement;

namespace MySpace.MSFast.Automation.Web.Core.Context
{
    public enum SiteType
    {
        Unknown = 0,
        MainSite = 1,
    }
    public abstract class MSFAContext : CurrentContext
    {
        #region Get Current

        public static new MSFAContext Current
        {
            get
            {
                return (MSFAContext)CurrentContext.GetCurrent(HttpContext.Current);
            }
        }
       
        #endregion

        public override IUser IUser
        {
            get { return User; }
        }
        private MSFAContextUser _user = null;
        public MSFAContextUser User
        {
            get
            {
                if (_user == null)
                {
                    _user = MSFAContextUser.Default().Decode(MSFACookieJar.EncryptedWebUser, IPAddress);

                    if (_user == null)
                    {
                        _user = MSFAContextUser.Default();
                        MSFACookieJar.EncryptedWebUser = null;
                    }
                }
                return _user;
            }
            set
            {
                if (value == null) MSFACookieJar.EncryptedWebUser = null;
                else MSFACookieJar.EncryptedWebUser = value.Encode(IPAddress);
                
                _user = value;
            }
        }

        #region SiteType
        public static Regex MainHostMatch = new Regex((String)AppConfig.Instance["Site.IsOnMainDomainRegex"], RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private SiteType _siteType = SiteType.Unknown;
        public SiteType SiteType
        {
            get
            {
                if (_siteType == SiteType.Unknown)
                {
                    String host = HttpContext.Current.Request.Url.Host.Trim();
                    if(MainHostMatch.IsMatch(host)){
                        _siteType = SiteType.MainSite;
                    }
                }
                return _siteType;
            }
        }
        #endregion
               
        public override void Update()
        {
            if (_user != null && _user.HasChanged) this.User = _user;            
        }
    }
}
