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
    public class DL_REVISION_MASTER
    {

        
        SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_REVISION_MASTER obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@Type", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;
                param[1] = new SqlParameter("@REVISION_CODE", SqlDbType.VarChar, 50);
                param[1].Value = obj.RevisionCode;
                param[2] = new SqlParameter("@REVSION_NAME", SqlDbType.VarChar, 50);
                param[2].Value = obj.RevisionName;
                param[3] = new SqlParameter("@REVISION_DATE", SqlDbType.VarChar, 50);
                param[3].Value = obj.RevisionDate;
                param[4] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 50);
                param[4].Value = obj.CreatedBy;
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_REVISION_MASTER]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion      
    }
}
