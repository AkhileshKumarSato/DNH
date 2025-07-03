using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using DNHApi.Models;
using System.Data.SqlClient;

namespace DNHApi.Dal
{
    public class DataAccess
    {
        StringBuilder _SbQry;

        #region DNH API

        public DataTable DL_EXECUTE_PART(PL_PART_MASTER opl)
        {
            DataTable DT = new DataTable();
            try
            {
                Datautility DU = new Datautility();
                SqlParameter[] parma = {
                                        new SqlParameter("@TYPE",opl.DbType),


                                   };

                DT = DU.GetDataUsingProcedure("[PRC_PART_MASTER]", parma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return DT;
        }
        public DataTable DL_EXECUTE_PRINT(PL_LABEL_PRINTIING opl)
        {
            DataTable DT = new DataTable();
            try
            {
                Datautility DU = new Datautility();
                SqlParameter[] parma = {
                                        new SqlParameter("@TYPE","PRINT_SAVE"),
                                          new SqlParameter("@PART_NO",opl.PartNo),
                                          new SqlParameter("@PACK_TYPE",opl.PackType),
                                            new SqlParameter("@TOTAL_QTY",opl.Qty),
                                            new SqlParameter("@PACK_QTY",opl.Qty),
                                              new SqlParameter("@QTY",opl.Qty ),
                                               new SqlParameter("@LOT_NO",opl.LotNo ),
                                                new SqlParameter("@BARCODE",opl.Barcode ),
                                                 new SqlParameter("@ACCESS_USER",opl.AccessUser ),
                                                     new SqlParameter("@OPERATOR",opl.Operator ),
                                                       new SqlParameter("@PACKING_DATE",opl.PackingDate ),
                                                new SqlParameter("@PRINTED_BY",opl.Operator),

                                   };

                DT = DU.GetDataUsingProcedure("[PRC_LABEL_PRINTING]", parma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return DT;
        }
        public string GetBarcode(string sType)
        {
            Datautility DU = new Datautility();
            try
            {

                SqlParameter[] parma = { new SqlParameter("@TrnType", sType) };
                return Convert.ToString(DU.ExecuteScalar("[PRC_GetRunningSerial]", parma));
            }
            catch (ArgumentNullException ex)
            {

                throw ex;
            }
        }

        #endregion


    }
}
