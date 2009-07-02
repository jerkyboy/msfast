//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors)
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
using System.Text;
using MySpace.MSFast.DataProcessors;

namespace MySpace.MSFast.DataValidators
{ 
    public interface IDataValidator
    {
		String HelpURL{get;set;}
		String Description{get;set;}
		String Name{get;set;}
        String GroupName{get;set;}
        IValidationResults Validate(ProcessedDataPackage package);
		void Dispose();
        void Init(Dictionary<string, string> config);
    }

    public abstract class DataValidator<T> : IDataValidator where T : IValidationResults 
	{
		private String _helpURL = "";
		private String _description = "";
        private String _name = "";
        private String _groupName = "";

        public String HelpURL
        {
            get { return this._helpURL; }
            set { this._helpURL = value; }
        }
        public String GroupName
        {
            get { return this._groupName; }
            set { this._groupName = value; }
        }
		public String Description
		{
			get { return this._description; }
			set { this._description = value; } 
		}
		public String Name
		{
			get { return this._name; }
			set { this._name = value; } 
		}

        public IValidationResults Validate(ProcessedDataPackage package)
        {
            return this.ValidateData(package);
        }

        public abstract T ValidateData(ProcessedDataPackage package);
		
		public virtual void Dispose()
        {
            this._description = null;
            this._name = null;
            this._helpURL = null;
        }
		
        public virtual void Init(Dictionary<string, string> config){}
	}
}
