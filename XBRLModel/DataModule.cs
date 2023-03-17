using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBRLModel
{
    public partial class DataModule : MSSQL
    {
        private static new DataModule Instance;

        public static DataModule GetDataModule()
        {
            if(Instance == null)
            {
                Instance = new DataModule();
            }
            return Instance;
        }


        public class Param
        {
            /// <summary>
            /// 파라미터 명
            /// </summary>
            public string _Name;
            /// <summary>
            /// 파라미터 변수 타입

            /// </summary>
            public System.Data.SqlDbType _Type;
            /// <summary>
            /// 파라미터 값
            /// 

            /// </summary>
            public object _Value;



            /// <summary>
            /// 프로시저 파라미터
            /// </summary>
            /// <param name="s_ParamNM">파라미터 명</param>
            /// <param name="param_Type">파라미터 데이터타입</param>
            /// <param name="param_Value">파라미터 값</param>
            public Param(string s_ParamNM, System.Data.SqlDbType param_Type, object param_Value)
            {
                _Name = s_ParamNM;
                _Type = param_Type;
                _Value = param_Value;
            }
        }
    }
}
