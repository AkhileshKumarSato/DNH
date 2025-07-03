using DNH_COMMON;
using DNH_PL;
using SatoLib;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DNH_DL
{
    public class DL_PICKING_UPLOAD_DOWNLOAD
    {
        private SqlHelper _SqlHelper = new SqlHelper();
        #region MyFuncation
        /// <summary>
        /// Execute Operation 
        /// </summary>
        /// <returns></returns>
        public DataTable DL_ExecuteTask(PL_PICKING_UPLOAD_DOWNLOAD obj)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = obj.DbType;

                param[1] = new SqlParameter("@PICKLIST_NO", SqlDbType.VarChar, 100);
                param[1].Value = obj.PicklistNo;
                param[2] = new SqlParameter("@CREATED_BY", SqlDbType.VarChar, 100);
                param[2].Value = obj.CreatedBy;
                if (obj.dtPickingDataDownload != null)
                {
                    param[3] = new SqlParameter("@PICKING_DATA_TBL", SqlDbType.Structured, 50);
                    param[3].Value = obj.dtPickingDataDownload;
                }
              
                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_PICKING_FILE_UPLOAD_DOWNLOAD]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion  

    }
}
