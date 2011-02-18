//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.Providers)
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
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using MySpace.MSFast.Automation.Entities.Tests;
using EYF.Providers.Gateway;

namespace MySpace.MSFast.Automation.Providers.Collectors
{
    public static class CollectorsProvider
    {   
        public static TesterType GetTesterType(ClientID clientID, ClientKey clientKey)
        {
            if(ClientKey.IsValidClientKey(clientKey) == false) throw new InvalidTesterClientKeyException();
            if(ClientID.IsValidClientID(clientID) == false) throw new InvalidTesterClientIDException();

            ClientIDClientKeyTesterTypeEntityIndex indx = EntitiesGateway.GetEntity<ClientIDClientKeyTesterTypeEntityIndex>(clientID);

            if (indx == null || ClientKey.IsValidClientKey(indx.ClientKey) == false) throw new InvalidTesterClientIDException();
            if (clientKey.Equals(indx.ClientKey) == false) throw new InvalidTesterClientKeyException();


            TesterType tt = EntitiesGateway.GetEntity<TesterType>(indx.TesterTypeID);

            if (tt != null)
            {
                EntitiesGateway.UpdateEntity(new UpdateTesterTypeLastPingEntityCommand()
                {
                    TesterTypeID = tt.TesterTypeID,
                    LastPing = EYF.Core.Atom.AtomClock.GetCurrentAtomDateTime()
                });
            }

            return tt;
        }
    }
}
