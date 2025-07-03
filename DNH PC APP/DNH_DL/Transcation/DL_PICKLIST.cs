using DNH_COMMON;
using DNH_PL;
using SatoLib;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DNH_DL
{
    public class DL_PICKLIST
    {
        private SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_PICKLIST obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[8];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@CUSTOMER_CODE", SqlDbType.VarChar, 50);
                param[1].Value = obj.Cust_Code;
                param[2] = new SqlParameter("@PICKLIST_NO", SqlDbType.VarChar, 50);
                param[2].Value = obj.Picklist_No;
                param[3] = new SqlParameter("@PICKLIST_DATE", SqlDbType.VarChar, 50);
                param[3].Value = obj.Picklist_Date;
                param[4] = new SqlParameter("@PART_NO", SqlDbType.VarChar, 50);
                param[4].Value = obj.Part_No;
                param[5] = new SqlParameter("@PICKLIST_QTY", SqlDbType.VarChar, 50);
                param[5].Value = obj.Picklist_Qty;
                param[6] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 50);
                param[6].Value = obj.CreatedBy;
                if (obj.dtPicklist != null)
                {
                    param[7] = new SqlParameter("@PICKLIST_TBL", SqlDbType.Structured, 50);
                    param[7].Value = obj.dtPicklist;
                }


                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_PICKLIST_GEN]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion  

    }
}
