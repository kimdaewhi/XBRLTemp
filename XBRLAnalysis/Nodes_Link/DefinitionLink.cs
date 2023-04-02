using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_Link
{
    class DefinitionLink
    {
        public class DefinitionStruct
        {
            public class Loc
            {
                public string Loc_Href { get; set; }
                public string Loc_Label { get; set; }
                public string Loc_Type { get; set; }
            }

            public class DefinitionArc
            {
                public string DA_Type { get; set; }
                public string DA_ArcRole { get; set; }
                public string DA_From { get; set; }
                public string DA_To { get; set; }
                public string DA_Use { get; set; }
                public string DA_Priority { get; set; }
                public string DA_Order { get; set; }
                
            }

            public Loc loc;
            public DefinitionArc da;

            public DefinitionStruct()
            {
                loc = new Loc();
                da = new DefinitionArc();
            }
        }

        public DefinitionStruct ds;

        public DefinitionLink()
        {
            ds = new DefinitionStruct();
        }

        public void InitSturct(XmlNode xn, int xnOrder)
        {
            #region link:loc
            if (xn.Name == "link:loc")
            {
                for (int j = 0; j < xn.Attributes.Count; j++)
                {
                    switch (xn.Attributes[j].Name)
                    {
                        case "xlink:href":
                            ds.loc.Loc_Href = xn.Attributes[j].Value.Split('#')[1].ToString();
                            break;

                        case "xlink:label":
                            ds.loc.Loc_Label = xn.Attributes[j].Value;
                            break;

                        case "xlink:type":
                            ds.loc.Loc_Type = xn.Attributes[j].Value;
                            break;
                    }
                }
            }
            #endregion

            #region link:presentationArc
            if (xn.Name == "link:definitionArc")
            {
                for (int j = 0; j < xn.Attributes.Count; j++)
                {
                    switch (xn.Attributes[j].Name)
                    {
                        case "xlink:type":
                            ds.da.DA_Type = xn.Attributes[j].Value;
                            break;

                        case "xlink:arcrole":
                            ds.da.DA_ArcRole = xn.Attributes[j].Value;
                            break;

                        case "xlink:from":
                            ds.da.DA_From = xn.Attributes[j].Value;
                            break;

                        case "xlink:to":
                            ds.da.DA_To = xn.Attributes[j].Value;
                            break;

                        case "priority":
                            ds.da.DA_Priority = xn.Attributes[j].Value;
                            break;

                        case "order":
                            ds.da.DA_Order = xn.Attributes[j].Value;
                            break;

                        case "use":
                            ds.da.DA_Use = xn.Attributes[j].Value;
                            break;
                    }
                }
            }
            #endregion
        }

    }
}
