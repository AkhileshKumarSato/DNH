
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
    public class DL_CUSTOMER_MASTER
    {

        
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_CUSTOMER_MASTER obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[6];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@CUST_CODE", SqlDbType.VarChar, 50);
                param[1].Value = obj.Cust_Code;
                param[2] = new SqlParameter("@CUST_NAME", SqlDbType.VarChar, 50);
                param[2].Value = obj.Cust_Name;
                param[3] = new SqlParameter("@CUST_ADDRESS", SqlDbType.VarChar, 50);
                param[3].Value = obj.Cust_Address;
                param[4] = new SqlParameter("@KANBAN", SqlDbType.VarChar, 500);
                param[4].Value = obj.Kanban;
                param[5] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 50);
                param[5].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_CUSTOMER_MASTER]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion      
    }
}
