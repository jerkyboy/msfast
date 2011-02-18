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
using EYF.Web.Handlers.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Entities.Triggers;
using EYF.Entities.StandardObjects;
using MySpace.MSFast.Automation.Web.Core.Context;
using MySpace.MSFast.Automation.Providers.Triggers;
using EYF.Web.Context;
using EYF.Core.Utils;
using EYF.Core.Security.Encryption;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.Triggers
{
    [Linkable(Target = "/Handlers/Triggers/DeleteTriggerHandler.axd")]
    [MSFAPageAttributes(RequieredUserAttributes = UserAttributes.IsRegisteredUser)]
    public class DeleteTriggerHandler : AsyncFormHandler<DeleteTriggerHandler>
    {
        [RequestFieldAttributes("t", false)]
        public DeleteTriggerToken DeleteTriggerToken;

        public override void ProcessForm()
        {
            if (this.DeleteTriggerToken == null ||
                !TriggerID.IsValidTriggerID(DeleteTriggerToken.TriggerID) ||
                String.IsNullOrEmpty(DeleteTriggerToken.TriggerName) ||
                !MSFAContext.Current.User.UserID.Equals(DeleteTriggerToken.UserID) ||
                EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime() > DeleteTriggerToken.Timestamp ||
                !TriggersProvider.DeleteTrigger(MSFAContext.Current.User.UserID, DeleteTriggerToken.TriggerID))
            {
                MSFAContext.Current.AddUnexpectedError();
                return;
            }

            MSFAContext.Current.AddIndicator(new ResponseIndicator("trigger_deleted", String.Format("{{triggerid:{0},triggername:\"{1}\"}}", DeleteTriggerToken.TriggerID, JSUtilities.EncodeJsString(DeleteTriggerToken.TriggerName))));

        }


        public static string GetTriggerToken(TriggerID TriggerID, string triggerName)
        {
            if (TriggerID.IsValidTriggerID(TriggerID) == false)
                return null;

            DeleteTriggerToken token = new DeleteTriggerToken();
            token.TriggerName = triggerName;
            token.TriggerID = TriggerID;
            token.UserID = MSFAContext.Current.User.UserID;
            token.Timestamp = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime().Add(new TimeSpan(0, 3, 0));
            token.Encode();

            return token.Token;

        }
    }
    public class DeleteTriggerToken
    {
        public String TriggerName;
        public TriggerID TriggerID;
        public UserID UserID;
        public DateTime Timestamp;

        public String Token = "";

        public DeleteTriggerToken() { }
        private DeleteTriggerToken(String token)
        {
            this.Token = token;
            Decode();
        }

        public void Encode()
        {
            SimpleByteStream sbs = new SimpleByteStream();

            sbs.WriteUInt32((uint)this.TriggerID);
            sbs.WriteUInt32((uint)this.UserID);
            sbs.WriteString(this.TriggerName);
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

            if (sbs.ReadUInt32(ref tmpUint) == false) throw new ArgumentException("Invalid Token"); this.TriggerID = new TriggerID(tmpUint);
            if (sbs.ReadUInt32(ref tmpUint) == false) throw new ArgumentException("Invalid Token"); this.UserID = new UserID(tmpUint);
            if (sbs.ReadString(ref tmpString) == false) throw new ArgumentException("Invalid Token"); this.TriggerName = tmpString;
            if (sbs.ReadLong(ref tmpLong) == false) throw new ArgumentException("Invalid Token"); this.Timestamp = new DateTime(tmpLong);

            sbs.Dispose();
        }

        public static implicit operator DeleteTriggerToken(String token) { return new DeleteTriggerToken(token); }
        public static implicit operator String(DeleteTriggerToken t) { return (t != null ? t.Token : null); }
    }
}
