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
using System.IO;
using MySpace.MSFast.SuProxy.Exceptions;
using System.Xml;
using MySpace.MSFast.SuProxy.Proxy;

namespace MySpace.MSFast.SuProxy.Pipes
{
	public class HttpPipesChainsFactory
	{
		private IDictionary<String, ChainObject[]> pipesChains = null;
		private HttpPipesRepository httpPipesRepository = null;

		public HttpPipesChainsFactory(SuProxyConfiguration config)
		{
			pipesChains = new Dictionary<String, ChainObject[]>();
			LoadChains(config.ConfigurationFiles);
			httpPipesRepository = new HttpPipesRepository(config);
		}

		public HttpPipe[] GetPipesInChain(string chainid)
		{
			if(this.pipesChains.ContainsKey(chainid))
			{
				ChainObject[] chainObjects = this.pipesChains[chainid];
				LinkedList<HttpPipe> pipes = new LinkedList<HttpPipe>();

				for (int i = 0; i < chainObjects.Length; i++)
				{
					if (chainObjects[i].ObjectType == ChainObjectType.Pipe)
					{
						pipes.AddLast(httpPipesRepository.GetPipeInstance(chainObjects[i].ObjectId,this));
					}
					else if (chainObjects[i].ObjectType == ChainObjectType.Chain)
					{
						HttpPipe[] morePipes = GetPipesInChain(chainObjects[i].ObjectId);
						foreach (HttpPipe np in morePipes) 
						{
							pipes.AddLast(np);
						}
					}
				}

				HttpPipe[] pipesArr = new HttpPipe[pipes.Count];
				pipes.CopyTo(pipesArr, 0);


				return pipesArr;
			}
			return null;
		}

		#region Chains Loader
		private void LoadChains(String[] configFiles)
		{
			if(configFiles == null || configFiles.Length == 0)
				throw new InvalidConfigException();

         foreach(String filename in configFiles)
			{
				LoadChains(filename);
			}
		}
		private void LoadChains(String configFilename)
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

			LoadChains(xml);
		}
		private void LoadChains(XmlDocument configXml)
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
					if (pipesNodes.Name == "chains")
					{
						foreach (XmlNode pipeNode in pipesNodes.ChildNodes)
						{
							if (pipeNode.Name == "chain")
								RegisterChainData(pipeNode);
						}
					}
				}
			}
		}
		private void RegisterChainData(XmlNode configNode)
		{
			lock (this.pipesChains)
			{
				LinkedList<ChainObject> objects = new LinkedList<ChainObject>();
				String chainName = configNode.Attributes["name"].Value;

				foreach (XmlNode xmlNd in configNode)
				{
					if (xmlNd.Name.ToLower().Equals("pipename"))
					{
						objects.AddLast(new ChainObject(ChainObjectType.Pipe,xmlNd.InnerText));
					}
					else if (xmlNd.Name.ToLower().Equals("chainname"))
					{
						objects.AddLast(new ChainObject(ChainObjectType.Chain, xmlNd.InnerText));
					}
				}

				if (String.IsNullOrEmpty(chainName))
					throw new InvalidConfigException();

				ChainObject[] objectsArr = new ChainObject[objects.Count];
				objects.CopyTo(objectsArr, 0);

				if (this.pipesChains.ContainsKey(chainName))
				{
					this.pipesChains.Remove(chainName);
				}

				this.pipesChains.Add(chainName, objectsArr);
			}
		}

		private enum ChainObjectType 
		{
			Pipe,
			Chain
		}

		private class ChainObject 
		{
			public ChainObjectType ObjectType = ChainObjectType.Pipe;
			public String ObjectId = "";
			
			public ChainObject(ChainObjectType type, String id) 
			{
				this.ObjectId = id;
				this.ObjectType = type;
			}
		}
		#endregion
	}
}
