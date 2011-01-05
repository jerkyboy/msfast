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
using System.Collections;

namespace MySpace.MSFast.DataValidators
{
	public interface IValidationResults : IList 
	{
		int Score{set;get;}
        String ResultsExplenation { set; get; }
        String GroupName { set; get; }
        IDataValidator Validator { set; get; }
	}
	
	public class ValidationResults<T> : List<T>, IValidationResults
	{
		private int _score = 0;
		public int Score
		{
			set 
			{
				if (value > 100)
					value = 100;
				else if (value < 0)
					value = 0;
				_score = value;
			}
			get { return _score; }
		}

        private IDataValidator _dataValidator = null;
        public IDataValidator Validator
		{
			set { _dataValidator = value; }
			get { return _dataValidator; }
		}

        private String _resultsExplenation = null;
        public String ResultsExplenation
        {
            set { _resultsExplenation = value; }
            get { return _resultsExplenation; }
        }

        private String _groupName = null;
        public String GroupName
        {
            set { _groupName = value; }
            get { return _groupName; }
        }

		public override string ToString()
		{
            return "Validation Results for:[" + _dataValidator.Name + "] [Score:" + this.Score + "]";
		}


    }
}
