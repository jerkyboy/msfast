using System;
using System.Collections.Generic;
using System.Text;
using ResourceManager = System.Resources.ResourceManager;

namespace MySpace.MSFast.DataValidators
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DataValidatorAttribute : Attribute
    {
        public string ResourceFile { get; set; }


        public string Name { get; set; }
        public string HelpUrl { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        private string GetResourceValue(string Key, string DefaultValue)
        {
            if (string.IsNullOrEmpty(ResourceFile))
                return DefaultValue;
            if (null == _Manager)
                return DefaultValue;

            string Value = null;

            try
            {
                Value = _Manager.GetString(Key);
                if (string.IsNullOrEmpty(Value))
                    Value = DefaultValue;
            }
            catch
            {
                Value = DefaultValue;
            }


            return Value;
        }

        ResourceManager _Manager;

        internal void SetupResourceManager(System.Reflection.Assembly assembly)
        {
            if (string.IsNullOrEmpty(this.ResourceFile))
                return;
            try
            {
                _Manager = new ResourceManager(this.ResourceFile, assembly);
            }
            catch
            {

            }
        }

        public string NameKey { get; set; }
        public string HelpUrlKey { get; set; }
        public string GroupNameKey { get; set; }
        public string DescriptionKey { get; set; }

        internal string GetName()
        {
            return GetResourceValue(this.NameKey, this.Name);
        }
        internal string GetHelpUrl()
        {
            return GetResourceValue(this.HelpUrlKey, this.HelpUrl);
        }
        internal string GetGroupName()
        {
            return GetResourceValue(this.GroupNameKey, this.GroupName);
        }
        internal string GetDescription()
        {
            return GetResourceValue(this.DescriptionKey, this.Description);
        }

    }
}
