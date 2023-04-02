using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_XBRL
{
    class Entity
    {
        public string Entity_Ns { get; set; }
        public string Entity_conRef { get; set; }
        public string Entity_decimals { get; set; }
        public string Entity_unitRef { get; set; }
        public string Entity_value { get; set; }


        public Entity(XmlNode xn)
        {
            Entity_Ns = xn.LocalName;
            for(int i = 0; i < xn.Attributes.Count; i++)
            {
                switch(xn.Attributes[i].Name)
                {
                    case "contextRef":
                        Entity_conRef = xn.Attributes[i].Value; ;
                        break;

                    case "decimals":
                        Entity_decimals = xn.Attributes[i].Value;
                        break;

                    case "unitRef":
                        Entity_unitRef = xn.Attributes[i].Value;
                        break;
                }
                Entity_value = xn.InnerText;
            }
        }
    }
}
