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
using EYF.Entities.StandardObjects;
using MySpace.MSFast.Automation.Entities.Users;
using EYF.Web.Context;

namespace MySpace.MSFast.Automation.Web.Core.Context
{
    public class MSFAContextUser : ContextObject<MSFAContextUser>, IMSFAUser
    {
        #region Properties

        private UserID _UserID = 0;
        public UserID UserID { get { return _UserID; } set { _UserID = value; SetChanged(); } }

        private String _FirstName = null;
        public String FirstName { get { return _FirstName; } set { _FirstName = value; SetChanged(); } }

        private String _LastName = null;
        public String LastName { get { return _LastName; } set { _LastName = value; SetChanged(); } }

        private EmailAddress _Email = null;
        public EmailAddress EmailAddress { get { return _Email; } set { _Email = value; SetChanged(); } }

        public string Password { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

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

        private MSFAContextUser()
        {
        }

        public void CopyUser(IMSFAUser euser) 
        {
            if (euser == null || euser.UserID == 0) return;

            this.UserID = euser.UserID;
            this.FirstName = euser.FirstName;
            this.LastName = euser.LastName;
            this.EmailAddress = euser.EmailAddress;
            this.UserAttributes = euser.UserAttributes;
            this.UserPermissions = euser.UserPermissions;
        }

        public static MSFAContextUser Default()
        {
            MSFAContextUser user = new MSFAContextUser();
            user._UserAttributes = UserAttributes.NoProperties;
            user._UserPermissions = UserPermissions.NoPermission;
            user.UpToDate();
            return user;
        }

        public override MSFAContextUser FromCookie(CookieByteStream cookie)
        {
            if (cookie == null || cookie.isValid() == false)
                return null;

            uint tempUint = 0;
            String tempString = null;

            if (cookie.ReadUInt32(ref tempUint) == false) return null; this._UserAttributes = (UserAttributes)Enum.ToObject(typeof(UserAttributes), tempUint);
            if (cookie.ReadUInt32(ref tempUint) == false) return null; this._UserPermissions = (UserPermissions)Enum.ToObject(typeof(UserPermissions), tempUint);
            if (cookie.ReadUInt32(ref tempUint) == false) return null; this._UserID = tempUint;
            if (cookie.ReadString(ref this._FirstName) == false) return null;
            if (cookie.ReadString(ref this._LastName) == false) return null;
            if (cookie.ReadString(ref tempString) == false) return null; this._Email = tempString;

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
            if (cbs.WriteString(this.LastName) == false) return null;
            if (cbs.WriteString(this.EmailAddress.ColumnValue.ToString()) == false) return null;

            return cbs;
        }
    }
}
