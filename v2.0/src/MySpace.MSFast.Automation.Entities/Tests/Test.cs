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
using EYF.Entities;
using EYF.Core.Exceptions;
using MySpace.MSFast.Automation.Entities.Collectors;

namespace MySpace.MSFast.Automation.Entities.Tests
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

        [EntityField("testname",Size = 45)]
        public String TestName;
      
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
