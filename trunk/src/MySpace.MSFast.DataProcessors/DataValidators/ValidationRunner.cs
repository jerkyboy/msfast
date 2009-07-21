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
using System.IO;
using System.Reflection;
using System.Xml;
using MySpace.MSFast.DataProcessors;
using System.Threading;
using MySpace.MSFast.DataProcessors.DataValidators.ValidationResultTypes;

namespace MySpace.MSFast.DataValidators
{
	public class ValidationRunner
	{
		public delegate void OnValidatorEventHandler(ValidationRunner sender);
		public delegate void OnValidatorProgressEventHandler(ValidationRunner sender, 
                                                             IDataValidator  validator,
                                                             IValidationResults results, bool success);

		public event OnValidatorEventHandler OnValidationStarted;
		public event OnValidatorEventHandler OnValidationEnded;
		public event OnValidatorProgressEventHandler OnValidatorProgress;

        private List<IDataValidator> validators = null;

		public int GetValidatorsCount()
		{
			if (validators != null)
			{
                return validators.Count;
			}

			return 0;
		}

		public bool IsRunning = false;
		private object runningLock = new object();
	
		public ValidationRunner() 
		{
            this.validators = new List<IDataValidator>();
		}
		
		public void Dispose()
		{
			IsRunning = false;
			lock (validators)
			{
                foreach (IDataValidator l in this.validators)
                {
                    try
                    {
                        l.Dispose();
                    }
                    catch { }
                }

				this.validators.Clear();
			}
		}

		#region Runner

		public void Validate(ProcessedDataPackage metaData)
		{
			lock (runningLock)
			{
				if (IsRunning)
				{
					throw new Exception("Validator already running");
				}

				IsRunning = true;

				new Thread(delegate(){
					this.ValidateData(metaData, null);
				}).Start();
			}
		}

        public ValidationResultsPackage ValidateBlocking(ProcessedDataPackage metaData)
		{
			lock (runningLock)
			{
				if (IsRunning)
				{
					throw new Exception("Validator already running");
				}

				IsRunning = true;
                ValidationResultsPackage vr = new ValidationResultsPackage();
				ValidateData(metaData,vr);
				return vr;
			}
		}

		public void AbortValidation()
		{
			IsRunning = false;
		}

		#endregion

		#region Validation Thread

        private void ValidateData(ProcessedDataPackage data, ValidationResultsPackage resultsList)
		{
			lock (runningLock)
			{
				if (OnValidationStarted != null)
					OnValidationStarted(this);

                IValidationResults vr = null;
				
				lock (this.validators)
				{
                    foreach (IDataValidator dv in this.validators)
					{
						vr = dv.Validate(data);
						vr.Validator = dv;
                        vr.GroupName = dv.GroupName;
						if (vr != null && resultsList != null)
							resultsList.AddLast(vr);

						if (this.OnValidatorProgress != null)
							this.OnValidatorProgress(this, dv, vr, vr != null);

					}
				}

				IsRunning = false;

				if (OnValidationEnded != null)
					OnValidationEnded(this);
			}
		}

		#endregion

		#region Loader

		public void LoadFromFile(String configFilename)
		{

			if (String.IsNullOrEmpty(configFilename))
				throw new Exception("Invalid Configuration Data");

			if (configFilename.IndexOf("\\") == -1 && configFilename.IndexOf("/") == -1)
			{
				configFilename = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ValidationRunner)).Location) + "\\" + configFilename;
			}

			XmlDocument xml = new XmlDocument();

			try
			{
				xml.Load(configFilename);
			}
			catch
			{
				throw new Exception("Invalid Configuration Data");
			}

			this.LoadFromXml(xml);

		}
		
		public void LoadFromXmlString(String xmlData)
		{
			if (String.IsNullOrEmpty(xmlData))
				throw new Exception("Invalid Configuration Data");

			XmlDocument xml = new XmlDocument();

			try
			{
				xml.LoadXml(xmlData);
			}
			catch
			{
				throw new Exception("Invalid Configuration Data");
			}

			this.LoadFromXml(xml);
		
		}

		public void LoadFromXml(XmlDocument xml)
		{

			if (xml == null || xml.ChildNodes == null || xml.ChildNodes.Count > 2)
			{
				throw new Exception("Invalid Configuration Data");
			}

			XmlNode config = xml.ChildNodes[1];

			foreach (XmlNode validatorsNodes in config.ChildNodes)
			{
				if (validatorsNodes.Name == "validators")
				{
					foreach (XmlNode validatorNodes in validatorsNodes.ChildNodes)
					{
						if (validatorNodes.Name == "validator")
							LoadAndRegisterValidator(validatorNodes);
					}
				}
			}
		}

		private void LoadAndRegisterValidator(XmlNode validatorNode)
		{
            String name = "";
            String groupname = "";
            String classname = "";
			String assembly = "";
			String description = "";
			String helpurl = "";

			Dictionary<String, String> config = new Dictionary<string, string>();

			foreach (XmlNode xmlNd in validatorNode)
			{
				if (xmlNd.Name.ToLower().Equals("name"))
				{
					name = xmlNd.InnerText;
				}
				else if (xmlNd.Name.ToLower().Equals("description"))
				{
					description = xmlNd.InnerText;
				}
				else if (xmlNd.Name.ToLower().Equals("classname"))
				{
					classname = xmlNd.InnerText;
				}
				else if (xmlNd.Name.ToLower().Equals("assembly"))
				{
					assembly = xmlNd.InnerText;
				}
				else if (xmlNd.Name.ToLower().Equals("helpurl"))
				{
					helpurl = xmlNd.InnerText;
				}
                else if (xmlNd.Name.ToLower().Equals("groupname"))
				{
                    groupname = xmlNd.InnerText;
				}
				else if (xmlNd.Name.ToLower().Equals("data"))
				{
					foreach (XmlNode nd in xmlNd.ChildNodes)
						config.Add(nd.Name, nd.InnerText);
				}

            }

			if (String.IsNullOrEmpty(name) ||
				String.IsNullOrEmpty(classname))

				throw new Exception("Invalid Configuration Data");

			if (String.IsNullOrEmpty(assembly) || assembly.IndexOf("\\") == -1)
			{
				assembly = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ValidationRunner)).Location) + "\\" + assembly;
			}

			Assembly validatorAssembly = Assembly.LoadFrom(assembly);
            try
            {
                IDataValidator validator = (IDataValidator)validatorAssembly.CreateInstance(classname);
            
			validator.Name = name;
			validator.Description = description;
			validator.HelpURL = helpurl;
            validator.GroupName = groupname;

			validator.Init(config);

			lock (this.validators)
			{
                if(this.validators.Contains(validator) == false){
					validators.Add(validator);
				}
            }
            }
            catch
            {
                return;
            }
		}

		#endregion

	}
}
