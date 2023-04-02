using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_XBRL
{
    class Unit
    {
        public string Unit_Id { get; set; }
        public string Unit_Value { get; set; }


        public Unit()
        {

        }

        public Unit(XmlNode xn)
        {
            Unit_Id = xn.Attributes["id"].Value;

            for (int i = 0; i < xn.ChildNodes.Count; i++)
            {
                XmlNode childeNode2 = xn.ChildNodes[i];
                Unit_Value = childeNode2.InnerText;
            }
        }


    }
}
