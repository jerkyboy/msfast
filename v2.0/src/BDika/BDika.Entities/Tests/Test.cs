using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;
using EYF.Core.Exceptions;
using BDika.Entities.Collectors;

namespace BDika.Entities.Tests
{
    #region TestID
    public class InvalidTestIDException : EYFException { }

    [Serializable]
    public class TestID : EntityIdentifier<TestID>
    {
        public static bool ValidateTestID(TestID TestID)
        {
            if (IsValidTestID(TestID) == false) throw new InvalidTestIDException();
            return true;
        }
        public static bool IsValidTestID(TestID TestID)
        {
            return TestID != 0;
        }

        private uint _id;

        public TestID(uint _id)
        {
            this._id = _id;
        }

        public static implicit operator TestID(uint i) { return new TestID(i); }
        public static implicit operator uint(TestID i) { return (i != null ? i._id : 0); }

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
    [CachableEntity("Test")]
    [DBEntity("tests")]
    public class Test : ConfigurableEntity
    {
        [EntityIdentity("testid")]
        public TestID TestID;

        [EntityField("enabled")]
        public bool Enabled;

        [NonSerialized]
        private String _TestName;
        [ConfigurableEntityParameter]
        public String TestName
        {
            get
            {
                if (String.IsNullOrEmpty(_TestName) == false)
                    return _TestName;

                if (Configuration != null)
                    return _TestName = Configuration.GetArgumentValue("test_name");

                return null;
            }
            set
            {
                _TestName = value;
                Configuration.SetArgument("test_name", value);
                UpdateConfiguration();
            }
        }

        [NonSerialized]
        private Uri _TestURL;
        [ConfigurableEntityParameter]
        public Uri TestURL
        {
            get
            {
                if (_TestURL != null)
                    return _TestURL;
                
                try
                {
                    return _TestURL = new Uri(String.Concat(Configuration.GetArgumentValue("test_protocol"), "://", Configuration.GetArgumentValue("test_domain"), Configuration.GetArgumentValue("test_path"), Configuration.GetArgumentValue("test_query_string")));
                }
                catch
                {
                }
                return null;
            }
            set
            {
                _TestURL = value;

                if (_TestURL != null)
                {
                    Configuration.SetArgument("test_protocol",_TestURL.Scheme);
                    Configuration.SetArgument("test_domain",_TestURL.Host);
                    Configuration.SetArgument("test_path",_TestURL.AbsolutePath);
                    Configuration.SetArgument("test_query_string",_TestURL.Query);
                    UpdateConfiguration();
                }
            }
        }
    }
}
