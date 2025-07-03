using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SatoLib;

namespace DNH_COMMON
{
    public class Common : IDisposable
    {
        
        public string CreatedBy { get; set; }
        public string Cust_Code { get; set; }
        public int ModuleId { get; set; }
        public string DbType { get; set; }
        private SqlHelper _SqlHelper = null;
        public DataTable GetGroup()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "SELECT_GROUP";

                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_BIND_COMBO]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetPart()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "BIND_PART";

                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_BIND_COMBO]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetOperator()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "BIND_OPERATOR";

                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_BIND_COMBO]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetCustomer()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "BIND_CUSTOMER";

                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_BIND_COMBO]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetPicklistNoForPicking()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "BIND_PICKLIST_PICKING";

                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_BIND_COMBO]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetPicklistNoForDispatch()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "BIND_PICKLIST_DISPATCH";

                return _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_BIND_COMBO]", param).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetRevisionDetails()
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TYPE", SqlDbType.VarChar, 100);
                param[0].Value = "SELECT";

                DataTable dataTable= _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_REVISION_MASTER]", param).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    GlobalVariable.mRevNo = dataTable.Rows[0]["REV_CODE"].ToString();
                    GlobalVariable.mRevName= dataTable.Rows[0]["REV_NAME"].ToString();
                    GlobalVariable.mRevDate= dataTable.Rows[0]["REV_DATE"].ToString();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetVersion(string sVersionName)
        {

            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@APP_NAME", SqlDbType.VarChar, 100);
                param[0].Value = "PC";
                param[1] = new SqlParameter("@APP_VERSION", SqlDbType.VarChar, 100);
                param[1].Value = sVersionName;

                DataTable dataTable = _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_GET_VERSION]", param).Tables[0];
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void SetModuleChildSectionRights(string sModule, bool updateflag, params Button[] buttons)
        {
            try
            {
                string btnName = string.Empty;
                StringBuilder _SbQry = new StringBuilder();
                _SqlHelper = new SqlHelper();
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@GROUP_NAME", SqlDbType.VarChar, 100);
                param[0].Value = GlobalVariable.UserGroup;
                param[1] = new SqlParameter("@MODULE_NAME", SqlDbType.VarChar, 100);
                param[1].Value = sModule;
                DataTable dataTable = _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_SET_CHILDSEC_RIGHTS]", param).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    foreach (var item in buttons)
                    {
                        if (item == null) { continue; }
                        btnName = item.Name.Remove(0, 3);
                        if (updateflag)
                        {
                            if (dataTable.Columns["Save"].ColumnName.Contains(btnName))
                            {
                                item.Enabled = (bool)dataTable.Rows[0]["Update"];

                            }
                        }
                        else
                        {
                            item.Enabled = (bool)dataTable.Rows[0]["Save"];

                        }
                        if (dataTable.Columns["Delete"].ColumnName.Contains(btnName))
                            item.Enabled = (bool)dataTable.Rows[0]["Delete"];
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
               
            }
        }
        public void GiveAccessToPrint(string sType,string userId,string sPassword)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Type", SqlDbType.VarChar, 100);
                param[0].Value = sType;
                param[1] = new SqlParameter("@UserID", SqlDbType.VarChar, 100);
                param[1].Value = userId;
                param[2] = new SqlParameter("@Password", SqlDbType.VarChar, 100);
                param[2].Value = sPassword;
                DataTable dataTable= _SqlHelper.ExecuteDataset(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_UserMaster]", param).Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                    {
                        GlobalVariable.mAccessUser = dataTable.Rows[0]["MSG"].ToString();
                    }
                    else
                    {
                        throw new Exception(dataTable.Rows[0]["MSG"].ToString());
                    }


                }
            }
            catch (ArgumentNullException ex)
            {

                throw ex;
            }
        }
        public string GetBarcode(string sType)
        {
            _SqlHelper = new SqlHelper();
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@TrnType", SqlDbType.VarChar, 100);
                param[0].Value = sType;

                return Convert.ToString( _SqlHelper.ExecuteScalar(GlobalVariable.mMainSqlConString, CommandType.StoredProcedure, "[PRC_GetRunningSerial]", param));
            }
            catch (ArgumentNullException ex)
            {

                throw ex;
            }
        }
  
        public string LablePrint(string partNo,string partDis,string lotNo,string dateOfPacking,string qty,string oppName,string barcode)
        {
            try
            {
                if (File.Exists(Application.StartupPath + "\\" + GlobalVariable.mPrnFileName))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\" + GlobalVariable.mPrnFileName);
                    string PrnFileTemp = sr.ReadToEnd();
                    sr.Close();

                    PrnFileTemp = PrnFileTemp.Replace("{VARREVNO}", GlobalVariable.mRevNo);
                    PrnFileTemp = PrnFileTemp.Replace("{VARREVNAME}", GlobalVariable.mRevName);
                    PrnFileTemp = PrnFileTemp.Replace("{VARREVDATE}", GlobalVariable.mRevDate);
                    PrnFileTemp = PrnFileTemp.Replace("{VARPARTNO}", partNo);
                    PrnFileTemp = PrnFileTemp.Replace("{VARPARTDIS}", partDis);
                    PrnFileTemp = PrnFileTemp.Replace("{VARLOTNO}", lotNo);
                    PrnFileTemp = PrnFileTemp.Replace("{VARDOP}", dateOfPacking);
                    PrnFileTemp = PrnFileTemp.Replace("{VARQTY}", qty);
                    PrnFileTemp = PrnFileTemp.Replace("{VAROPPNAMESRNO}", oppName+"/"+barcode.Substring(barcode.Length-4));
                    PrnFileTemp = PrnFileTemp.Replace("{VARBARLEN}", "DN" + barcode.Length.ToString().PadLeft(4, '0'));
                    PrnFileTemp = PrnFileTemp.Replace("{VARBARCODE}", barcode);
                    return PrintBarcode.PrintCommand(PrnFileTemp, GlobalVariable.mPrinterName);
                }
                else
                    throw new NoNullAllowedException( "Prn File " + GlobalVariable.mPrnFileName + " not found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }


    }
}
