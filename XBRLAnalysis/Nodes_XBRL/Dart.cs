using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_XBRL
{
    class Dart
    {
        public string Dart_Ns { get; set; }
        public string Dart_conRef { get; set; }
        public string Dart_decimals { get; set; }
        public string Dart_unitRef { get; set; }
        public string Dart_value { get; set; }


        public Dart(XmlNode xn)
        {
            Dart_Ns = xn.LocalName;
            for(int i = 0; i < xn.Attributes.Count; i++)
            {
                switch (xn.Attributes[i].Name)
                {
                    case "contextRef":
                        Dart_conRef = xn.Attributes[i].Value; ;
                        break;

                    case "decimals":
                        Dart_decimals = xn.Attributes[i].Value;
                        break;

                    case "unitRef":
                        Dart_unitRef = xn.Attributes[i].Value;
                        break;
                }
                Dart_value = xn.InnerText;
            }
        }


    }
}
