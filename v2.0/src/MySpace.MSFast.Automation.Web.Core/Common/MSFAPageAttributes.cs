//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Core)
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
using EYF.Web.Common;
using MySpace.MSFast.Automation.Entities.Users;

namespace MySpace.MSFast.Automation.Web.Core.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MSFAPageAttributes : PageAttributes
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
