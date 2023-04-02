using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XBRLAnalysis.Nodes_Label
{
    class LabelLink
    {
        public class LabelStruct
        {
            public class Loc
            {
                public string Loc_xlinkHref { get; set; }
                public string Loc_xlinkLabel { get; set; }
            }

            public class Label
            {
                public string Label_xlinkRole { get; set; }
                public string Label_xlinkLabel { get; set; }
                public string Label_xlinkLang { get; set; }
                public string Label_Id { get; set; }
                public string Label_Value { get; set; }
            }

            public class LabelArc
            {
                public string Arc_xLinkFrom { get; set; }
                public string Arc_xLinkTo { get; set; }

            }

            public Loc loc;
            public Label label;
            public LabelArc labelArc;

            public LabelStruct()
            {
                loc = new Loc();
                label = new Label();
                labelArc = new LabelArc();
            }
        }

        public LabelStruct lb;


        /* 실제 xml 노드의 필드가 아닌 애들은 inner class 변수로 만들지 않음. */

        /// <summary>
        /// napespace_계정코드 구조
        /// </summary>
        public string Loc_HrefVal { get; set; }

        /// <summary>
        /// xbrl문서 상위 노드 namespace
        /// </summary>
        public string Xbrl_Namespace { get; set; }

        /// <summary>
        /// 계정 코드(?)
        /// </summary>
        public string Xbrl_ItemCode { get; set; }

        public LabelLink()
        {
            lb = new LabelStruct();
        }


        public void InitSturct(XmlNode xn, int xnOrder)
        {
            #region link:loc
            if(xn.Name == "link:loc")
            {
                for (int j = 0; j < xn.Attributes.Count; j++)
                {
                    switch (xn.Attributes[j].Name)
                    {
                        case "xlink:href":
                            lb.loc.Loc_xlinkHref = xn.Attributes[j].Value;
                            string hrefVal = xn.Attributes[j].Value.Split('#')[1].ToString();
                            Loc_HrefVal = hrefVal;
                            Xbrl_Namespace = hrefVal.Split('_')[0].ToString();
                            if(hrefVal.Split('_')[0].Contains("entity"))
                            {
                                string s_result = string.Empty;
                                for(int k = 0; k< hrefVal.Split('_').Length; k++)
                                {
                                    if(k > 0)
                                    {
                                        s_result += hrefVal.Split('_')[k] + "_";
                                    }
                                    
                                }
                                Xbrl_ItemCode = s_result.Substring(0, s_result.Length - 1);
                            }
                            else
                            {
                                Xbrl_ItemCode = hrefVal.Split('_')[1].ToString();
                            }
                            // Xbrl_ItemCode = hrefVal.Split('_')[hrefVal.Split('_').Length - 1].ToString();
                            // Xbrl_ItemCode = hrefVal.Split('_')[1].ToString();
                            break;
                        case "xlink:label":
                            lb.loc.Loc_xlinkLabel = xn.Attributes[j].Value;
                            break;
                    }
                }
            }
            #endregion

            #region link:label
            if(xn.Name == "link:label")
            {
                for (int j = 0; j < xn.Attributes.Count; j++)
                {
                    switch (xn.Attributes[j].Name)
                    {
                        case "xlink:role":
                            lb.label.Label_xlinkRole = xn.Attributes[j].Value;
                            break;
                        case "xlink:label":
                            lb.label.Label_xlinkLabel = xn.Attributes[j].Value;
                            break;
                        case "xml:lang":
                            lb.label.Label_xlinkLang = xn.Attributes[j].Value;
                            break;
                        case "id":
                            lb.label.Label_Id = xn.Attributes[j].Value;
                            break;
                    }
                    lb.label.Label_Value = xn.InnerText;
                }
            }
            #endregion

            #region link:labelArc
            if(xn.Name == "link:labelArc")
            {
                for (int j = 0; j < xn.Attributes.Count; j++)
                {
                    switch (xn.Attributes[j].Name)
                    {
                        case "xlink:from":
                            lb.labelArc.Arc_xLinkFrom = xn.Attributes[j].Value;
                            break;
                        case "xlink:to":
                            lb.labelArc.Arc_xLinkTo = xn.Attributes[j].Value;
                            break;
                    }
                }
            }
            #endregion
        }


    }
}
