using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Core.Exceptions;
using EYF.Entities;
using BDika.Entities.Collectors;

namespace BDika.Entities.Tests
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
        public ClientID ClientID;

        [EntityField("clientkey", Size = 45)]
        public ClientKey ClientKey;
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
