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
using EYF.Entities.StandardObjects;
using EYF.Core.Utils;
using EYF.Core.Security.Encryption;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Web.Core.Context;
using EYF.Web.Context;
using EYF.Web.Handlers.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Providers.Tests;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Tests
{
    [Linkable(Target = "/Handlers/Tests/DeleteTesterTypeHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class DeleteTesterTypeHandler : AsyncFormHandler<DeleteTesterTypeHandler>
    {
        [RequestFieldAttributes("t", false)]
        public DeleteTesterTypeToken DeleteTesterTypeToken;

        public override void ProcessForm()
        {
            if (this.DeleteTesterTypeToken == null ||
                !TesterTypeID.IsValidTesterTypeID(DeleteTesterTypeToken.TesterTypeID) ||
                String.IsNullOrEmpty(DeleteTesterTypeToken.TesterTypeName) ||
                !MSFAContext.Current.User.UserID.Equals(DeleteTesterTypeToken.UserID) ||
                EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime() > DeleteTesterTypeToken.Timestamp ||
                !TestsProvider.DeleteTesterType(MSFAContext.Current.User.UserID, DeleteTesterTypeToken.TesterTypeID))
            {
                MSFAContext.Current.AddUnexpectedError();
                return;
            }

            MSFAContext.Current.AddIndicator(new ResponseIndicator("tester_type_deleted", String.Format("{{testertypeid:{0},testertypename:\"{1}\"}}", DeleteTesterTypeToken.TesterTypeID, JSUtilities.EncodeJsString(DeleteTesterTypeToken.TesterTypeName))));

        }


        public static string GetTesterTypeToken(TesterTypeID testerTypeID, string testerTypeName)
        {
            if (TesterTypeID.IsValidTesterTypeID(testerTypeID) == false)
                return null;

            DeleteTesterTypeToken token = new DeleteTesterTypeToken();
            token.TesterTypeName = testerTypeName;
            token.TesterTypeID = testerTypeID;
            token.UserID = MSFAContext.Current.User.UserID;
            token.Timestamp = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime().Add(new TimeSpan(0, 3, 0));
            token.Encode();

            return token.Token;

        }
    }
    public class DeleteTesterTypeToken
    {
        public String TesterTypeName;
        public TesterTypeID TesterTypeID;
        public UserID UserID;
        public DateTime Timestamp;

        public String Token = "";

        public DeleteTesterTypeToken() { }
        private DeleteTesterTypeToken(String token)
        {
            this.Token = token;
            Decode();
        }

        public void Encode()
        {
            SimpleByteStream sbs = new SimpleByteStream();

            sbs.WriteUInt32((uint)this.TesterTypeID);
            sbs.WriteUInt32((uint)this.UserID);
            sbs.WriteString(this.TesterTypeName);
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

            if (sbs.ReadUInt32(ref tmpUint) == false) throw new ArgumentException("Invalid Token"); this.TesterTypeID = new TesterTypeID(tmpUint);
            if (sbs.ReadUInt32(ref tmpUint) == false) throw new ArgumentException("Invalid Token"); this.UserID = new UserID(tmpUint);
            if (sbs.ReadString(ref tmpString) == false) throw new ArgumentException("Invalid Token"); this.TesterTypeName = tmpString;
            if (sbs.ReadLong(ref tmpLong) == false) throw new ArgumentException("Invalid Token"); this.Timestamp = new DateTime(tmpLong);

            sbs.Dispose();
        }

        public static implicit operator DeleteTesterTypeToken(String token) { return new DeleteTesterTypeToken(token); }
        public static implicit operator String(DeleteTesterTypeToken t) { return (t != null ? t.Token : null); }
    }
}
