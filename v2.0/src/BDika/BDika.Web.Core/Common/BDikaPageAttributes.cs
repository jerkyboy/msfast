using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EYF.Web.Common;
using BDika.Entities.Users;

namespace BDika.Web.Core.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class BDikaPageAttributes : PageAttributes
    {
        public UserAttributes RequieredUserAttributes
        {
            set
            {
                this.IsRequieredUserAttributesSet = true;
                this._requieredUserAttributes = value;
            }
            get
            {
                return this._requieredUserAttributes;
            }
        }

        public UserAttributes RequieredMissingUserAttributes
        {
            set
            {
                this.IsRequieredMissingUserAttributesSet = true;
                this._requieredMissingUserAttributes = value;
            }
            get
            {
                return this._requieredMissingUserAttributes;
            }
        }

        public UserPermissions RequieredUserPermissions
        {
            set
            {
                this.IsRequieredUserPermissionsSet = true;
                this._requieredUserPermissions = value;
            }
            get
            {
                return this._requieredUserPermissions;
            }
        }

        public bool IsRequieredMissingUserAttributesSet = false;
        private UserAttributes _requieredMissingUserAttributes = UserAttributes.NoProperties;

        public bool IsRequieredUserAttributesSet = false;
        private UserAttributes _requieredUserAttributes = UserAttributes.NoProperties;

        public bool IsRequieredUserPermissionsSet = false;
        private UserPermissions _requieredUserPermissions = UserPermissions.NoPermission;
    }
}
