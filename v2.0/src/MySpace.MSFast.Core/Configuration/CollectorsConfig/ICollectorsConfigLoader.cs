﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.Core.Configuration.CollectorsConfig
{
    public interface ICollectorsConfigLoader
    {
        void LoadConfig(CollectorsConfig cc);
    }
}