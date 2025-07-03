
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_COMMON;
using DNH_PL;

using SatoLib;


namespace DNH_DL
{
    public class DL_LABEL_PRINTING
    {

        
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_LABEL_PRINTIING obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[12];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@PACK_TYPE", SqlDbType.VarChar, 50);
                param[1].Value = obj.PackType;
                param[2] = new SqlParameter("@PART_NO", SqlDbType.VarChar, 50);
                param[2].Value = obj.PartNo;
                param[3] = new SqlParameter("@TOTAL_QTY", SqlDbType.VarChar, 50);
                param[3].Value = obj.TotalQty;
                param[4] = new SqlParameter("@PACK_QTY", SqlDbType.VarChar, 50);
                param[4].Value = obj.PackQty;
                param[5] = new SqlParameter("@QTY", SqlDbType.VarChar, 50);
                param[5].Value = obj.Qty;
                param[6] = new SqlParameter("@LOT_NO", SqlDbType.VarChar, 500);
                param[6].Value = obj.LotNo;
                param[7] = new SqlParameter("@BARCODE", SqlDbType.VarChar, 500);
                param[7].Value = obj.Barcode;
                param[8] = new SqlParameter("@ACCESS_USER", SqlDbType.VarChar, 50);
                param[8].Value = obj.AccessUser;
                param[9] = new SqlParameter("@OPERATOR", SqlDbType.VarChar, 50);
                param[9].Value = obj.Operator;
                param[10] = new SqlParameter("@PACKING_DATE", SqlDbType.VarChar, 50);
                param[10].Value = obj.PackingDate;
                param[11] = new SqlParameter("@PRINTED_BY", SqlDbType.VarChar, 50);
                param[11].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_LABEL_PRINTING]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion      
    }
}
