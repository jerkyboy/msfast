//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Entities)
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
using EYF.Entities;

namespace MySpace.MSFast.Automation.Entities.Users
{
    [Flags]
    public enum UserAttributes : uint
    {
        NoProperties = 0,
        IsRegisteredUser = 1
    }

    [Flags]
    public enum UserPermissions : uint
    {
        NoPermission = 0
    }

    public interface IMSFAUser : IUser 
    {
        new UserID UserID { get; set; }

        string FirstName { get; set; }
        string LastName { get; set; }

        EmailAddress EmailAddress { get; set; }
        string Password { get; set; }

        UserAttributes UserAttributes { get; set; }
        UserPermissions UserPermissions { get; set; }

        bool HasAttribute(UserAttributes p);
        void SetAttribute(UserAttributes p, bool b);

        bool HasPermission(UserPermissions p);
        void SetPermission(UserPermissions p, bool b);
    }

    [Serializable]
    [CachableEntity("User")]
    [DBEntity("users")]
    public class DBUser : Entity, IMSFAUser
    {
        [EntityIdentity("userid")]
        public UserID _UserID;
        public UserID UserID { get { return _UserID; } set { this._UserID = value; } }

        [EntityField("firstname")]
        public string _FirstName;
        public string FirstName { get { return _FirstName; } set { this._FirstName = value; } }

        [EntityField("lastname")]
        public string _LastName;
        public string LastName { get { return _LastName; } set { this._LastName = value; } }

        [EntityField("attributes")]
        public uint _RawUserAttributes;
        public UserAttributes UserAttributes
        {
            get
            {
                UserAttributes ps = (UserAttributes)this._RawUserAttributes;
                return ps;
            }
            set
            {
                this._RawUserAttributes = (uint)value;
            }
        }

        [EntityField("permissions")]
        public uint _RawUserPermissions;
        public UserPermissions UserPermissions
        {
            get
            {
                UserPermissions ps = (UserPermissions)this._RawUserPermissions;
                return ps;
            }
            set
            {
                this._RawUserPermissions = (uint)value;
            }
        }

        [EntityField("email")]
        public EmailAddress _EmailAddress;
        public EmailAddress EmailAddress { get { return _EmailAddress; } set { this._EmailAddress = value; } }

        [EntityField("password", true)]
        public string _Password;
        public string Password { get { return _Password; } set { this._Password = value; } }

        #region User Properties

        public bool HasAttribute(UserAttributes p)
        {
            return ((this.UserAttributes & p) == p);
        }

        public void SetAttribute(UserAttributes p, bool b)
        {
            if (b)
            {
                this.UserAttributes |= p;
            }
            else
            {
                this.UserAttributes &= (~p);
            }
        }

        public bool HasPermission(UserPermissions p)
        {
            return ((this.UserPermissions & p) == p);
        }

        public void SetPermission(UserPermissions p, bool b)
        {
            if (b)
            {
                this.UserPermissions |= p;
            }
            else
            {
                this.UserPermissions &= (~p);
            }
        }

        #endregion
    }
}
