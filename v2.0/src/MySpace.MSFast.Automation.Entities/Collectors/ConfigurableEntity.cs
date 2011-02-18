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
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace MySpace.MSFast.Automation.Entities.Collectors
{
    [Serializable]
    public class ConfigurableEntity : Entity
    {
        [EntityField("configuration")]
        public String RawConfig;

        [NonSerialized]
        private ExtCollectorsConfig _configuration;

        public ExtCollectorsConfig Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ExtCollectorsConfig();
                    
                    if(String.IsNullOrEmpty(this.RawConfig) == false)
                        _configuration.AppendConfig(new JSONCollectorsConfigLoader(this.RawConfig));
                }
                return _configuration;
            }
            set
            {
                this._configuration = value;
            }
        }

        public void UpdateConfiguration()
        {
            if (_configuration != null)
                this.RawConfig = new JSONCollectorsConfigWriter().SerializeConfig(this._configuration);
        }
    }

    public class ConfigurableEntityParameterAttribute : Attribute
    {
    }
}
