using DNH_COMMON;
using DNH_PL;
using SatoLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNH_DL
{
    public class DL_LABEL_REPRINTING
    {
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_LABEL_REPRINTING obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[11];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@FROM_DATE", SqlDbType.VarChar, 50);
                param[1].Value = obj.FromDate;
                param[2] = new SqlParameter("@TO_DATE", SqlDbType.VarChar, 50);
                param[2].Value = obj.ToDate;
                param[3] = new SqlParameter("@PART_NO", SqlDbType.VarChar, 50);
                param[3].Value = obj.PartNo;
                param[4] = new SqlParameter("@LOT_NO", SqlDbType.VarChar, 500);
                param[4].Value = obj.LotNo;
                param[5] = new SqlParameter("@SERIAL_NO", SqlDbType.VarChar, 50);
                param[5].Value = obj.SerialNo;
                param[6] = new SqlParameter("@BARCODE", SqlDbType.VarChar, 500);
                param[6].Value = obj.Barcode;
                param[7] = new SqlParameter("@PACK_TYPE", SqlDbType.VarChar, 500);
                param[7].Value = obj.PackType;
                param[8] = new SqlParameter("@QTY", SqlDbType.VarChar, 500);
                param[8].Value = obj.Qty;
                param[9] = new SqlParameter("@RE_PRINT_USER", SqlDbType.VarChar, 500);
                param[9].Value = obj.CreatedBy;


                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_LABEL_REPRINTING]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion  
    }
}
