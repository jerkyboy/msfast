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
using EYF.Core.Exceptions;
using EYF.Entities;
using MySpace.MSFast.Automation.Entities.Collectors;

namespace MySpace.MSFast.Automation.Entities.Tests
{
    #region TesterTypeID
    public class InvalidTesterTypeIDException : EYFException { }
    public class InvalidTesterClientIDException : EYFException { }
    public class InvalidTesterClientKeyException : EYFException { }

    [Serializable]
    public class TesterTypeID : EntityIdentifier<TesterTypeID>
    {
        public static bool ValidateTesterTypeID(TesterTypeID TesterTypeID)
        {
            if (IsValidTesterTypeID(TesterTypeID) == false) throw new InvalidTesterTypeIDException();
            return true;
        }
        public static bool IsValidTesterTypeID(TesterTypeID TesterTypeID)
        {
            return TesterTypeID != 0;
        }

        private uint _id;

        public TesterTypeID(uint _id)
        {
            this._id = _id;
        }

        public static implicit operator TesterTypeID(uint i) { return new TesterTypeID(i); }
        public static implicit operator uint(TesterTypeID i) { return (i != null ? i._id : 0); }

        public override string ToString() { return UniqueIdetifier; }

        public override String UniqueIdetifier
        {
            get
            {
                return this._id.ToString();
            }
        }
        public override object ColumnValue
        {
            get
            {
                return this._id;
            }
        }
        public override int GetHashCode() { return (int)_id; }
    }
    #endregion

    [Serializable]
    [CachableEntity("TesterType")]
    [DBEntity("testertypes")]
    public class TesterType : ConfigurableEntity
    {
        [EntityIdentity("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("name", Size = 45)]
        public String Name;

        [EntityField("clientid", Size = 45)]
        public ClientID ClientID = Guid.NewGuid().ToString().Replace("-","").Substring(0,10).ToUpper();

        [EntityField("clientkey", Size = 45)]
        public ClientKey ClientKey = Guid.NewGuid().ToString().Replace("-", "");

        [EntityField("lastping")]
        public DateTime LastPing = new DateTime(1971, 1, 1);

        public bool Enabled = true;

    }

    [EntityCommand(EntityCommandType.Update, typeof(TesterType))]
    [DBEntity("testertypes")]
    public class UpdateTesterTypeLastPingEntityCommand : Entity
    {
        [EntityIdentity("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("lastping")]
        public DateTime LastPing = new DateTime(1971,1,1);
    }

    [EntityCommand(EntityCommandType.Update, typeof(TesterType))]
    [DBEntity("testertypes")]
    public class UpdateTesterTypeEntityCommand : Entity
    {
        [EntityIdentity("testertypeid")]
        public TesterTypeID TesterTypeID;

        [EntityField("name", Size = 45)]
        public String Name;
    }

    [Serializable]
    [CachableEntity("ClientIDClientKeyTesterTypeEntityIndex")]
    [DBEntity("testertypes")]
    public class ClientIDClientKeyTesterTypeEntityIndex : Entity
    {
        [EntityIdentity("clientid", Size = 45)]
        public ClientID ClientID;

        [EntityField("clientkey", Size = 45)]
        public ClientKey ClientKey;

        [EntityField("testertypeid")]
        public TesterTypeID TesterTypeID;
    }

    [Serializable]
    public class ClientID : CEntityIdentifier
    {
        public static bool IsValidClientID(ClientID ClientID)
        {
            return (ClientID != null && String.IsNullOrEmpty(ClientID._id) == false);
        }

        private String _id;

        public ClientID(String _id)
        {
            if (_id == null)
                throw new NullReferenceException();

            this._id = _id.ToLower();
        }

        public static implicit operator ClientID(String i) { return new ClientID(i); }
        public static implicit operator String(ClientID i) { return (i != null ? i._id : null); }

        public override string ToString() { return UniqueIdetifier; }

        public override String UniqueIdetifier
        {
            get
            {
                return this._id.ToString().ToLower();
            }
        }
        public override object ColumnValue
        {
            get
            {
                return this._id.ToString().ToLower();
            }
        }
    }

    [Serializable]
    public class ClientKey : CEntityIdentifier
    {
        public static bool IsValidClientKey(ClientKey ClientKey)
        {
            return (ClientKey != null && String.IsNullOrEmpty(ClientKey._id) == false);
        }

        private String _id;

        public ClientKey(String _id)
        {
            if (_id == null)
                throw new NullReferenceException();

            this._id = _id.ToLower();
        }

        public static implicit operator ClientKey(String i) { return new ClientKey(i); }
        public static implicit operator String(ClientKey i) { return (i != null ? i._id : null); }

        public override string ToString() { return UniqueIdetifier; }

        public override String UniqueIdetifier
        {
            get
            {
                return this._id.ToString().ToLower();
            }
        }
        public override object ColumnValue
        {
            get
            {
                return this._id.ToString().ToLower();
            }
        }
    }
}
