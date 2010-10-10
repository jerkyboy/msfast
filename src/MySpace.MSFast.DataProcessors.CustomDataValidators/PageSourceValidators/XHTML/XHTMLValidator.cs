//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors.CustomDataValidators)
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
using System.Xml;
using System.IO;
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataValidators;


namespace MySpace.MSFast.DataProcessors.CustomDataValidators.PageSourceValidators.XHTML
{
    public class XHTMLValidator : DataValidator<ValidationResults<SourceValidationOccurance>>
	{

		int GetIndex(string buffer, int line, int lineposition)
		{
			int position = 0;
			int currentline = 1;
			while (position < buffer.Length)
			{
				if (buffer[position] == (char)10)
				{
					currentline++;
				}
				if (currentline == line)
					break;

				position++;
			}

			return position + lineposition;
		}
		const int DISPLAYBUFFER = 10;


        public override ValidationResults<SourceValidationOccurance> ValidateData(ProcessedDataPackage package)
        {
            ValidationResults<SourceValidationOccurance> results = new ValidationResults<SourceValidationOccurance>();
            results.Score = -1;

            PageSourceData data = package.GetData<PageSourceData>();

            if (data == null || String.IsNullOrEmpty(data.PageSource))
                return results;

            using (StringReader reader = new StringReader(data.PageSource))
			{
				
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ProhibitDtd = false;
				settings.ValidationType = ValidationType.DTD;
				settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(
				
					delegate(object sender, System.Xml.Schema.ValidationEventArgs e)
					 {
						 int position = GetIndex(data.PageSource, e.Exception.LineNumber, e.Exception.LinePosition);
						 int length = data.PageSource.Length - position > DISPLAYBUFFER ? DISPLAYBUFFER : data.PageSource.Length - position;

                         SourceValidationOccurance p = new SourceValidationOccurance(data, position, length);
						 p.Comment = e.Exception.Message;
						 results.Add(p);
					 });
				settings.XmlResolver = new XHTMLXmlResolver();

				using (XmlReader xmlReader = XmlReader.Create(reader, settings))
				{

                    try
                    {
                        while (xmlReader.Read())
                        {
                            
                        }
                    }
                    catch (System.Xml.XmlException ex)
                    {
                        int position = GetIndex(data.PageSource, ex.LineNumber, ex.LinePosition);
                        int length = data.PageSource.Length - position > DISPLAYBUFFER ? DISPLAYBUFFER : data.PageSource.Length - position;

                        SourceValidationOccurance p = new SourceValidationOccurance(data, position, length);
                        p.Comment = ex.Message;
                        results.Add(p);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
				}
			}

			results.Score = (results.Count == 0 ? 100 : 0);

			return results;
		}
	}
}
