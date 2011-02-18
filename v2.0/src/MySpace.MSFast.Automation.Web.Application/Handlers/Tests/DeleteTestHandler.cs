//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Application)
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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Entities.StandardObjects;
using EYF.Core.Utils;
using EYF.Core.Security.Encryption;
using MySpace.MSFast.Automation.Web.Core.Context;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Handlers.Common;
using MySpace.MSFast.Automation.Providers.Tests;
using EYF.Web.Context;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/DeleteTestHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class DeleteTestHandler : AsyncFormHandler<DeleteTestHandler>
    {
        [RequestFieldAttributes("t", false)]
        public DeleteTestToken DeleteTestToken;

        public override void ProcessForm()
        {
            if (this.DeleteTestToken == null ||
                !TestID.IsValidTestID(DeleteTestToken.TestID) ||
                String.IsNullOrEmpty(DeleteTestToken.TestName) ||
                !MSFAContext.Current.User.UserID.Equals(DeleteTestToken.UserID) ||
                EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime() > DeleteTestToken.Timestamp ||
                !TestsProvider.DeleteTest(MSFAContext.Current.User.UserID, DeleteTestToken.TestID))
            {
                MSFAContext.Current.AddUnexpectedError();
                return;
            }

            MSFAContext.Current.AddIndicator(new ResponseIndicator("test_deleted", String.Format("{{testid:{0},testname:\"{1}\"}}", DeleteTestToken.TestID, JSUtilities.EncodeJsString(DeleteTestToken.TestName))));

        }


        public static string GetTestToken(TestID testID, string testName)
        {
            if (TestID.IsValidTestID(testID) == false)
                return null;

            DeleteTestToken token = new DeleteTestToken();
            token.TestName = testName;
            token.TestID = testID;
            token.UserID = MSFAContext.Current.User.UserID;
            token.Timestamp = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime().Add(new TimeSpan(0, 3, 0));
            token.Encode();

            return token.Token;

        }
    }
    public class DeleteTestToken
    {
        public String TestName;
        public TestID TestID;
        public UserID UserID;
        public DateTime Timestamp;

        public String Token = "";

        public DeleteTestToken() { }
        private DeleteTestToken(String token)
        {
            this.Token = token;
            Decode();
        }

        public void Encode()
        {
            SimpleByteStream sbs = new SimpleByteStream();

            sbs.WriteUInt32((uint)this.TestID);
            sbs.WriteUInt32((uint)this.UserID);
            sbs.WriteString(this.TestName);
            sbs.WriteLong(this.Timestamp.Ticks);

            sbs.Flush();

            byte[] s = StrongEncryption.FormEncryption.Encode(sbs.ByteArray());

            sbs.Dispose();

            this.Token = Convert.ToBase64String(s);
        }
        private void Decode()
        {
            byte[] data = Convert.FromBase64String(this.Token);
            data = StrongEncryption.FormEncryption.Decode(data);

            SimpleByteStream sbs = new SimpleByteStream(data);

            String tmpString = null;
            uint tmpUint = 0;
            long tmpLong = 0;

            if (sbs.ReadUInt32(ref tmpUint) == false) throw new ArgumentException("Invalid Token"); this.TestID = new TestID(tmpUint);
            if (sbs.ReadUInt32(ref tmpUint) == false) throw new ArgumentException("Invalid Token"); this.UserID = new UserID(tmpUint);
            if (sbs.ReadString(ref tmpString) == false) throw new ArgumentException("Invalid Token"); this.TestName = tmpString;
            if (sbs.ReadLong(ref tmpLong) == false) throw new ArgumentException("Invalid Token"); this.Timestamp = new DateTime(tmpLong);

            sbs.Dispose();
        }

        public static implicit operator DeleteTestToken(String token) { return new DeleteTestToken(token); }
        public static implicit operator String(DeleteTestToken t) { return (t != null ? t.Token : null); }
    }
}
