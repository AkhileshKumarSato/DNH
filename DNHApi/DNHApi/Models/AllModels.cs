using System;
using System.Collections.Generic;

namespace DNHApi.Models
{
    public enum EnumDbType { BIND_MODEL, SAVE };

    public class Common
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DbType { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class PL_PART_MASTER : Common
    {
        public string Part_No { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string PackType { get; set; }
        public string UnitValue { get; set; }

        //public List<PL_PART_MASTER> lstModel = new List<PL_PART_MASTER>();

    }
    public class PL_LABEL_PRINTIING
    {
        public string PackType { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public int PackQty { get; set; }
        public int TotalQty { get; set; }
        public int Qty { get; set; }
        public string LotNo { get; set; }
        public string Barcode { get; set; }
        public string AccessUser { get; set; }
        public string Operator { get; set; }
        public string PackingDate { get; set; }
        public string PackingDateOnLabel { get; set; }
    }
    public class PL_LABEL_PRINTIING_RETURN:Common
    {
        public string Barcode { get; set; }
        public string PackingDateOnLabel { get; set; }
    }



}