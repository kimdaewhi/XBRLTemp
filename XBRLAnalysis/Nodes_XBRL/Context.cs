using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_XBRL
{
    class Context
    {
        public string Context_id { get; set; }
        public string Entity_identifier { get; set; }
        public string Period_startDate { get; set; }
        public string Period_endDate { get; set; }
        public string Period_instance { get; set; }
        public string[] Dimension_id { get; set; }
        public string[] Dimension_value { get; set; }

        public int Xbrldi_cnt { get; set; }

        public Context()
        {

        }

        public Context(XmlNode xn)
        {
            Context_id = xn.Attributes["id"].Value;

            Dimension_id = new string[2];
            Dimension_value = new string[2];

            for (int i = 0; i < xn.ChildNodes.Count; i++)
            {
                XmlNode childeNode2 = xn.ChildNodes[i];

                for (int j = 0; j < childeNode2.ChildNodes.Count; j++)
                {
                    XmlNode childNode3 = childeNode2.ChildNodes[j];
                    switch(childNode3.Name)
                    {
                        case "identifier":
                            Entity_identifier = childNode3.InnerText;
                            break;
                        case "startDate":
                            Period_startDate = childNode3.InnerText;
                            break;
                        case "endDate":
                            Period_endDate = childNode3.InnerText;
                            break;
                        case "instant":
                            Period_instance = childNode3.InnerText;
                            break;
                        case "xbrldi:explicitMember":
                            Dimension_id[j] = childNode3.Attributes["dimension"].Value;
                            Dimension_value[j] = childNode3.InnerText;
                            break;
                    }
                }
            }
        }


        
    }
}
