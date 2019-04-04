using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
namespace WeatherAppTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> oDicTempData = new Dictionary<string, string>();
            XmlDocument oXMLDocument = null;
            var CurrentURL = @"http://api.openweathermap.org/data/2.5/forecast?id=6619279&appid=67d56d35122a25324119980946fd31d2&mode=xml";
            using (WebClient oClient = new WebClient())
            {
                string xmlContent = oClient.DownloadString(CurrentURL);
                oXMLDocument = new XmlDocument();
                oXMLDocument.LoadXml(xmlContent);
                
                 XmlNodeList lstNodeTemp = oXMLDocument.SelectNodes("//temperature");
                foreach(XmlNode oNode in lstNodeTemp)
                {
                    XmlAttribute oAttrTempValue = oNode.Attributes["value"];
                    double dKelvin = double.Parse(oAttrTempValue.Value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                    double dCelcius = dKelvin - 273.15;
                    if (dCelcius > 25)
                    {
                        XmlNode oParentNode = oNode.ParentNode;
                        if (oParentNode != null)
                        {
                            XmlAttribute oToDateTime = oParentNode.Attributes["to"];
                            if (oToDateTime != null)
                            {
                                string date = oToDateTime.Value;
                                if (date.Length > 10)
                                {
                                    date = date.Substring(0, 10);
                                    if (!oDicTempData.ContainsKey(date))
                                        oDicTempData.Add(date, (dCelcius.ToString()));
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Number of days with temperature > 20 degrees is " + oDicTempData.Count);
        }
    }
}
