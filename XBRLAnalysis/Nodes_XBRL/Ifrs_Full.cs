using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_XBRL
{
    class Ifrs_Full
    {
        public string Ifrs_Full_Ns { get; set; }
        public string Ifrs_Full_conRef { get; set; }
        public string Ifrs_Full_decimals { get; set; }
        public string Ifrs_Full_unitRef { get; set; }
        public string Ifrs_Full_value { get; set; }

        public Ifrs_Full(XmlNode xn)
        {
            Ifrs_Full_Ns = xn.LocalName;
            for (int i = 0; i < xn.Attributes.Count; i++)
            {
                switch (xn.Attributes[i].Name)
                {
                    case "contextRef":
                        Ifrs_Full_conRef = xn.Attributes[i].Value; ;
                        break;

                    case "decimals":
                        Ifrs_Full_decimals = xn.Attributes[i].Value;
                        break;

                    case "unitRef":
                        Ifrs_Full_unitRef = xn.Attributes[i].Value;
                        break;
                }
                Ifrs_Full_value = xn.InnerText;
            }
        }



    }
}
