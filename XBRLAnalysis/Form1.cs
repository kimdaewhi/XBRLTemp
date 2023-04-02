using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System.Configuration;
using XBRLModel;
using System.IO;
using System.Text.RegularExpressions;

namespace XBRLAnalysis
{
    public partial class XBRLAnalysis : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        // 파일 확장자
        private enum FileType
        {
            XBRL = 0,
            LABEL_KO = 1,
            LABEL_EN = 2,
            LINK_CAL = 3,
            LINK_DIM = 4,
            LINK_PRE = 5,
            LINK_SCHEMA = 6
        }

        private string _XbrlMsgCaption = "XBRLAnalysis";

        private List<string> _xmlNodeList;       // root 제와한 최상위 노드 List

        private int _iRow;                       // Excel 출력 시 row line
        private int _iCol;                       // Excel 출력 시 column line

        // xml/xbrl 파일을 DataSet 형태로 변환한 객체
        private System.Data.DataSet _ds_global = new System.Data.DataSet();

        // 파일 확장자 타입
        private FileType _ft;

        private int _ord_Pre = 0;
        private int _ord_Def = 0;


        private XBRLModel.MSSQL _mssqlModel;
        private string _install_FolderDir = string.Empty;

        private string _corpCode;

        private string _sDocDate = string.Empty;

        private List<TreeNode> _CurrentNodeMatches = new List<TreeNode>();




        public XBRLAnalysis()
        {
            InitializeComponent();
            _xmlNodeList = new List<string>();

            _iRow = 4;
            _iCol = 2;
            radio_XBRL.Checked = true;

            _mssqlModel = MSSQL.Instance();
        }



        #region =============================================== Button Event ===============================================

        /// <summary>
        /// 변환한 xbrl / xml 파일 Excel 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ToExcel_Click(object sender, EventArgs e)
        {
            if(txt_xbrlFilePath1.Text == string.Empty)
            {
                MessageBox.Show("파일 경로가 입력되지 않았습니다.", _XbrlMsgCaption);
                return;
            }

            string sFileName = string.Empty;
            string sXmlFilePath = string.Empty;

            // 출력할 Excel 파일 경로
            switch (_ft)
            {
                case FileType.XBRL:
                    sFileName = ConfigurationManager.AppSettings["Path_XbrlTemplatePath"] + ConfigurationManager.AppSettings["File_XbrlInstance"];
                    break;

                case FileType.LABEL_KO:
                    sFileName = ConfigurationManager.AppSettings["Path_XbrlTemplatePath"] + ConfigurationManager.AppSettings["File_XbrlLabel"];
                    break;

                case FileType.LABEL_EN:
                    sFileName = ConfigurationManager.AppSettings["Path_XbrlTemplatePath"] + ConfigurationManager.AppSettings["File_XbrlLabel"];
                    break;

                case FileType.LINK_PRE:
                    sFileName = ConfigurationManager.AppSettings["Path_XbrlTemplatePath"] + ConfigurationManager.AppSettings["File_XbrlLinkPresentation"];
                    break;

                case FileType.LINK_DIM:
                    sFileName = ConfigurationManager.AppSettings["Path_XbrlTemplatePath"] + ConfigurationManager.AppSettings["File_XbrlLinkDimension"];
                    break;
            }

            // 1. Excel파일 열기
            Microsoft.Office.Interop.Excel._Application XL = XLM_Simple.GetActiveExcel();
            try
            {
                XLM_Simple.Open(XL, sFileName, true, Type.Missing);
                XL = XLM_Simple.GetActiveExcel();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Excel 파일 열기에 실패하였습니다." + Environment.NewLine + ex.Message, _XbrlMsgCaption);
                return;
            }

            // 2. xbrl 최상위 노드 집합의 DataSet 생성/데이터 추가
            _ds_global = GetDataSetXbrl(_ft, txt_xbrlFilePath1.Text);
            _ord_Pre = 0;
            _ord_Def = 0;
            // 3. DataSet을 Excel에 출력
            for (int i = 0; i < _ds_global.Tables.Count; i++)
            {
                System.Data.DataTable dt = _ds_global.Tables[i];
                XLM_Simple.SetDataOnCell(XL, dt.TableName, _iRow, _iCol, dt);
            }

            // 4. 다른 이름으로 저장
            string sNewFileName = sFileName.Split('\\')[sFileName.Split('\\').Length - 2] + "_1";
            XLM_Simple.OpenSaveDialog(XL, sNewFileName);

            if (XL != null)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(XL.Worksheets);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(XL.Workbooks);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(XL);

                XL = null;
            }
            System.Windows.Forms.Application.DoEvents();

        }


        /// <summary>
        /// Xbrl Instance + Label 매핑하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InsertFinData_Click(object sender, EventArgs e)
        {
            // 1. 파일 로드 유무 확인
            if (tv_FolderList.Nodes.Count <= 0)
            {
                MessageBox.Show("xbrl 파일이 로드되지 않았습니다." + Environment.NewLine + "폴더 선택 후 재시도하십시오.", _XbrlMsgCaption);
                return;
            }

            PrintConsole("xml 파일 로드중입니다.");
            // 2. Presentation / Label / xbrl Instance 문서 찾기
            string searchTxt = ".xbrl|pre|lab";
            for(int i = 0; i < searchTxt.Split('|').Length; i++)
            {
                SearchNodes(searchTxt.Split('|')[i], tv_FolderList.Nodes[0]);
            }

            string sXbrlFile = string.Empty;                    // xbrl
            string sLinkBasePre = string.Empty;                 // LinkBase - Presentation
            string sLabelKo = string.Empty;                     // Label(Ko)
            string sLabelEn = string.Empty;                     // Label(En)

            // 3. 정규식 사용하여 각각에 해당하는 파일 읽어오기
            for (int i = 0; i < _CurrentNodeMatches.Count; i++)
            {
                if (Regex.IsMatch(_CurrentNodeMatches[i].Text, @".xbrl$") == true)      // xbrl
                {
                    sXbrlFile = _CurrentNodeMatches[i].FullPath;
                }
                else if (Regex.IsMatch(_CurrentNodeMatches[i].Text, @"^pre_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$") == true)     // presentation
                {
                    sLinkBasePre = _CurrentNodeMatches[i].FullPath;
                }
                else if (Regex.IsMatch(_CurrentNodeMatches[i].Text, @"^lab_[0-9]{8}-ko_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$") == true)     // label_ko
                {
                    sLabelKo = _CurrentNodeMatches[i].FullPath;
                }
                else if(Regex.IsMatch(_CurrentNodeMatches[i].Text, @"^lab_[0-9]{8}-en_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$") == true)      // label_en
                {
                    sLabelEn = _CurrentNodeMatches[i].FullPath;
                }
            }
            _CurrentNodeMatches.Clear();                         // 검색 종료 후 CurrentNodeMatches Clear해준다(그냥... List 관리 차원...??)

            // 4. 파일 Full경로 가져오기
            string newstring = _install_FolderDir.Substring(0, _install_FolderDir.Substring(0, _install_FolderDir.Length - 1).LastIndexOf('\\'));
            sXbrlFile = newstring + @"\" + sXbrlFile;
            sLinkBasePre = newstring + @"\" + sLinkBasePre;
            sLabelKo = newstring + @"\" + sLabelKo;
            sLabelEn = newstring + @"\" + sLabelEn;

            PrintConsole("데이터 생성중입니다.");
            // 5. 데이터셋 생성
            DataSet ds_xbrl = GetDataSetXbrl(FileType.XBRL, sXbrlFile);                     // XBRL Instance
            DataSet ds_LinkBasePre = GetDataSetXbrl(FileType.LINK_PRE, sLinkBasePre);       // LinkBase - Presentation
            DataSet ds_label_ko = GetDataSetXbrl(FileType.LABEL_KO, sLabelKo);              // LinkBase - Label(Ko)
            DataSet ds_label_en = GetDataSetXbrl(FileType.LABEL_KO, sLabelEn);              // LinkBase - Label(En)

            DataSet ds_Item = ds_xbrl.Clone();            // 결과 담을 result DataSet
            _ord_Pre = 0;           // 한번 시도 후 재시도하면 오류나니깐 초기화해주기

            PrintConsole("확장계정 Insert/Update 시작");
            #region ============================= 확장계정 Set =============================
            // key값으로 매핑 후 LINQ 이용하여 결과셋에 담기
            for (int i = 0; i < ds_Item.Tables.Count; i++)
            {
                if (ds_xbrl.Tables[i].TableName == "dart" || ds_xbrl.Tables[i].TableName.Contains("entity") || ds_xbrl.Tables[i].TableName == "ifrs-full")
                {
                    #region 재무제표에 해당하는 시트
                    ds_Item.Tables[i].Columns.Add("Item_Category", typeof(string));         // 재무제표 타입(ifrs-full / dart / entity)
                    ds_Item.Tables[i].Columns.Add("ItemName_KO", typeof(string));           // 계정명(국)
                    ds_Item.Tables[i].Columns.Add("ItemName_EN", typeof(string));           // 계정명(영)

                    var qry = from tb_xbrl in ds_xbrl.Tables[i].AsEnumerable()

                              join tb_label_ko in ds_label_ko.Tables[1].AsEnumerable() on tb_xbrl.Field<string>("namespace") equals tb_label_ko.Field<string>("Xbrl_ItemCode") into dataKey
                              from lblKoRslt in dataKey.DefaultIfEmpty()

                              join tb_label_en in ds_label_en.Tables[1].AsEnumerable() on tb_xbrl.Field<string>("namespace") equals tb_label_en.Field<string>("Xbrl_ItemCode") into dataKey2
                              from lblEnRslt in dataKey2.DefaultIfEmpty()

                              select new
                              {
                                  Namespace = tb_xbrl.Field<string>("namespace"),
                                  ContextRef = tb_xbrl.Field<string>("contextRef"),
                                  Decimals = tb_xbrl.Field<string>("decimals"),
                                  UnitRef = tb_xbrl.Field<string>("unitRef"),
                                  Value = tb_xbrl.Field<double?>("value"),
                                  Item_Category = (lblKoRslt == null) ? ((lblEnRslt == null) ? "" : lblEnRslt.Field<string>("Xbrl_Namespace")) : lblKoRslt.Field<string>("Xbrl_Namespace"),
                                  ItemName_KO = (lblKoRslt == null) ? "" : lblKoRslt.Field<string>("label_value"),      // LINQ로 left outer join 구현할 때 꼭 null 처리 해주자...
                                  ItemName_EN = (lblEnRslt == null) ? "" : lblEnRslt.Field<string>("label_value"),
                              };

                    foreach (var row in qry)
                    {
                        ds_Item.Tables[i].Rows.Add(row.Namespace, row.ContextRef, row.Decimals, row.UnitRef, row.Value, row.Item_Category, row.ItemName_KO, row.ItemName_EN);
                    }
                    #endregion
                }
                else
                {
                    #region 재무제표에 해당하지 않는 시트(그대로 결과셋에 추가)
                    foreach (DataRow dr in ds_xbrl.Tables[i].Rows)
                    {
                        ds_Item.Tables[i].ImportRow(dr);
                    }
                    #endregion
                }
            }


            System.Data.DataTable dt_ItemList = new System.Data.DataTable("T_MST_ITEM");
            dt_ItemList.Columns.Add("ITEM_CD", typeof(string));
            dt_ItemList.Columns.Add("ITEM_NM_KO", typeof(string));
            dt_ItemList.Columns.Add("ITEM_NM_EN", typeof(string));
            dt_ItemList.Columns.Add("ITEM_CTG", typeof(string));
            dt_ItemList.Columns.Add("LVL", typeof(int));
            dt_ItemList.Columns.Add("ITEM_CD_FNG", typeof(string));
            dt_ItemList.Columns.Add("ROLE_CD", typeof(string));

            for (int i = 0; i < ds_Item.Tables.Count; i++)
            {
                if (ds_Item.Tables[i].TableName == "dart" || ds_Item.Tables[i].TableName == "ifrs-full" || ds_Item.Tables[i].TableName.Contains("entity"))
                {
                    for (int j = 0; j < ds_Item.Tables[i].Rows.Count; j++)
                    {
                        /* dr[0] : 계정코드, dr[6] : 계정명(국), dr[7] : 계정명(영), dr[5] : 계정종류(ifrs/dart 등), 계정 depth,  */
                        DataRow dr = ds_Item.Tables[i].Rows[j];
                        dt_ItemList.Rows.Add(dr[0], dr[6], dr[7], dr[5], 1);
                    }
                }
            }

            DataView dv = dt_ItemList.DefaultView;
            // 중복 제거
            dt_ItemList = dv.ToTable(true, new string[] { "ITEM_CD", "ITEM_NM_KO", "ITEM_NM_EN", "ITEM_CTG", "LVL", "ITEM_CD_FNG", "ROLE_CD" });

            // 확장 계정만 가져옴
            dt_ItemList = dt_ItemList.Select("ITEM_CTG LIKE '%entity%'").CopyToDataTable();
            dt_ItemList = dt_ItemList.AsEnumerable().GroupBy(row => row.Field<string>("ITEM_CD")).Select(group => group.First()).CopyToDataTable();

            #endregion ====================================================================


            // 6. 결과 담을 DataSet, 테이블 복사해서 담을꺼라서 여기서 컬럼 생성해줌(T_RPT_CORP_FIN)
            System.Data.DataTable dt_result = new System.Data.DataTable();
            #region 결과 DataSet 컬럼 추가
            dt_result.Columns.Add("DOC_CD", typeof(string));
            dt_result.Columns.Add("ROLE_CD", typeof(string));
            dt_result.Columns.Add("ROLE_TYP_CTG", typeof(string));
            dt_result.Columns.Add("YYMM", typeof(string));
            dt_result.Columns.Add("ITEM_CD", typeof(string));
            dt_result.Columns.Add("CONTEXT_REF", typeof(string));
            dt_result.Columns.Add("ITEM_VAL", typeof(double));

            dt_result.Columns.Add("ORD", typeof(decimal));
            dt_result.Columns.Add("PRIORITY", typeof(int));
            dt_result.Columns.Add("USE_PRE", typeof(string));
            #endregion

            // 7. 사업보고서 Main 데이터 세팅하기
            DataSet ds_rpt = new DataSet();
            ds_rpt.Tables.Add("T_RPT_CORP_MAIN");

            // 8. Main 테이블 컬럼 세팅
            #region ##### T_RPT_COPR_MAIN Column Set ####
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("DOC_CD", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("CORP_CD", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("RPT_CD_CHLD", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("REGI_DT", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("STND_DT", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("DOC_TITLE", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("QTR_TYP", typeof(string));
            ds_rpt.Tables["T_RPT_CORP_MAIN"].Columns.Add("UNIT_INFO", typeof(string));          // 단위 추가
            #endregion ###################################


            // 9. DataSet에 데이터 입력 및 DB 저장
            try
            {
                PrintConsole("공시 보고서 Main 데이터셋 생성중");
                #region ************** T_RPT_CORP_MAIN Data Insert **************
                DateTime now = DateTime.Now;
                _corpCode = ds_xbrl.Tables["context"].Rows[0]["entity_identifier"].ToString();
                string sCorpCode = _corpCode;                                                                                               // 기업고유코드
                string sRptTitle = ds_xbrl.Tables["dart-gcd"].Select("namespace='DocumentTitle' AND xml_lang='ko'")[0]["value"].ToString(); // 보고서명
                string sStndDt = ds_xbrl.Tables["context"].Select("period_endDate = MAX(period_endDate)")[0][3].ToString();                 // 기준일자
                sStndDt = sStndDt.Substring(0, 7).Replace("-", "");

                string sRPtCd = GetRptCd(sRptTitle);                                                                                        // 보고서 상세유형 코드
                _sDocDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                string sDocId = now.ToString("yyyyMMddHHmmss") + _corpCode + "_" + sRPtCd;                                                  // 보고서 고유코드(요거 나중에....작성일자로 다시 세팅하자)
                string sQtyTyp = GetQtrType(sRptTitle);                                                                                     // 분기구분
                
                string unitInfo = ds_xbrl.Tables["dart-gcd"].Select("namespace='UnitInfo' AND xml_lang='ko'")[0]["value"].ToString(); // 단위

                ds_rpt.Tables["T_RPT_CORP_MAIN"].Rows.Add(sDocId, sCorpCode, sRPtCd, _sDocDate, sStndDt, sRptTitle, sQtyTyp, unitInfo);
                #endregion ******************************************************
                PrintConsole("공시 보고서 Main 데이터셋 생성 완료");


                PrintConsole("공시 보고서 Financial 데이터셋 생성중");
                #region ********************* T_RPT_CORP_FIN Data Insert *********************
                ds_LinkBasePre.Tables[1].Columns.Add("Item_Category");          // 재무제표 타입(ifrs-full / dart / entity)
                ds_LinkBasePre.Tables[1].Columns.Add("Namespace");              // 계정코드
                ds_LinkBasePre.Tables[1].Columns.Add("CSType");                 // 연결/개별 여부(C : 연결, S : 개별)
                for (int i = 0; i < ds_LinkBasePre.Tables[1].Rows.Count; i++)
                {
                    #region Sheet명, Namespace, 연결/개별 여부 추가
                    if (ds_LinkBasePre.Tables[1].Rows[i]["preArc_xlink_to"].ToString() != string.Empty && ds_LinkBasePre.Tables[1].Rows[i]["loc_xlink_label"].ToString() != string.Empty)
                    {
                        try
                        {
                            /* xlink_label에서 sheet명, 아이템 코드 조회 
                             * ex) Loc_dart_ShortTermPrepaidConstructionCosts 
                             *   >> Loc_ 떼고(sOrgStr)
                             *   >> dart_ShortTermPrepaidConstructionCosts에서 구분자('_') 기준으로 앞에껀 시트명(sSheet), 뒤에껀 아이템코드(sNamespace)
                             * 
                             * CSType은 presentationLink_xlink:role에서 재무제표 코드 가져오기(뒤 7자리)
                             * ex) http://dart.fss.or.kr/role/ifrs/dart_2019-10-01_role-D210000
                             *   >> 뒤 7자리(D210000) 조회
                             *   >> 마지막 '0'이면 연결(C), '5'면 개별(S)
                             */

                            string sOrgStr = ds_LinkBasePre.Tables[1].Rows[i]["loc_xlink_label"].ToString().Substring(ds_LinkBasePre.Tables[1].Rows[i]["loc_xlink_label"].ToString().Length - (ds_LinkBasePre.Tables[1].Rows[i]["loc_xlink_label"].ToString().Length - 4));
                            string sSheet = ds_LinkBasePre.Tables[1].Rows[i]["preArc_xlink_to"].ToString().Substring(ds_LinkBasePre.Tables[1].Rows[i]["preArc_xlink_to"].ToString().Length - (ds_LinkBasePre.Tables[1].Rows[i]["preArc_xlink_to"].ToString().Length - 4)).Split(new[] { '_' }, 2)[0];
                            string sNamespace = sOrgStr.Split(new[] { '_' }, 2)[1];

                            string sCSType = ds_LinkBasePre.Tables[1].Rows[i]["presentationLink_xlink_role"].ToString() == string.Empty ? 
                                "" : 
                                ds_LinkBasePre.Tables[1].Rows[i]["presentationLink_xlink_role"].ToString().Substring(ds_LinkBasePre.Tables[1].Rows[i]["presentationLink_xlink_role"].ToString().Length - 7);
                            sCSType = sCSType[sCSType.Length - 1].ToString() == "0" ? "C" : "S";

                            ds_LinkBasePre.Tables[1].Rows[i]["Item_Category"] = sSheet;
                            ds_LinkBasePre.Tables[1].Rows[i]["Namespace"] = sNamespace;
                            ds_LinkBasePre.Tables[1].Rows[i]["CSType"] = sCSType;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("i : " + i.ToString() + Environment.NewLine + ex.Message);
                            return;
                        }

                    }
                    #endregion
                }


                for (int i = 0; i < ds_xbrl.Tables.Count; i++)
                {
                    if (ds_xbrl.Tables[i].TableName == "dart" || ds_xbrl.Tables[i].TableName.Contains("entity") || ds_xbrl.Tables[i].TableName == "ifrs-full")
                    {
                        #region 재무데이터에 해당하는 애들
                        // xbrl 데이터셋에 연결/개별 여부 컬럼 추가
                        ds_xbrl.Tables[i].Columns.Add("CSType", typeof(string));
                        for (int j = 0; j < ds_xbrl.Tables[i].Rows.Count; j++)
                        {
                            ds_xbrl.Tables[i].Rows[j]["CSType"] = ds_xbrl.Tables[i].Rows[j]["contextRef"].ToString().Split('_')[4] == "ConsolidatedMember" ? "C" : "S";
                        }

                        string xbrlTableName = ds_xbrl.Tables[i].TableName;

                        if (xbrlTableName == "entity")      // sheet명이 entity면 기업고유코드 붙여줌
                            xbrlTableName = xbrlTableName + sCorpCode;

                        // 조건 : xbrl 시트명 == pre Item_Category && xbrl 계정코드 == pre 계정코드
                        // 와;;;
                        var qry = from tb_xbrl in ds_xbrl.Tables[i].AsEnumerable()
                                  join tb_pre in ds_LinkBasePre.Tables[1].Select("Item_Category IS NOT NULL").AsEnumerable()
                                  on new { a = tb_xbrl.Field<string>("namespace")
                                         , b = xbrlTableName
                                         // , c = tb_xbrl.Field<string>("contextRef").Split('_')[4] == "ConsolidatedMember" ? "C" : "S" }
                                         , c = tb_xbrl.Field<string>("CSType")
                                  }
                                  equals new { a = tb_pre.Field<string>("Namespace")
                                             , b = tb_pre.Field<string>("Item_Category")
                                             // , c = tb_pre.Field<string>("presentationLink_xlink_role").ToString().Substring(tb_pre.Field<string>("presentationLink_xlink_role").ToString().Length - 7)[6].ToString()
                                             , c = tb_pre.Field<string>("CSType")
                                  } into dataKey
                                  from tbPresentation in dataKey.DefaultIfEmpty()
                                  select new
                                  {
                                      DOC_CD = sDocId,
                                      //ROLE_CD = tbPresentation == null ?
                                      //      "" : tb_xbrl.Field<string>("contextRef").Split('_')[4] == "ConsolidatedMember" ?
                                      //                  tbPresentation.Field<string>("presentationLink_xlink_role").Substring(tbPresentation.Field<string>("presentationLink_xlink_role").Length - 7).Remove((tbPresentation.Field<string>("presentationLink_xlink_role").Substring(tbPresentation.Field<string>("presentationLink_xlink_role").Length - 7)).Length - 1, 1) + "0"
                                      //                : tbPresentation.Field<string>("presentationLink_xlink_role").Substring(tbPresentation.Field<string>("presentationLink_xlink_role").Length - 7).Remove((tbPresentation.Field<string>("presentationLink_xlink_role").Substring(tbPresentation.Field<string>("presentationLink_xlink_role").Length - 7)).Length - 1, 1) + "5",
                                      ROLE_CD = tbPresentation == null ? 
                                            "" : tbPresentation.Field<string>("presentationLink_xlink_role").ToString().Substring(tbPresentation.Field<string>("presentationLink_xlink_role").ToString().Length - 7).ToString(), 
                                      ROLE_TYP_CTG = "",
                                      YYMM = tb_xbrl.Field<string>("contextRef").Substring(0, 3) == "BPF" ? tb_xbrl.Field<string>("contextRef").Substring(4, 4) : tb_xbrl.Field<string>("contextRef").Substring(3, 4),
                                      ITEM_CD = tb_xbrl.Field<string>("namespace"),
                                      CONTEXT_REF = tb_xbrl.Field<string>("contextRef"),
                                      ITEM_VAL = tb_xbrl.Field<double>("value"),
                                      ORD = tbPresentation == null ? 0 : tbPresentation.Field<decimal>("preArc_order"),
                                      PRIORITY = tbPresentation == null ? 0 : tbPresentation.Field<int>("preArc_priority"),
                                      USE_PRE = tbPresentation == null ? "" : tbPresentation.Field<string>("preArc_use")
                                  };

                        foreach (var row in qry)
                        {
                            // dt_result.Rows.Add(row.DOC_CD, row.ROLE_CD, row.ROLE_TYP_CTG, row.YYMM, row.ITEM_CD, row.CONTEXT_REF, row.ITEM_VAL);
                            dt_result.Rows.Add(row.DOC_CD, row.ROLE_CD, row.ROLE_TYP_CTG, row.YYMM, row.ITEM_CD, row.CONTEXT_REF, row.ITEM_VAL, row.ORD, row.PRIORITY, row.USE_PRE);
                        }

                        #endregion
                    }

                }


                ds_rpt.Tables.Add(dt_result.Copy());
                ds_rpt.Tables[1].TableName = "T_RPT_CORP_FIN";

                // LinkRole 테이블
                // DB에서 불러오기
                System.Data.DataTable dt_LinkRole = _mssqlModel.execProcedure("P_MST_FIN_LINKROLE_GET", new DataModule.Param[] { }).Tables[0];
                System.Data.DataTable dt_rptCorpFin = ds_rpt.Tables["T_RPT_CORP_FIN"].Copy();

                DataView dvCoprFin = dt_rptCorpFin.DefaultView;
                // 중복 제거
                dt_rptCorpFin = dvCoprFin.ToTable(true, new string[] { "DOC_CD", "ROLE_CD", "ROLE_TYP_CTG", "YYMM", "ITEM_CD", "CONTEXT_REF", "ITEM_VAL", "ORD", "PRIORITY", "USE_PRE" });


                var qry2 = from tb_rptCorpFin in dt_rptCorpFin.AsEnumerable()
                           join tb_LinkRole in dt_LinkRole.AsEnumerable() on new { a = tb_rptCorpFin.Field<string>("ROLE_CD") } equals new { a = tb_LinkRole.Field<string>("ROLE_CD") } into dataKey
                           from RptCorpFin in dataKey.DefaultIfEmpty()
                           select new
                           {
                               DOC_CD = sDocId,
                               ROLE_CD = tb_rptCorpFin.Field<string>("ROLE_CD"),
                               ROLE_TYP_CTG = (RptCorpFin == null) ? "" : RptCorpFin.Field<string>("ROLE_TYP_CTG"),
                               YYMM = tb_rptCorpFin.Field<string>("YYMM"),
                               ITEM_CD = tb_rptCorpFin.Field<string>("ITEM_CD"),
                               CONTEXT_REF = tb_rptCorpFin.Field<string>("CONTEXT_REF"),
                               ITEM_VAL = tb_rptCorpFin.Field<double>("ITEM_VAL"),
                               ORD = tb_rptCorpFin.Field<decimal>("ORD"),
                               PRIORITY = tb_rptCorpFin.Field<int>("PRIORITY"),
                               USE_PRE = tb_rptCorpFin.Field<string>("USE_PRE")
                           };

                ds_rpt.Tables["T_RPT_CORP_FIN"].Clear();
                foreach (var row in qry2)
                {
                    ds_rpt.Tables["T_RPT_CORP_FIN"].Rows.Add(row.DOC_CD, row.ROLE_CD, row.ROLE_TYP_CTG, row.YYMM, row.ITEM_CD, row.CONTEXT_REF, row.ITEM_VAL, row.ORD, row.PRIORITY, row.USE_PRE);
                }
                #endregion *******************************************************************
                PrintConsole("공시 보고서 Financial 데이터셋 생성 완료");

                #region 확장계정에 계정과목 종류 추가(dart / ifrs-full / entity)
                var qry3 = from tb_ItemList in dt_ItemList.AsEnumerable()
                           join tb_Pre in ds_LinkBasePre.Tables[1].Select("Item_Category IS NOT NULL").AsEnumerable()
                           on new { a = tb_ItemList.Field<string>("ITEM_CD") } equals new { a = tb_Pre.Field<string>("Namespace") } into dataKey
                           from ItemList in dataKey.DefaultIfEmpty()
                           select new
                           {
                               ITEM_CD = tb_ItemList.Field<string>("ITEM_CD"),
                               ITEM_NM_KO = tb_ItemList.Field<string>("ITEM_NM_KO"),
                               ITEM_NM_EN = tb_ItemList.Field<string>("ITEM_NM_EN"),
                               ITEM_CTG = tb_ItemList.Field<string>("ITEM_CTG"),
                               LVL = tb_ItemList.Field<int>("LVL"),
                               ITEM_CD_FNG = "",
                               ROLE_CD = ItemList == null ? "" : ItemList.Field<string>("presentationLink_xlink_role").Substring(ItemList.Field<string>("presentationLink_xlink_role").Length - 7)
                           };

                int roleCnt = 0;
                foreach (var row in qry3)
                {
                    dt_ItemList.Rows[roleCnt]["ROLE_CD"] = row.ROLE_CD;
                    roleCnt++;
                }
                roleCnt = 0;
                #endregion

                /* DB 저장 */
                string itemXML = GetStringXML(dt_ItemList);
                string rptMainXml = GetStringXML(ds_rpt.Tables["T_RPT_CORP_MAIN"]);
                string rptFinXml = GetStringXML(ds_rpt.Tables["T_RPT_CORP_FIN"]);


                System.Data.DataTable rslt = _mssqlModel.execProcedure("P_XBRL_MST_ITEM_SET", new DataModule.Param[] { new DataModule.Param("@PXML_MST_ITEM", SqlDbType.Xml, itemXML) }).Tables[0];
                PrintConsole("확장계정 Insert/Update 종료");

                System.Data.DataTable dt_rptMain = _mssqlModel.execProcedure("P_RPT_CORP_MAIN_SET"
                                                                            , new DataModule.Param[] {  new DataModule.Param("@DOC_CD", SqlDbType.VarChar, sDocId)
                                                                                                      , new DataModule.Param("@PXML_RPT_CORP_MAIN", SqlDbType.Xml, rptMainXml)
                                                                                                     }).Tables[0];
                System.Data.DataTable dt_rptFin = _mssqlModel.execProcedure("P_RPT_CORP_FIN_SET"
                                                                            , new DataModule.Param[] {  new DataModule.Param("@DOC_CD", SqlDbType.VarChar, sDocId)
                                                                                                      , new DataModule.Param("@PXML_RPT_CORP_FIN", SqlDbType.Xml, rptFinXml)
                                                                                                     }).Tables[0];
                PrintConsole("공시 보고서 데이터 저장 완료");
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 저장 실패" + Environment.NewLine + ex.Message, _XbrlMsgCaption);
                return;
            }
        }


        /// <summary>
        /// Console 출력 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ConsoleClear_Click(object sender, EventArgs e)
        {
            txtBox_Console.Clear();
        }


        /// <summary>
        /// 폴더 트리뷰 경로 선택 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenFileDialog3_Click(object sender, EventArgs e)
        {
            if (txtBox_folderPath.Text == string.Empty)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _install_FolderDir = fbd.SelectedPath + @"\";
                    txtBox_folderPath.Text = _install_FolderDir;
                }
            }
            else
            {
                _install_FolderDir = txtBox_folderPath.Text;
                InitTreeViewFolder();
                LoadDirectory(_install_FolderDir);

                _corpCode = tv_FolderList.Nodes[0].Nodes[2].Text.Substring(tv_FolderList.Nodes[0].Nodes[2].Text.Length - 8);
                lbl_corpCode.Text = "기업코드 : " + _corpCode;
            }

        }

        #endregion =========================================================================================================



        #region =============================================== RadioButton Event ===============================================
        private void radio_XBRL_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_XBRL.Checked == true)
            {
                _ft = FileType.XBRL;
            }
        }

        private void radio_LabelKo_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_LabelKo.Checked == true)
            {
                _ft = FileType.LABEL_KO;
            }
        }

        private void radio_LabelEn_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_LabelEn.Checked == true)
            {
                _ft = FileType.LABEL_EN;
            }
        }

        private void radio_LinkPre_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_LinkPre.Checked == true)
            {
                _ft = FileType.LINK_PRE;
            }
        }

        private void radio_LinkDim_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_LinkDim.Checked == true)
            {
                _ft = FileType.LINK_DIM;
            }
        }

        private void radio_LinkCal_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_LinkCal.Checked == true)
            {
                _ft = FileType.LINK_CAL;
            }
        }

        #endregion ==============================================================================================================

        /// <summary>
        /// xml 트리뷰 확장/축소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ExpandTree_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_ExpandTree.Checked == true)
            {
                tv_xbrlList.ExpandAll();
            }
            else
            {
                tv_xbrlList.CollapseAll();
            }
        }

        /// <summary>
        /// 파일 트리뷰 확장/축소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_ExpandTreeFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ExpandTreeFolder.Checked == true)
            {
                tv_FolderList.ExpandAll();
            }
            else
            {
                tv_FolderList.CollapseAll();
            }
        }


        /// <summary>
        /// 폴더 트리뷰 노드 더블클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_FolderList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Text != string.Empty && e.Node.Text.Contains("."))
                {
                    // 선택한 파일 전체경로 읽어오기
                    string str = string.Empty;
                    for (int i = 1; i < e.Node.FullPath.Split('\\').Length; i++)
                    {
                        str += e.Node.FullPath.Split('\\')[i] + @"\";
                    }
                    txt_xbrlFilePath1.Text = (_install_FolderDir + @"\" + str).Substring(0, (_install_FolderDir + @"\" + str).Length - 1);
                    txt_xbrlFilePath2.Text = e.Node.Text;

                    // 변환 자동 실행
                    if (txt_xbrlFilePath1.Text == string.Empty)
                    {
                        MessageBox.Show("파일 경로가 입력되지 않았습니다.", _XbrlMsgCaption);
                        return;
                    }
                    string sXmlFilePath1 = txt_xbrlFilePath1.Text;

                    SetXmlNodeTree(sXmlFilePath1);
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("파일을 찾을 수 없습니다.", _XbrlMsgCaption);
            }
        }


        



        /// <summary>
        /// 읽어온 xml을 TreeView로 변환, 재귀함수 구조
        /// </summary>
        /// <param name="childNode">Xml의 하위노드</param>
        /// <param name="tn">추가할 TreeNode</param>
        /// <param name="i">Loop 순서</param>
        private void GetXbrlTreeNode(XmlNode childNode, TreeNode tn, int i)
        {
            if (childNode.Value != null)
                tn.Nodes.Add(childNode.Value);
            else
                tn.Nodes.Add(GetNodeNameTag(childNode.Name));


            if (childNode.HasChildNodes == true)
            {
                for (int j = 0; j < childNode.ChildNodes.Count; j++)
                {
                    XmlNode childNode2 = childNode.ChildNodes[j];
                    TreeNode tn2 = tn.Nodes[i];
                    GetXbrlTreeNode(childNode2, tn2, j);
                }

            }

        }


        /// <summary>
        /// Node명 Tag형식으로 변경
        /// ex. Tag >> <Tag>
        /// </summary>
        /// <param name="name">Tag명</param>
        /// <returns>Tag 반환값</returns>
        private string GetNodeNameTag(string name)
        {
            return "<" + name + ">";
        }

        /// <summary>
        /// Console 출력
        /// </summary>
        /// <param name="str">출력할 문자열</param>
        private void PrintConsole(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.AppendLine(" " + DateTime.Now.ToString());
            sb.AppendLine("--------------------------------------------------------------------------------------------------------------------------------");
            txtBox_Console.Text = txtBox_Console.Text + sb.ToString();
            txtBox_Console.SelectionStart = txtBox_Console.Text.Length;
            txtBox_Console.ScrollToCaret();
        }


        private XmlDocument XmlDocumentLoad(string sFilePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(sFilePath);
            }
            catch (Exception ex) { }

            return xmlDoc;
        }


        /// <summary>
        /// 변환 파일 기반의 DataSet 생성(Header, Data)
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        private DataSet GetDataSetXbrl(FileType fe, string sFilePath)
        {
            string fp = string.Empty;
            _ds_global.Tables.Clear();

            // DataSet 테이블, Header 추가
            DataSet ds = MakeDataSet(fe);

            fp = sFilePath;
            XmlDocument xmlDoc = XmlDocumentLoad(fp);
            if (xmlDoc.DocumentElement == null)
                return ds;
            XmlElement root = xmlDoc.DocumentElement;
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                // 생성된 DataSet에 데이터 추가
                InsertRow(fe, root, ds, i);
            }
            return ds;
        }

        /// <summary>
        /// XML 파일 이용한 TreeView Set
        /// </summary>
        /// <param name="sXmlFilePath"></param>
        private void SetXmlNodeTree(string sXmlFilePath)
        {
            tv_xbrlList.Nodes.Clear();
            _xmlNodeList.Clear();
            checkBox_ExpandTree.CheckState = CheckState.Unchecked;

            // 1. xml 파일 로드
            // XmlDocument xmlDoc = new XmlDocument();
            XmlDocument xmlDoc = XmlDocumentLoad(sXmlFilePath);
            if (xmlDoc.DocumentElement == null)
            {
                MessageBox.Show("xml 로드에 실패하였습니다." + Environment.NewLine + "xml 파일 경로를 확인하세요.", _XbrlMsgCaption);
                return;
            }

            // 2. xml의 Root 얻어오기 / TreeView 최상단 Item에 추가
            XmlElement root = xmlDoc.DocumentElement;
            tv_xbrlList.Nodes.Add(GetNodeNameTag(root.Name));   // root Name

            // 3. xml 각 노드를 하위 TreeView/List에 추가
            if (root.HasChildNodes == true)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    XmlNode childNode = root.ChildNodes[i];
                    TreeNode tn = tv_xbrlList.Nodes[0];

                    _xmlNodeList.Add(childNode.Name);        // List 추가
                    GetXbrlTreeNode(childNode, tn, i);      // TreeNode 추가

                }

            }

            // 4.중복 제거한 xml Element List 출력
            _xmlNodeList = _xmlNodeList.Distinct().ToList();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _xmlNodeList.Count; i++)
            {
                sb.AppendLine("Node List[" + i + "] : " + _xmlNodeList[i].ToString());
            }
            PrintConsole(sb.ToString());
            sb.Clear();
        }


        /// <summary>
        /// 파일 구조에 따른 DataSet 생성
        /// </summary>
        /// <param name="fe">파일 타입</param>
        /// <returns>데이터 추가된 DataSet</returns>
        private DataSet MakeDataSet(FileType fe)
        {
            DataSet ds = new DataSet();

            switch (fe)
            {
                case FileType.XBRL:
                    #region ============================== XBRL DataSet ==============================
                    ds.Tables.Add("link");
                    ds.Tables.Add("context");
                    ds.Tables.Add("unit");
                    ds.Tables.Add("dart-gcd");
                    ds.Tables.Add("dart");
                    ds.Tables.Add("entity");
                    ds.Tables.Add("ifrs-full");

                    // Table["link"]
                    ds.Tables["link"].Columns.Add("link#xlink:type", typeof(string));
                    ds.Tables["link"].Columns.Add("link#xlink:href", typeof(string));

                    // Table["context"]
                    ds.Tables["context"].Columns.Add("id", typeof(string));
                    ds.Tables["context"].Columns.Add("entity_identifier", typeof(string));
                    ds.Tables["context"].Columns.Add("period_startDate", typeof(string));
                    ds.Tables["context"].Columns.Add("period_endDate", typeof(string));
                    ds.Tables["context"].Columns.Add("period_instant", typeof(string));
                    ds.Tables["context"].Columns.Add("dimension_id1", typeof(string));
                    ds.Tables["context"].Columns.Add("dimension_value1", typeof(string));
                    ds.Tables["context"].Columns.Add("dimension_id2", typeof(string));
                    ds.Tables["context"].Columns.Add("dimension_value2", typeof(string));

                    // Table["unit"]
                    ds.Tables["unit"].Columns.Add("unit_id", typeof(string));
                    ds.Tables["unit"].Columns.Add("unit_value", typeof(string));

                    // Table["dart-gcd"]
                    ds.Tables["dart-gcd"].Columns.Add("namespace", typeof(string));
                    ds.Tables["dart-gcd"].Columns.Add("contextRef", typeof(string));
                    ds.Tables["dart-gcd"].Columns.Add("xml_lang", typeof(string));
                    ds.Tables["dart-gcd"].Columns.Add("decimals", typeof(string));
                    ds.Tables["dart-gcd"].Columns.Add("unitRef", typeof(string));
                    ds.Tables["dart-gcd"].Columns.Add("value", typeof(string));

                    // Table["dart"]
                    ds.Tables["dart"].Columns.Add("namespace", typeof(string));
                    ds.Tables["dart"].Columns.Add("contextRef", typeof(string));
                    ds.Tables["dart"].Columns.Add("decimals", typeof(string));
                    ds.Tables["dart"].Columns.Add("unitRef", typeof(string));
                    ds.Tables["dart"].Columns.Add("value", typeof(double));

                    // Table["entity"]
                    ds.Tables["entity"].Columns.Add("namespace", typeof(string));
                    ds.Tables["entity"].Columns.Add("contextRef", typeof(string));
                    ds.Tables["entity"].Columns.Add("decimals", typeof(string));
                    ds.Tables["entity"].Columns.Add("unitRef", typeof(string));
                    ds.Tables["entity"].Columns.Add("value", typeof(double));

                    // Table["ifrs-full"]
                    ds.Tables["ifrs-full"].Columns.Add("namespace", typeof(string));
                    ds.Tables["ifrs-full"].Columns.Add("contextRef", typeof(string));
                    ds.Tables["ifrs-full"].Columns.Add("decimals", typeof(string));
                    ds.Tables["ifrs-full"].Columns.Add("unitRef", typeof(string));
                    ds.Tables["ifrs-full"].Columns.Add("value", typeof(double));
                    
                    break;
                    #endregion =======================================================================

                case FileType.LABEL_KO:
                    #region ============================== Label DataSet ==============================
                    ds.Tables.Add("link_roleRef");
                    ds.Tables.Add("link_labelLink");

                    // Table["link_roleRef"]
                    ds.Tables["link_roleRef"].Columns.Add("roleURI", typeof(string));
                    ds.Tables["link_roleRef"].Columns.Add("xlink:type", typeof(string));
                    ds.Tables["link_roleRef"].Columns.Add("xlink:href", typeof(string));

                    // Table["link_labelLink"]
                    ds.Tables["link_labelLink"].Columns.Add("loc_xlink:href", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("loc_xlink:label", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("loc_xlink:hrefVal", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("label_xlink:role", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("label_xlink:label", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("label_xlink:lang", typeof(string));

                    ds.Tables["link_labelLink"].Columns.Add("label_id", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("label_value", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("labelArc_xlink:from", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("labelArc_xlink:to", typeof(string));

                    ds.Tables["link_labelLink"].Columns.Add("Loc_HrefVal", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("Xbrl_Namespace", typeof(string));
                    ds.Tables["link_labelLink"].Columns.Add("Xbrl_ItemCode", typeof(string));

                    break;
                #endregion =======================================================================

                case FileType.LABEL_EN:
                    break;

                case FileType.LINK_PRE:
                    #region ============================== Link_Pre DataSet ==============================
                    ds.Tables.Add("link_roleRef");
                    ds.Tables.Add("link_presentationLink");

                    // Table["link_roleRef"]
                    ds.Tables["link_roleRef"].Columns.Add("roleURI", typeof(string));
                    ds.Tables["link_roleRef"].Columns.Add("xlink:type", typeof(string));
                    ds.Tables["link_roleRef"].Columns.Add("xlink:href", typeof(string));

                    // Table["link_labelLink"]
                    ds.Tables["link_presentationLink"].Columns.Add("presentationLink_xlink_role", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("loc_xlink_href", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("loc_xlink_label", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("loc_xlink_type", typeof(string));

                    ds.Tables["link_presentationLink"].Columns.Add("preArc_xlink_type", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("preArc_xlink_arcrole", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("preArc_xlink_from", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("preArc_xlink_to", typeof(string));
                    ds.Tables["link_presentationLink"].Columns.Add("preArc_priority", typeof(int));
                    ds.Tables["link_presentationLink"].Columns.Add("preArc_order", typeof(decimal));
                    ds.Tables["link_presentationLink"].Columns.Add("preArc_use", typeof(string));

                    break;
                #endregion =======================================================================

                case FileType.LINK_DIM:
                    #region ============================== Link_Pre DataSet ==============================
                    ds.Tables.Add("link_arcroleRef");
                    ds.Tables.Add("link_roleRef");
                    ds.Tables.Add("link_definitionLink");

                    // Table["link_arcroleRef"]
                    ds.Tables["link_arcroleRef"].Columns.Add("arcroleURI", typeof(string));
                    ds.Tables["link_arcroleRef"].Columns.Add("xlink:type", typeof(string));
                    ds.Tables["link_arcroleRef"].Columns.Add("xlink:href", typeof(string));

                    // Table["link_roleRef"]
                    ds.Tables["link_roleRef"].Columns.Add("roleURI", typeof(string));
                    ds.Tables["link_roleRef"].Columns.Add("xlink:type", typeof(string));
                    ds.Tables["link_roleRef"].Columns.Add("xlink:href", typeof(string));

                    // Table["link_labelLink"]
                    ds.Tables["link_definitionLink"].Columns.Add("definitionLink_xlink:role", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("loc_xlink:href", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("loc_xlink:label", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("loc_xlink:type", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_xlink:type", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_xlink:arcrole", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_xlink:from", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_xlink:to", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_priority", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_order", typeof(string));
                    ds.Tables["link_definitionLink"].Columns.Add("defArc_use", typeof(string));
                    #endregion =======================================================================
                    break;

            }
            return ds;
        }
        

        /// <summary>
        /// DataSet에 데이터 Insert
        /// </summary>
        /// <param name="fe">파일 타입</param>
        /// <param name="root">root 요소</param>
        /// <param name="ds">데이터 추가할 DataSet</param>
        /// <param name="i">하위노드 순번</param>
        private void InsertRow(FileType ft, XmlElement root, DataSet ds, int i)
        {
            XmlNode childNode = root.ChildNodes[i];
            string nodeName = string.Empty;
            string sheetName = string.Empty;
            int strIndex = 0;

            DataRow dr;
            switch (ft)
            {
                case FileType.XBRL:
                    #region XBRL
                    // 1. 최상위 노드들을 각 Excel Sheet명으로 지정해주기 위한 작업(ex. dart-gcd:Amendment >> dart-gcd, dart-gcd:AuthorName >> dart-gcd...)
                    nodeName = childNode.Name == "#text" ? childNode.ParentNode.Name : childNode.Name;
                    strIndex = nodeName.IndexOf(':') == -1 ? nodeName.Length : nodeName.IndexOf(':');
                    sheetName = nodeName.Substring(0, strIndex);
                    if (sheetName.Contains("entity") == true)
                        sheetName = "entity";

                    // 2. 각 시트(테이블)에 노드별 데이터 추가
                    dr = ds.Tables[sheetName].Rows.Add();
                    if(sheetName == "link")
                    {

                    }
                    else if(sheetName == "context")
                    {
                        Nodes_XBRL.Context context = new Nodes_XBRL.Context(childNode);
                        dr["id"] = context.Context_id;
                        dr["entity_identifier"] = context.Entity_identifier;
                        dr["period_startDate"] = context.Period_startDate;
                        dr["period_endDate"] = context.Period_endDate;
                        dr["period_instant"] = context.Period_instance;
                        dr["dimension_id1"] = context.Dimension_id[0];
                        dr["dimension_value1"] = context.Dimension_value[0];
                        dr["dimension_id2"] = context.Dimension_value[1];
                        dr["dimension_value2"] = context.Dimension_value[1];
                    }
                    else if (sheetName == "unit")
                    {
                        Nodes_XBRL.Unit unit = new Nodes_XBRL.Unit(childNode);
                        dr["unit_id"] = unit.Unit_Id;
                        dr["unit_value"] = unit.Unit_Value;
                    }
                    else if (sheetName == "dart-gcd")
                    {
                        Nodes_XBRL.Dart_gcd dart_gcd = new Nodes_XBRL.Dart_gcd(childNode);
                        dr["namespace"] = dart_gcd.DartGcd_Ns;
                        dr["contextRef"] = dart_gcd.DartGcd_ConRef;
                        dr["xml_lang"] = dart_gcd.DartGcd_xmlLang;
                        dr["decimals"] = dart_gcd.DartGcd_decimals;
                        dr["unitRef"] = dart_gcd.DartGcd_unitRef;
                        dr["value"] = dart_gcd.DartGcd_value;
                    }
                    else if (sheetName == "dart")
                    {
                        Nodes_XBRL.Dart dart = new Nodes_XBRL.Dart(childNode);
                        dr["namespace"] = dart.Dart_Ns;
                        dr["contextRef"] = dart.Dart_conRef;
                        dr["decimals"] = dart.Dart_decimals;
                        dr["unitRef"] = dart.Dart_unitRef;
                        dr["value"] = dart.Dart_value;
                    }
                    else if (sheetName == "entity")
                    {
                        Nodes_XBRL.Entity entity = new Nodes_XBRL.Entity(childNode);
                        dr["namespace"] = entity.Entity_Ns;
                        dr["contextRef"] = entity.Entity_conRef;
                        dr["decimals"] = entity.Entity_decimals;
                        dr["unitRef"] = entity.Entity_unitRef;
                        dr["value"] = entity.Entity_value;
                    }
                    else if (sheetName == "ifrs-full")
                    {
                        Nodes_XBRL.Ifrs_Full ifrs_full = new Nodes_XBRL.Ifrs_Full(childNode);
                        dr["namespace"] = ifrs_full.Ifrs_Full_Ns;
                        dr["contextRef"] = ifrs_full.Ifrs_Full_conRef;
                        dr["decimals"] = ifrs_full.Ifrs_Full_decimals;
                        dr["unitRef"] = ifrs_full.Ifrs_Full_unitRef;
                        dr["value"] = ifrs_full.Ifrs_Full_value;
                    }
                    #region 주석
                    //switch (sheetName)
                    //{
                    //    case "link":
                    //        break;

                    //    case "context":
                    //        Nodes_XBRL.Context context = new Nodes_XBRL.Context(childNode);
                    //        dr["id"] = context.Context_id;
                    //        dr["entity_identifier"] = context.Entity_identifier;
                    //        dr["period_startDate"] = context.Period_startDate;
                    //        dr["period_endDate"] = context.Period_endDate;
                    //        dr["period_instant"] = context.Period_instance;
                    //        dr["dimension_id1"] = context.Dimension_id[0];
                    //        dr["dimension_value1"] = context.Dimension_value[0];
                    //        dr["dimension_id2"] = context.Dimension_value[1];
                    //        dr["dimension_value2"] = context.Dimension_value[1];
                    //        break;

                    //    case "unit":
                    //        Nodes_XBRL.Unit unit = new Nodes_XBRL.Unit(childNode);
                    //        dr["unit_id"] = unit.Unit_Id;
                    //        dr["unit_value"] = unit.Unit_Value;
                    //        break;

                    //    case "dart-gcd":
                    //        Nodes_XBRL.Dart_gcd dart_gcd = new Nodes_XBRL.Dart_gcd(childNode);
                    //        dr["namespace"] = dart_gcd.DartGcd_Ns;
                    //        dr["contextRef"] = dart_gcd.DartGcd_ConRef;
                    //        dr["xml_lang"] = dart_gcd.DartGcd_xmlLang;
                    //        dr["decimals"] = dart_gcd.DartGcd_decimals;
                    //        dr["unitRef"] = dart_gcd.DartGcd_unitRef;
                    //        dr["value"] = dart_gcd.DartGcd_value;
                    //        break;

                    //    case "dart":
                    //        Nodes_XBRL.Dart dart = new Nodes_XBRL.Dart(childNode);
                    //        dr["namespace"] = dart.Dart_Ns;
                    //        dr["contextRef"] = dart.Dart_conRef;
                    //        dr["decimals"] = dart.Dart_decimals;
                    //        dr["unitRef"] = dart.Dart_unitRef;
                    //        dr["value"] = dart.Dart_value;
                    //        break;

                    //    case "entity00126380":
                    //        Nodes_XBRL.Entity entity = new Nodes_XBRL.Entity(childNode);
                    //        dr["namespace"] = entity.Entity_Ns;
                    //        dr["contextRef"] = entity.Entity_conRef;
                    //        dr["decimals"] = entity.Entity_decimals;
                    //        dr["unitRef"] = entity.Entity_unitRef;
                    //        dr["value"] = entity.Entity_value;
                    //        break;

                    //    case "ifrs-full":
                    //        Nodes_XBRL.Ifrs_Full ifrs_full = new Nodes_XBRL.Ifrs_Full(childNode);
                    //        dr["namespace"] = ifrs_full.Ifrs_Full_Ns;
                    //        dr["contextRef"] = ifrs_full.Ifrs_Full_conRef;
                    //        dr["decimals"] = ifrs_full.Ifrs_Full_decimals;
                    //        dr["unitRef"] = ifrs_full.Ifrs_Full_unitRef;
                    //        dr["value"] = ifrs_full.Ifrs_Full_value;
                    //        break;
                    //}
                    #endregion
                    break;
                    #endregion

                case FileType.LABEL_KO:
                    #region Label
                    nodeName = childNode.Name == "#text" ? childNode.ParentNode.Name : childNode.Name;
                    sheetName = nodeName.Replace(":", "_"); // 시트명에 ':' 추가 안됨...

                    switch(sheetName)
                    {
                        case "link_roleRef":
                            dr = ds.Tables[sheetName].Rows.Add();
                            Nodes_Label.RoleRef rolRef = new Nodes_Label.RoleRef(childNode);
                            dr["roleURI"] = rolRef.Role_Uri;
                            dr["xlink:type"] = rolRef.Type;
                            dr["xlink:href"] = rolRef.Href;
                            break;

                        case "link_labelLink":
                            Nodes_Label.LabelLink label = new Nodes_Label.LabelLink();
                            int ord = 0;
                            // 여기서 돌려서 각각 Loc, Label, LabelArc 만들어야겟다
                            for (int j = 0; j < childNode.ChildNodes.Count; j++)
                            {
                                XmlNode childNode2 = childNode.ChildNodes[j];
                                label.InitSturct(childNode2, j);

                                // 3개의 row를 하나의 row로 변환. 각 row마다 inner class로 구현함. 이해안되면 xbrl의 label 파일을 보쟈...
                                // if ((j != 0) && (j % 3 == 2))
                                if((j != 0) && (childNode2.Name == "link:labelArc"))
                                {
                                    ds.Tables[sheetName].Rows.Add();
                                    ds.Tables[sheetName].Rows[ord]["loc_xlink:href"] = label.lb.loc.Loc_xlinkHref;
                                    ds.Tables[sheetName].Rows[ord]["loc_xlink:label"] = label.lb.loc.Loc_xlinkLabel;
                                    ds.Tables[sheetName].Rows[ord]["loc_xlink:hrefVal"] = label.Loc_HrefVal;

                                    ds.Tables[sheetName].Rows[ord]["label_xlink:role"] = label.lb.label.Label_xlinkRole;
                                    ds.Tables[sheetName].Rows[ord]["label_xlink:label"] = label.lb.label.Label_xlinkLabel;
                                    ds.Tables[sheetName].Rows[ord]["label_xlink:lang"] = label.lb.label.Label_xlinkLang;
                                    ds.Tables[sheetName].Rows[ord]["label_id"] = label.lb.label.Label_Id;
                                    ds.Tables[sheetName].Rows[ord]["label_value"] = label.lb.label.Label_Value;

                                    ds.Tables[sheetName].Rows[ord]["labelArc_xlink:from"] = label.lb.labelArc.Arc_xLinkFrom;
                                    ds.Tables[sheetName].Rows[ord]["labelArc_xlink:to"] = label.lb.labelArc.Arc_xLinkTo;

                                    ds.Tables[sheetName].Rows[ord]["Loc_HrefVal"] = label.Loc_HrefVal;
                                    ds.Tables[sheetName].Rows[ord]["Xbrl_Namespace"] = label.Xbrl_Namespace;
                                    ds.Tables[sheetName].Rows[ord]["Xbrl_ItemCode"] = label.Xbrl_ItemCode;

                                    ord++;
                                }

                            }
                            ord = 0;
                            break;
                    }

                    break;
                #endregion

                case FileType.LINK_PRE:
                    #region Link Pre
                    nodeName = childNode.Name == "#text" ? childNode.ParentNode.Name : childNode.Name;
                    sheetName = nodeName.Replace(":", "_"); // 시트명에 ':' 추가 안됨...
                    switch (sheetName)
                    {
                        case "link_roleRef":
                            dr = ds.Tables[sheetName].Rows.Add();
                            Nodes_Link.RoleRef rolRef = new Nodes_Link.RoleRef(childNode);
                            dr["roleURI"] = rolRef.Role_Uri;
                            dr["xlink:type"] = rolRef.Type;
                            dr["xlink:href"] = rolRef.Href;
                            break;

                        case "link_presentationLink":
                            Nodes_Link.PresentationArc pa = new Nodes_Link.PresentationArc();
                            string bfNodeName = string.Empty;
                            for (int j = 0; j < childNode.ChildNodes.Count; j++)
                            {
                                #region
                                //XmlNode childNode2 = childNode.ChildNodes[j];
                                //string sPresentationLink = childNode.Attributes["xlink:role"].Value;
                                //pa.InitSturct(childNode2, j);

                                //if (j % 2 == 0)
                                //{
                                //    ds.Tables[sheetName].Rows.Add();

                                //    ds.Tables[sheetName].Rows[_ord_Pre]["presentationLink_xlink_role"] = sPresentationLink;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["loc_xlink_href"] = pa.ps.loc.Loc_Href;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["loc_xlink_label"] = pa.ps.loc.Loc_Label;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["loc_xlink_type"] = pa.ps.loc.Loc_Type;

                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_type"] = pa.ps.pa.PA_Type;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_arcRole"] = pa.ps.pa.PA_ArcRole;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_from"] = pa.ps.pa.PA_From;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_to"] = pa.ps.pa.PA_To;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_priority"] = pa.ps.pa.PA_Priority;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_order"] = pa.ps.pa.PA_Order;
                                //    ds.Tables[sheetName].Rows[_ord_Pre]["preArc_use"] = pa.ps.pa.PA_Use;

                                //    _ord_Pre++;
                                //}
                                #endregion
                                XmlNode childNode2 = childNode.ChildNodes[j];
                                string sPresentationLink = childNode.Attributes["xlink:role"].Value;

                                if (j == 0)
                                    pa.InitSturct(childNode2, j);


                                if (childNode.ChildNodes[j].Name == bfNodeName && bfNodeName == "link:loc")
                                {
                                    AddRowPre(ds, sheetName, sPresentationLink, pa);
                                    _ord_Pre++;
                                    pa.ClearStruct();

                                    pa.InitSturct(childNode2, j);
                                }
                                else if (childNode.ChildNodes[j].Name == bfNodeName && bfNodeName == "link:presentationArc")
                                {
                                    pa.ClearStruct();
                                    pa.InitSturct(childNode2, j);
                                    AddRowPre(ds, sheetName, sPresentationLink, pa);
                                    _ord_Pre++;
                                }
                                else if (childNode.ChildNodes[j].Name != bfNodeName && bfNodeName == "link:loc" && childNode.ChildNodes[j].Name == "link:presentationArc")
                                {
                                    pa.InitSturct(childNode2, j);
                                    AddRowPre(ds, sheetName, sPresentationLink, pa);
                                    _ord_Pre++;

                                    pa.ClearStruct();
                                }
                                else if (childNode.ChildNodes[j].Name != bfNodeName && bfNodeName == "link:presentationArc" && childNode.ChildNodes[j].Name == "link:loc")
                                {
                                    pa.InitSturct(childNode2, j);
                                }
                                bfNodeName = childNode.ChildNodes[j].Name;
                                
                            }
                            break;
                    }

                    break;
                #endregion

                case FileType.LINK_DIM:
                    #region Link Dim
                    nodeName = childNode.Name == "#text" ? childNode.ParentNode.Name : childNode.Name;
                    sheetName = nodeName.Replace(":", "_");
                    switch (sheetName)
                    {
                        case "link_arcroleRef":
                            dr = ds.Tables[sheetName].Rows.Add();
                            Nodes_Link.ArcRoleRef arRef = new Nodes_Link.ArcRoleRef(childNode);
                            dr["arcroleURI"] = arRef.Arcrole_Uri;
                            dr["xlink:type"] = arRef.Type;
                            dr["xlink:href"] = arRef.Href;
                            break;

                        case "link_roleRef":
                            dr = ds.Tables[sheetName].Rows.Add();
                            Nodes_Link.RoleRef rolRef = new Nodes_Link.RoleRef(childNode);
                            dr["roleURI"] = rolRef.Role_Uri;
                            dr["xlink:type"] = rolRef.Type;
                            dr["xlink:href"] = rolRef.Href;
                            break;

                        case "link_definitionLink":
                            Nodes_Link.DefinitionLink da = new Nodes_Link.DefinitionLink();
                            for (int j = 0; j < childNode.ChildNodes.Count; j++)
                            {
                                XmlNode childNode2 = childNode.ChildNodes[j];
                                string sDefinitionLink = childNode.Attributes["xlink:role"].Value;
                                da.InitSturct(childNode2, j);

                                if (j % 2 == 0)
                                {
                                    ds.Tables[sheetName].Rows.Add();

                                    ds.Tables[sheetName].Rows[_ord_Def]["definitionLink_xlink:role"] = sDefinitionLink;
                                    ds.Tables[sheetName].Rows[_ord_Def]["loc_xlink:href"] = da.ds.loc.Loc_Href;
                                    ds.Tables[sheetName].Rows[_ord_Def]["loc_xlink:label"] = da.ds.loc.Loc_Label;
                                    ds.Tables[sheetName].Rows[_ord_Def]["loc_xlink:type"] = da.ds.loc.Loc_Type;

                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_xlink:type"] = da.ds.da.DA_Type;
                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_xlink:arcRole"] = da.ds.da.DA_ArcRole;
                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_xlink:from"] = da.ds.da.DA_From;
                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_xlink:to"] = da.ds.da.DA_To;
                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_priority"] = da.ds.da.DA_Priority;
                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_order"] = da.ds.da.DA_Order;
                                    ds.Tables[sheetName].Rows[_ord_Def]["defArc_use"] = da.ds.da.DA_Use;

                                    _ord_Def++;
                                }
                            }
                            break;
                    }
                    #endregion
                    break;

                case FileType.LINK_CAL:
                    #region Link Cal
                    #endregion
                    break;
            }
            
        }



        private void AddRowPre(DataSet ds, string sheetName, string sPresentationLink, Nodes_Link.PresentationArc pa)
        {
            ds.Tables[sheetName].Rows.Add();

            ds.Tables[sheetName].Rows[_ord_Pre]["presentationLink_xlink_role"] = sPresentationLink;
            ds.Tables[sheetName].Rows[_ord_Pre]["loc_xlink_href"] = pa.ps.loc.Loc_Href;
            ds.Tables[sheetName].Rows[_ord_Pre]["loc_xlink_label"] = pa.ps.loc.Loc_Label;
            ds.Tables[sheetName].Rows[_ord_Pre]["loc_xlink_type"] = pa.ps.loc.Loc_Type;

            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_type"] = pa.ps.pa.PA_Type;
            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_arcRole"] = pa.ps.pa.PA_ArcRole;
            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_from"] = pa.ps.pa.PA_From;
            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_xlink_to"] = pa.ps.pa.PA_To;
            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_priority"] = pa.ps.pa.PA_Priority;
            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_order"] = pa.ps.pa.PA_Order;
            ds.Tables[sheetName].Rows[_ord_Pre]["preArc_use"] = pa.ps.pa.PA_Use;
        }

        /// <summary>
        /// DataTable을 xml로 변환
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetStringXML(System.Data.DataTable dt)
        {
            System.Data.DataTable dt_copy = dt.Copy();


            DataSet ds = new DataSet("TABLE");
            dt_copy.TableName = "ROW";
            ds.Tables.Add(dt_copy);

            // TimeZone 설정 변경 이거 안하면 +09:00 이 돼서 시간이 변경됨.
            foreach (System.Data.DataTable dt1 in ds.Tables)
            {
                foreach (DataColumn dc in dt1.Columns)
                {
                    if (dc.DataType == typeof(DateTime)) { dc.DateTimeMode = DataSetDateTime.Unspecified; }
                }
            }

            XmlDataDocument xdd = new XmlDataDocument(ds);
            string sss = xdd.OuterXml;


            return xdd.OuterXml;
        }
        

        /// <summary>
        /// 폴더 트리뷰 초기화(이미지 아이콘)
        /// </summary>
        public void InitTreeViewFolder()
        {
            Image folderImage = (Image)Properties.Resources.ResourceManager.GetObject("folder");
            Image streamImage = (Image)Properties.Resources.ResourceManager.GetObject("file");
            

            tv_FolderList.ImageList = new ImageList();
            tv_FolderList.ImageList.Images.Add(folderImage);
            tv_FolderList.ImageList.Images.Add(streamImage);
        }

        /// <summary>
        /// 폴더 트리뷰 폴더 노드 추가
        /// </summary>
        /// <param name="Dir"></param>
        public void LoadDirectory(string Dir)
        {
            tv_FolderList.Nodes.Clear();

            DirectoryInfo di = new DirectoryInfo(Dir);
            TreeNode tn = tv_FolderList.Nodes.Add(di.Name);
            tn.Tag = di.FullName;
            LoadFiles(Dir, tn);
            LoadSubDirectories(Dir, tn);
            tv_FolderList.ExpandAll();
        }

        /// <summary>
        /// 폴더 트리뷰 파일 노드 추가
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="td"></param>
        private void LoadFiles(string dir, TreeNode td)
        {
            string[] Files = Directory.GetFiles(dir, "*.*");

            foreach (string file in Files)
            {
                FileInfo fi = new FileInfo(file);
                TreeNode tn = td.Nodes.Add(fi.Name);
                tn.Tag = fi.FullName;
                tn.ImageIndex = 1;
                tn.SelectedImageIndex = 1;
            }
        }

        /// <summary>
        /// 선택한 폴더 경로의 하위 디렉토리 모두 조회(재귀함수 호출)
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="td"></param>
        private void LoadSubDirectories(string dir, TreeNode td)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo di = new DirectoryInfo(subdirectory);
                TreeNode tn = td.Nodes.Add(di.Name);
                tn.ImageIndex = 0;
                tn.SelectedImageIndex = 0;
                tn.Tag = di.FullName;
                LoadFiles(subdirectory, tn);
                LoadSubDirectories(subdirectory, tn);
            }
        }


        


        /// <summary>
        /// 테슽흐 버튼 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Test_Click(object sender, EventArgs e)
        {
            // string xbrlFiles = txt_xbrlFilePath2.Text;
            // string patXBRL = @".xbrl$";
            // string patLabelKo = @"^lab_[0-9]{8}-ko_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$";
            // string patLabelEn = @"^lab_[0-9]{8}-en_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$";
            // string patLBCal = @"^cal_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$";
            // string patLBDim = @"^dim_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$";
            // string patLBPre = @"^pre_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$";
            // string patLBSchema = @"^role_ifrs_for_[0-9]{8}-dim_[0-9]{4}-[0-9]{2}-[0-9]{2}.xsd$";

            // Regex regInfo = new Regex(@".xbrl$|^lab_[0-9]{8}-ko_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$|^lab_[0-9]{8}-ko_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml|^lab_[0-9]{8}-en_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$|^cal_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$|^dim_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$|^pre_ifrs_for_[0-9]{8}_[0-9]{4}-[0-9]{2}-[0-9]{2}.xml$|^role_ifrs_for_[0-9]{8}-dim_[0-9]{4}-[0-9]{2}-[0-9]{2}.xsd$");

        }


        /// <summary>
        /// 테슽흐 버튼2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Test2_Click(object sender, EventArgs e)
        {

        }


        


        /// <summary>
        /// 입력 문자열 기반으로 하위노드 돌면서 문자열 검색. 
        /// 검색 결과가 있으면 해당 Node의 Text를 CurrentNodeMatches에 List형태로 저장
        /// </summary>
        /// <param name="SearchText"></param>
        /// <param name="StartNode"></param>
        private void SearchNodes(string SearchText, TreeNode StartNode)
        {
            TreeNode node = null;
            while (StartNode != null)
            {
                if (StartNode.Text.ToLower().Contains(SearchText.ToLower()))
                {
                    _CurrentNodeMatches.Add(StartNode);
                }
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodes(SearchText, StartNode.Nodes[0]);//Recursive Search 
                }
                StartNode = StartNode.NextNode;
            }
        }




        private string GetRptCd(string sRptNm)
        {
            string rslt = string.Empty;

            switch (sRptNm)
            {
                case "사업보고서":
                    rslt = "A001";
                    break;
                case "반기보고서":
                    rslt = "A002";
                    break;
                case "분기보고서":
                    rslt = "A003";
                    break;
                case "등록법인결산서류(자본시장법이전)":
                    rslt = "A004";
                    break;
                case "소액공모법인결산서류":
                    rslt = "A005";
                    break;
            }

            return rslt;
        }

        private string GetQtrType(string sRptNm)
        {
            string rslt = string.Empty;
            switch (sRptNm)
            {
                case "사업보고서":
                    rslt = "AA";
                    break;
                case "분기보고서":
                    rslt = "QQ";
                    break;
                default:
                    rslt = "N";
                    break;
            }
            return rslt;
        }

        
    }
}
