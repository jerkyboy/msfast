using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.Core.Configuration.Common
{
    public interface CollectionMetaInfo
    {
        int CollectionID { get; }
        String DumpFolder { get; }
    }
}
