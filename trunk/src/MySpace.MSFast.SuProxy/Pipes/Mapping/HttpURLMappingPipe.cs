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
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using MySpace.MSFast.SuProxy.Exceptions;

namespace MySpace.MSFast.SuProxy.Pipes.Mapping
{
	public class HttpURLMappingPipe : HttpPipe
	{
		private static Dictionary<String, LinkedList<Regex>> categories = new Dictionary<string, LinkedList<Regex>>();
		private static Dictionary<String, Dictionary<Regex, String>> categoriesMapping = new Dictionary<string,Dictionary<Regex,string>>();

		private static object initLock = new object();

		bool isCheckedForMorePipes = false;
		String categoryId = String.Empty;

		public override void Init(Dictionary<object, object> dictionary) 
		{
			base.Init(dictionary);

			lock (initLock) 
			{
				if (categories.Count == 0)
				{
					LoadCategories(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" 
                        + dictionary["configfolder"].ToString(),
                        dictionary["configfilenamepattern"].ToString());
				}
				this.categoryId = (String)dictionary["category"];
			}
		}

		public override void SendData(byte[] buffer, int offset, int length)
		{
			if (isCheckedForMorePipes == false) 
			{
				isCheckedForMorePipes = true;

				if (categories.ContainsKey(this.categoryId) && categories[this.categoryId].Count > 0)
				{
					LinkedList<Regex> regexs = categories[this.categoryId];
					Dictionary<Regex, String> mapping = categoriesMapping[this.categoryId];

					if (this.PipesChain.ChainState.ContainsKey("REQUEST_URI"))
					{
						String uriStr = (String)this.PipesChain.ChainState["REQUEST_URI"];

						foreach (Regex match in regexs)
						{
							if (match.IsMatch(uriStr))
							{
								AddPipesChain(mapping[match]);
								break;
							}
						}
					}
				}
			}

			base.SendData(buffer, offset, length);
		}

		private void AddPipesChain(string p)
		{
			HttpPipe[] pipes = this.ChainsFactory.GetPipesInChain(p);
			
			if(pipes != null)
			{
				this.PipesChain.AddAfter(this,pipes);
			}
		}

		#region Categories and mapping loader

		private void LoadCategories(String folder,String pattern)
		{
			DirectoryInfo di = new DirectoryInfo(folder);
			foreach (FileInfo fi in di.GetFiles(pattern))
			{
				LoadCategories(fi.FullName);
			}
		}
		private void LoadCategories(string configFilename)
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

			LoadCategories(xml);
		}
		private void LoadCategories(XmlDocument configXml)
		{
			if (configXml.ChildNodes.Count > 2)
			{
				throw new InvalidConfigException();
			}

			XmlNode config = configXml.ChildNodes[1];
			if (config != null)
			{
				foreach (XmlNode nodes in config.ChildNodes)
				{
					if (nodes.Name == "urlmapping")
					{
						foreach (XmlNode node in nodes.ChildNodes)
						{
							if (node.Name == "map")
								RegisterMappingData(node);
						}
					}
				}
			}
		}
		private void RegisterMappingData(XmlNode node)
		{
			lock (categories)
			{
				Dictionary<Regex, String> mappingDic = null;
				LinkedList<Regex> categoriesLst = null;

				String categoryId = node.Attributes["category"].Value;

				if(categories.ContainsKey(categoryId))
				{
					categoriesLst = categories[categoryId];
					mappingDic = categoriesMapping[categoryId];
				}
				else
				{
					categoriesLst = new LinkedList<Regex>();
					mappingDic = new Dictionary<Regex, string>();

					categories.Add(categoryId, categoriesLst);
					categoriesMapping.Add(categoryId, mappingDic);
				}

				Regex match = null;
				String chain = null;

				foreach (XmlNode n in node)
				{
					if (n.Name.ToLower().Equals("match"))
					{
						match = new Regex(n.InnerText,RegexOptions.Compiled);
					}
					else if (n.Name.ToLower().Equals("chain"))
					{
						chain = n.InnerText;
					}
				}
				if (!String.IsNullOrEmpty(chain) && match != null) 
				{
					categoriesLst.AddLast(match);
					mappingDic.Add(match, chain);
				}
			}
		}

		#endregion

	}
}
