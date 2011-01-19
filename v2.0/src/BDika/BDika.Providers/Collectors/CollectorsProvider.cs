using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Entities.Tests;
using EYF.Providers.Gateway;

namespace BDika.Providers.Collectors
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

            return EntitiesGateway.GetEntity<TesterType>(indx.TesterTypeID);
        }
    }
}
