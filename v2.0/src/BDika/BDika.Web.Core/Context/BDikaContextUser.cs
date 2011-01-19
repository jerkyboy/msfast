using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities.StandardObjects;
using BDika.Entities.Users;
using EYF.Web.Context;

namespace BDika.Web.Core.Context
{
    public class BDikaContextUser : ContextObject<BDikaContextUser>, IBDikaUser
    {
        #region Properties

        private UserID _UserID = 0;
        public UserID UserID { get { return _UserID; } set { _UserID = value; SetChanged(); } }

        private String _FirstName = null;
        public String FirstName { get { return _FirstName; } set { _FirstName = value; SetChanged(); } }
        
        private UserAttributes _UserAttributes = UserAttributes.NoProperties;
        public UserAttributes UserAttributes { get { return _UserAttributes; } set { _UserAttributes = value; SetChanged(); } }

        private UserPermissions _UserPermissions = UserPermissions.NoPermission;
        public UserPermissions UserPermissions { get { return _UserPermissions; } set { _UserPermissions = value; SetChanged(); } }

        public bool HasAttribute(UserAttributes p)
        {
            return ((this._UserAttributes & p) == p);
        }
        public void SetAttribute(UserAttributes p, bool b)
        {
            if (b)
            {
                this._UserAttributes |= p;
            }
            else
            {
                this._UserAttributes &= (~p);
            }
            SetChanged();
        }

        public bool HasPermission(UserPermissions p)
        {
            return ((this._UserPermissions & p) == p);
        }
        public void SetPermission(UserPermissions p, bool b)
        {
            if (b)
            {
                this._UserPermissions |= p;
            }
            else
            {
                this._UserPermissions &= (~p);
            }
            SetChanged();
        }
 
        #endregion

        private BDikaContextUser()
        {
        }

        public void CopyUser(IBDikaUser euser) 
        {
            if (euser == null || euser.UserID == 0) return;

            this.UserID = euser.UserID;
            this.FirstName = euser.FirstName;
            this.UserAttributes = euser.UserAttributes;
            this.UserPermissions = euser.UserPermissions;
        }

        public static BDikaContextUser Default()
        {
            BDikaContextUser user = new BDikaContextUser();
            user._UserAttributes = UserAttributes.NoProperties;
            user._UserPermissions = UserPermissions.NoPermission;
            user.UpToDate();
            return user;
        }

        public override BDikaContextUser FromCookie(CookieByteStream cookie)
        {
            if (cookie == null || cookie.isValid() == false)
                return null;

            uint tempUint = 0;

            if (cookie.ReadUInt32(ref tempUint) == false) return null; this._UserAttributes = (UserAttributes)Enum.ToObject(typeof(UserAttributes), tempUint);
            if (cookie.ReadUInt32(ref tempUint) == false) return null; this._UserPermissions = (UserPermissions)Enum.ToObject(typeof(UserPermissions), tempUint);
            if (cookie.ReadUInt32(ref tempUint) == false) return null; this._UserID = tempUint;
            if (cookie.ReadString(ref this._FirstName) == false) return null;

            this.UpToDate();
          
            return this;
        }

        public override CookieByteStream ToCookie(uint ipAddress)
        {
            if (this.HasAttribute(UserAttributes.IsRegisteredUser) == false)
                return null;

            CookieByteStream cbs = new CookieByteStream(ipAddress);

            if (cbs.WriteUInt32((uint)this.UserAttributes) == false) return null;
            if (cbs.WriteUInt32((uint)this.UserPermissions) == false) return null;
            if (cbs.WriteUInt32(this.UserID) == false) return null;
            if (cbs.WriteString(this.FirstName) == false) return null;

            return cbs;
        }
    }
}
