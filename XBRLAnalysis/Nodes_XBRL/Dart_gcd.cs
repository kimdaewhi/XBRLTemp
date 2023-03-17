using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_XBRL
{
    class Dart_gcd
    {
        public string DartGcd_Ns { get; set; }
        public string DartGcd_ConRef { get; set; }
        public string DartGcd_xmlLang { get; set; }
        public string DartGcd_decimals { get; set; }
        public string DartGcd_unitRef { get; set; }
        public string DartGcd_value { get; set; }



        public Dart_gcd(XmlNode xn)
        {
            DartGcd_Ns = xn.LocalName;
            for (int i = 0; i < xn.Attributes.Count; i++)
            { 
                switch (xn.Attributes[i].Name)
                {
                    case "contextRef":
                        DartGcd_ConRef = xn.Attributes[i].Value;
                        break;
                    case "xml:lang":
                        DartGcd_xmlLang = xn.Attributes[i].Value;
                        break;
                    case "decimals":
                        DartGcd_decimals = xn.Attributes[i].Value;
                        break;
                    case "unitRef":
                        DartGcd_unitRef = xn.Attributes[i].Value;
                        break;
                }
                DartGcd_value = xn.InnerText;
            }
        }
    }



}
