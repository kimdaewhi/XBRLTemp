using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_Link
{
    class PresentationArc
    {
        public class PresentationStruct
        {
            public class Loc
            {
                public string Loc_Href { get; set; }
                public string Loc_Label { get; set; }
                public string Loc_Type { get; set; }
            }

            public class PresentationArc
            {
                public string PA_Type { get; set; }
                public string PA_ArcRole { get; set; }
                public string PA_From { get; set; }
                public string PA_To { get; set; }
                public int PA_Priority { get; set; }
                public decimal PA_Order { get; set; }
                public string PA_Use { get; set; }
            }


            public Loc loc;
            public PresentationArc pa;
            
            public PresentationStruct()
            {
                loc = new Loc();
                pa = new PresentationArc();
            }

            
        }

        public PresentationStruct ps;


        public PresentationArc()
        {
            ps = new PresentationStruct();
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
                            ps.loc.Loc_Href = xn.Attributes[j].Value.Split('#')[1].ToString();
                            break;

                        case "xlink:label":
                            ps.loc.Loc_Label = xn.Attributes[j].Value;
                            break;

                        case "xlink:type":
                            ps.loc.Loc_Type = xn.Attributes[j].Value;
                            break;
                    }
                }
            }
            #endregion

            #region link:presentationArc
            if (xn.Name == "link:presentationArc")
            {
                for (int j = 0; j < xn.Attributes.Count; j++)
                {
                    switch (xn.Attributes[j].Name)
                    {
                        case "xlink:type":
                            ps.pa.PA_Type = xn.Attributes[j].Value;
                            break;

                        case "xlink:arcrole":
                            ps.pa.PA_ArcRole = xn.Attributes[j].Value;
                            break;

                        case "xlink:from":
                            ps.pa.PA_From = xn.Attributes[j].Value;
                            break;

                        case "xlink:to":
                            ps.pa.PA_To = xn.Attributes[j].Value;
                            break;

                        case "priority":
                            // ps.pa.PA_Priority = xn.Attributes[j].Value
                            ps.pa.PA_Priority = xn.Attributes[j].Value == null ? 0 : Convert.ToInt32(xn.Attributes[j].Value);
                            break;

                        case "order":
                            // ps.pa.PA_Order = xn.Attributes[j].Value;
                            ps.pa.PA_Order = xn.Attributes[j].Value == null ? 0 : Convert.ToDecimal(xn.Attributes[j].Value);
                            break;

                        case "use":
                            ps.pa.PA_Use = xn.Attributes[j].Value;
                            break;
                    }
                }
            }
            #endregion
        }

        public void ClearStruct()
        {
            this.ps = new PresentationStruct();
        }


    }
}
