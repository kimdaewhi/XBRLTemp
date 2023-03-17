using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_Link
{
    class RoleRef
    {
        public string Role_Uri { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }

        public RoleRef()
        {

        }

        public RoleRef(XmlNode xn)
        {
            for (int i = 0; i < xn.Attributes.Count; i++)
            {
                switch (xn.Attributes[i].Name)
                {
                    case "roleURI":
                        Role_Uri = xn.Attributes[i].Value;
                        break;

                    case "xlink:type":
                        Type = xn.Attributes[i].Value;
                        break;

                    case "xlink:href":
                        Href = xn.Attributes[i].Value;
                        break;
                }
            }
        }


    }
}
