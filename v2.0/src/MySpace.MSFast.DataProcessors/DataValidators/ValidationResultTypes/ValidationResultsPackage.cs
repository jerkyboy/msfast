using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.DataValidators;
using System.Collections.ObjectModel;
using System.Xml;
using MySpace.MSFast.DataValidators.ValidationResultTypes;

namespace MySpace.MSFast.DataProcessors.DataValidators.ValidationResultTypes
{
    public class ValidationResultsPackage : LinkedList<IValidationResults>
    {
        public XmlDocument Serialize()
        {
            XmlDocument xml = new XmlDocument();
            XmlElement results = xml.CreateElement("validationresults");

            xml.InsertBefore(xml.CreateXmlDeclaration("1.0", "utf-8", null), xml.DocumentElement);

            xml.AppendChild(results);

            XmlElement res = null;
            foreach (IValidationResults result in this)
            {
                res = xml.CreateElement("results");
                
                res.SetAttribute("group", result.GroupName);
                res.SetAttribute("name", result.Validator.Name);
                res.SetAttribute("score", result.Score.ToString());

                if(result is ValidationResults<SourceValidationOccurance>)
                {
                    res.SetAttribute("type", "source");
                    AddSourceOccurances((ValidationResults<SourceValidationOccurance>)result, res, xml);
                }
                else if(result is ValidationResults<DownloadStateOccurance>)
                {
                    res.SetAttribute("type", "download");
                    AddDownloadOccurances((ValidationResults<DownloadStateOccurance>)result, res, xml);
                }

                results.AppendChild(res);
            }

            return xml;
        }

        private void AddDownloadOccurances(ValidationResults<DownloadStateOccurance> validationResults, XmlElement res, XmlDocument xml)
        {
            XmlElement x = null;
            foreach (DownloadStateOccurance psv in validationResults)
            {
                x  = xml.CreateElement("comment");
                x.InnerText = psv.Comment;
                res.AppendChild(x);

                x  = xml.CreateElement("url");
                x.InnerText = psv.URL;
                res.AppendChild(x);

                x  = xml.CreateElement("type");
                x.InnerText = psv.URLType.ToString();
                res.AppendChild(x);

                x  = xml.CreateElement("guid");
                x.InnerText = psv.FileGUID;
                res.AppendChild(x);
            }
        }

        private void AddSourceOccurances(ValidationResults<SourceValidationOccurance> validationResults, XmlElement res, XmlDocument xml)
        {
            
            XmlElement x = null;

            foreach (SourceValidationOccurance psv in validationResults)
            {
                x = xml.CreateElement("comment");
                x.InnerText = psv.Comment;
                res.AppendChild(x);

                x = xml.CreateElement("index");
                x.InnerText = psv.StartIndex.ToString();
                res.AppendChild(x);

                x = xml.CreateElement("length");
                x.InnerText = psv.Length.ToString();
                res.AppendChild(x);

                x = xml.CreateElement("source");
                x.InnerText = psv.PeiceSource;
                res.AppendChild(x);
            }
        }

    }
}