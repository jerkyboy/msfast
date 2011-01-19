using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Web.Context;
using System.Web;
using EYF.Entities.StandardObjects;
using BDika.Entities.Users;
using System.Text.RegularExpressions;
using EYF.Core.Configuration;
using EYF.Web.Common.StaticFilesManagement;

namespace BDika.Web.Core.Context
{
    public enum SiteType
    {
        Unknown = 0,
        MainSite = 1,
    }
    public abstract class BDikaContext : CurrentContext
    {
        #region Get Current

        public static new BDikaContext Current
        {
            get
            {
                return (BDikaContext)CurrentContext.GetCurrent(HttpContext.Current);
            }
        }
       
        #endregion

        public override IUser IUser
        {
            get { return User; }
        }
        private BDikaContextUser _user = null;
        public BDikaContextUser User
        {
            get
            {
                if (_user == null)
                {
                    _user = BDikaContextUser.Default().Decode(BDikaCookieJar.EncryptedWebUser, IPAddress);

                    if (_user == null)
                    {
                        _user = BDikaContextUser.Default();
                        BDikaCookieJar.EncryptedWebUser = null;
                    }
                }
                return _user;
            }
            set
            {
                if (value == null) BDikaCookieJar.EncryptedWebUser = null;
                else BDikaCookieJar.EncryptedWebUser = value.Encode(IPAddress);
                
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
