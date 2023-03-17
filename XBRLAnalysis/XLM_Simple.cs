using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace XBRLAnalysis
{
    class XLM_Simple : ApplicationClass, IDisposable
    {
        private bool disposed = false;

        //  0x80028018(TYPE_E_INVDATAREAD) 에러에 대한 처리
        System.Globalization.CultureInfo Culture = null;

        private static Workbook Wb = null;
        private static Workbooks Wbs = null;
        private static Microsoft.Office.Interop.Excel._Application excel_application = null;



        public XLM_Simple()
        {
            try
            {
                Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(this.LanguageSettings.get_LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI));
            }
            catch { }
        }


        public static void New(_Application XL)
        {
            try
            {
                Wbs = XL.Workbooks;
                Wb = Wbs.Add(XlWBATemplate.xlWBATWorksheet);
                XL.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Wbs != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Wbs);

                if (Wb != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Wb);
            }
        }


        /// <summary>
        /// Excel 오브젝트 파일 Open
        /// </summary>
        /// <param name="XL">새로운 Excel 오브젝트</param>
        /// <param name="FileName">Excel 파일 명(경로 포함)</param>
        /// <param name="Visible">Visible 여부</param>
        /// <param name="Password">Excel Workbook 패스워드(없을시 Type.Missing)</param>
        public static void Open(_Application XL, string FileName, bool Visible, object Password)
        {
            if (XL == null)
            {
                XL = new Microsoft.Office.Interop.Excel.Application();
            }

            try
            {
                Wbs = XL.Workbooks;
                Wb = Wbs.Open(FileName, Type.Missing, Type.Missing, Type.Missing, Password, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                XL.Visible = Visible;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Wbs != null)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Wbs);
                    Wbs = null;
                }

                if (Wb != null)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Wb);
                    Wb = null;
                }
            }
        }


        /// <summary>
        /// 현재 떠있는 엑셀 객체를 가져오는 메서드.
        /// </summary>
        /// <returns>Excel 오브젝트</returns>
        public static _Application GetActiveExcel()
        {
            // Microsoft.Office.Interop.Excel._Application excel_application = null;

            try
            {
                excel_application = (System.Runtime.InteropServices.Marshal.GetActiveObject("EXCEL.Application") as _Application);
            }
            catch (Exception ex)
            {
                excel_application = null;
            }
            return excel_application ?? new Microsoft.Office.Interop.Excel.Application();
        }


        /// <summary>
        /// Excel Sheet 추가(우측)
        /// </summary>
        /// <param name="XL">Excel 오브젝트</param>
        /// <param name="SheetName">Sheet명</param>
        /// <returns>생성 Sheet 객체</returns>
        public static _Worksheet AddSheet(_Application XL, string SheetName)
        {
            _Worksheet LSheet = null;
            _Worksheet Sheet = null;

            try
            {
                LSheet = (_Worksheet)XL.Worksheets[XL.Worksheets.Count];    // 마지막 시트 가져 온다
                Sheet = (_Worksheet)XL.Worksheets.Add(Type.Missing, LSheet, Type.Missing, XlSheetType.xlWorksheet);
                Sheet.Name = SheetName;
            }
            finally
            {
                if (LSheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(LSheet);

                if (Sheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Sheet);
            }

            return Sheet;
        }

        public void SheetRename(string SheetName, string NewSheetName)
        {
            _Worksheet Sheet = null;

            try
            {
                Sheet = (_Worksheet)this.Worksheets[SheetName];
                Sheet.Activate();
                Sheet.Name = NewSheetName;
            }
            finally
            {
                if (Sheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Sheet);
            }
        }

        public static void SetDataOnCell(_Application XL, string SheetName, long Row, long Col, System.Data.DataTable Data)
        {
            Range Range = null;
            _Worksheet Sheet = null;
            object[,] DataArray;

            if (Data == null) return;

            if (Data.Rows.Count == 0) return;

            try
            {
                if (string.IsNullOrEmpty(SheetName))
                    Sheet = (_Worksheet)XL.Worksheets[1];
                else
                    Sheet = (_Worksheet)XL.Worksheets[SheetName];
                Range = (Range)Sheet.Cells[Row, Col];

                DataArray = DataSetToArray(Data);
                Range.get_Resize(DataArray.GetUpperBound(0) + 1, DataArray.GetUpperBound(1) + 1).Value2 = DataArray;
            }
            finally
            {
                if (Range != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Range);

                if (Sheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Sheet);
            }
        }

        public static void SetDataOnCell(_Application XL, string SheetName, long Row, long Col, string Data)
        {
            Range Range = null;
            _Worksheet Sheet = null;

            try
            {
                Sheet = (_Worksheet)XL.Worksheets[SheetName];
                Range = (Range)Sheet.Cells[Row, Col];
                Range.Value2 = Data;
            }
            finally
            {
                if (Range != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Range);

                if (Sheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Sheet);
            }
        }


        /// <summary>
        /// 데이터 테이블을 배열로 변환하는 함수
        /// </summary>
        /// <param name="DataTable">변환할 DataTable</param>
        /// <returns></returns>
        public static object[,] DataSetToArray(System.Data.DataTable DataTable)
        {
            object[,] ob_Array = new object[DataTable.Rows.Count, DataTable.Columns.Count];
            int col = 0;
            int row = 0;

            foreach (System.Data.DataRow DataRow in DataTable.Rows)
            {
                foreach (System.Data.DataColumn DataColumn in DataTable.Columns)
                {
                    if (DataRow[DataColumn] == System.DBNull.Value)
                        ob_Array[row, col] = "";
                    else
                        ob_Array[row, col] = DataRow[DataColumn];

                    col++;
                }
                col = 0;
                row++;
            }

            return ob_Array;
        }


        /// <summary>
        /// 다른 이름으로 저장
        /// </summary>
        /// <param name="XL">Excel 오브젝트</param>
        /// <param name="s_FileName">저장할 파일명</param>
        public static void OpenSaveDialog(_Application XL, string s_FileName)
        {
            object dlgAnswer = new object();

            // dlgAnswer = XL.GetSaveAsFilename(s_FileName + ".xlsm", "Excel 매크로 사용 통합 문서(*.xlsm),*.xlsm,Microsoft Excel 통합 문서(*.xls),*.xls", 1, null, null);
            dlgAnswer = XL.GetSaveAsFilename(s_FileName + ".xlsx", "Excel 문서(*.xlsx),*.xlsx, Microsoft Excel 통합 문서(*.xls),*.xls", 1, null, null);
            try
            {
                if (dlgAnswer.ToString() != "False")
                {
                    if (dlgAnswer.ToString().Substring(dlgAnswer.ToString().Length - 4, 4).ToUpper() == ".XLS")
                    {
                        XL.ActiveWorkbook.SaveAs(dlgAnswer
                                                 , XlFileFormat.xlWorkbookNormal
                                                 , Type.Missing
                                                 , Type.Missing
                                                 , false
                                                 , false
                                                 , XlSaveAsAccessMode.xlExclusive
                                                 , false
                                                 , false
                                                 , false
                                                 , false
                                                 , Type.Missing
                                                 );
                    }
                    else if (dlgAnswer.ToString().Substring(dlgAnswer.ToString().Length - 5, 5).ToUpper() == ".XLSX")
                    {
                        XL.ActiveWorkbook.SaveAs(dlgAnswer
                                                 , XlFileFormat.xlWorkbookNormal
                                                 , Type.Missing
                                                 , Type.Missing
                                                 , false
                                                 , false
                                                 , XlSaveAsAccessMode.xlExclusive
                                                 , false
                                                 , false
                                                 , false
                                                 , false
                                                 , Type.Missing
                                                 );
                    }
                }
            }
            catch
            {
                // 기존 파일이 있는경우에 [취소] 를 하면 에러 발생하는데 이유몰르겠음;;;
            }
        }



        public virtual void Dispose(bool disposing)
        {
            if (Culture != null)
                System.Threading.Thread.CurrentThread.CurrentCulture = Culture;

            if (disposed == false)
            {
                if (disposing)
                {

                }

                if (Wbs != null)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Wbs);
                    Wbs = null;
                }

                if (Wb != null)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Wb);
                    Wb = null;
                }
                if(excel_application != null)
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel_application);
                    excel_application = null;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(this);
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
