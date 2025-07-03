using DNHApi.Dal;
using DNHApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace DNHApi.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpGet]
        public string GetTestValue()
        {
            return "Working";
        }



        #region Barcode Printing Line

        /*
      * This action method will return the back no details
      */
        public string AddDoubleQuotes(string value)
        {
            return "\"" + value + "\"";
        }
        /// <summary>
        ///BIND PART MASTER
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/service/GetModel")]
        public List<PL_PART_MASTER> GetModel()
        {
            List<PL_PART_MASTER> _obj = new List<PL_PART_MASTER>();
            DataAccess dataAccess = new DataAccess();
            try
            {


                DataTable dtPart = dataAccess.DL_EXECUTE_PART(new PL_PART_MASTER { DbType = "SELECT" });
                if (dtPart.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPart.Rows.Count; i++)
                    {
                        _obj.Add(new PL_PART_MASTER()
                        {
                            Part_No = dtPart.Rows[i]["PART_NO"].ToString(),
                            Description = dtPart.Rows[i]["DESCRIPTION"].ToString(),
                            Qty = Convert.ToInt32(dtPart.Rows[i]["QTY"].ToString()),
                            PackType = dtPart.Rows[i]["PACK_TYPE"].ToString(),
                            UnitValue= dtPart.Rows[i]["UNIT_VALUE"].ToString(),
                            //CreatedBy = dtPart.Rows[i]["CREATED_BY"].ToString(),
                            //CreatedOn = Convert.ToDateTime(dtPart.Rows[i]["CREATED_ON"].ToString()),
                        });
                    }

                    //_obj.Response = "Y";
                    //_obj.ErrorMessage = "";

                }
                else
                {
                    //_obj.Response = "N";
                    //_obj.ErrorMessage = "Model  details not found ";
                }
            }
            catch (Exception ex)
            {
                //    _obj.Response = "N";
                //    _obj.ErrorMessage = "Error : " + ex.Message;
            }
            return _obj;
        }

        /*
        
        * Model parameter shoule be in the last otherwise AEP is having some problem
        */

        [HttpPost]
        [Route("api/service/Save")]
        public PL_LABEL_PRINTIING_RETURN Save(PL_LABEL_PRINTIING obj)
        {
            PL_LABEL_PRINTIING_RETURN _obj = new PL_LABEL_PRINTIING_RETURN();
            DataAccess dataAccess = new DataAccess();
            try
            {
                obj.Barcode = obj.PartNo + "~" + obj.Qty + "~" + obj.LotNo + "~" + dataAccess.GetBarcode("LABEL_PRINT");
                DataTable dt = dataAccess.DL_EXECUTE_PRINT(obj);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == "Y")
                    {
                        _obj.Barcode = obj.Barcode; //obj.PartNo + "~" + obj.Qty + "~" + obj.LotNo + "~" + dataAccess.GetBarcode("LABEL_PRINT");
                        _obj.PackingDateOnLabel = DateTime.Now.ToString("dd-MMM-yyyy");
                        _obj.Response = "Y";
                    }
                    else
                    {
                        _obj.Response = "N";
                        _obj.ErrorMessage = "Data Not Saved";
                    }
                }
                else
                {
                    _obj.Response = "N";
                    _obj.ErrorMessage = "Could not be saved, details not returned";
                }
            }
            catch (Exception ex)
            {
                _obj.Response = "N";
                _obj.ErrorMessage = "Error : " + ex.Message;
            }
            return _obj;
        }


        #endregion
    }
}
