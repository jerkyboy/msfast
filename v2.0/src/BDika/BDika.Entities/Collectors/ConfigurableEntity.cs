using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Entities;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Entities.Collectors
{
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
