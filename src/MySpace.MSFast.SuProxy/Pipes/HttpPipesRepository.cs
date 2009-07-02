//=======================================================================
/* Project: MSFast (MySpace.MSFast.SuProxy)
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
using MySpace.MSFast.SuProxy.Exceptions;
using System.Xml;
using System.IO;
using System.Reflection;
using MySpace.MSFast.SuProxy.Proxy;

namespace MySpace.MSFast.SuProxy.Pipes
{
	public class HttpPipesRepository
	{
		private static readonly object sLock = new object();
		private IDictionary<String, HttpPipeMeta> availablePipes = null;
		private SuProxyConfiguration config;

		public HttpPipesRepository(SuProxyConfiguration config)
		{
			this.config = config;
			availablePipes = new Dictionary<String, HttpPipeMeta>();
			LoadPipes(config.ConfigurationFiles);
		}

		#region Pipes Loader

		private void LoadPipes(String[] configFiles)
		{
			if (configFiles == null || configFiles.Length == 0)
				throw new InvalidConfigException();

			foreach (String filename in configFiles)
			{
				LoadPipes(filename);
			}
		}
		private void LoadPipes(String configFilename)
		{
			if (String.IsNullOrEmpty(configFilename))
				throw new InvalidConfigException();

			XmlDocument xml = new XmlDocument();

			try
			{
				xml.Load(configFilename);
			}
			catch (Exception e)
			{
				throw new InvalidConfigException(e);
			}

			LoadPipes(xml);
		}
		private void LoadPipes(XmlDocument configXml)
		{
			if (configXml.ChildNodes.Count > 2)
			{
				throw new InvalidConfigException();
			}

			XmlNode config = configXml.ChildNodes[1];

			if (config != null)
			{
				foreach (XmlNode pipesNodes in config.ChildNodes)
				{
					if (pipesNodes.Name == "pipes")
					{
						foreach (XmlNode pipeNode in pipesNodes.ChildNodes)
						{
							if (pipeNode.Name == "pipe")
								RegisterPipeData(pipeNode);
						}
					}
				}
			}
		}
		private void RegisterPipeData(XmlNode configNode)
		{
			lock (this.availablePipes)
			{
				HttpPipeMeta pipeInfo = new HttpPipeMeta();
				pipeInfo.Config = new Dictionary<object, object>();

				pipeInfo.PipeName = configNode.Attributes["name"].Value;
				pipeInfo.Classname = configNode.Attributes["classname"].Value;
				
				try
				{
					if(configNode.Attributes["assembly"] != null)
						pipeInfo.Assembly = configNode.Attributes["assembly"].Value;
				}
				catch 
				{
				}

				foreach (XmlNode xmlNd in configNode)
				{
					if (xmlNd.Name.ToLower().Equals("data"))
					{
						foreach (XmlNode nd in xmlNd.ChildNodes)
							pipeInfo.Config.Add(nd.Name, nd.InnerText);
					}
				}

				if (String.IsNullOrEmpty(pipeInfo.PipeName) ||
					String.IsNullOrEmpty(pipeInfo.Classname))
					throw new InvalidConfigException();

				if (this.availablePipes.ContainsKey(pipeInfo.PipeName))
				{
					this.availablePipes.Remove(pipeInfo.PipeName);
				}
				
				this.availablePipes.Add(pipeInfo.PipeName, pipeInfo);
			}
		}
		public HttpPipe GetPipeInstance(String pipeName, HttpPipesChainsFactory httpPipesChainsFactory)
		{

			if (!availablePipes.ContainsKey(pipeName))
				throw new HttpPipeNotFoundException();

			HttpPipeMeta mi = availablePipes[pipeName];
			Assembly pipeAssembly = null;

			if (String.IsNullOrEmpty(mi.Assembly) == false)
			{
				String location = mi.Assembly;

				if (File.Exists(location) == false)
				{
					location = (Path.GetDirectoryName(Assembly.GetAssembly(typeof(HttpPipesRepository)).Location).Replace("\\","/") + "/" + location);
				}
				if (File.Exists(location))
				{
					pipeAssembly = Assembly.LoadFrom(location);
				}
			}

			if (pipeAssembly == null)
				pipeAssembly = Assembly.GetAssembly(typeof(HttpPipesRepository));

			HttpPipe pipe = (HttpPipe)pipeAssembly.CreateInstance(mi.Classname);
			pipe.Configuration = this.config;
			pipe.ChainsFactory = httpPipesChainsFactory;
			pipe.Init(mi.Config);

			return pipe;
		}

		#endregion

		private class HttpPipeMeta
		{
			public String PipeName = null;
			public String Classname = null;
			public String Assembly = null;
			public Dictionary<object, object> Config = null;
		}
	}
}
