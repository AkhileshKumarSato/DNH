using DNH_COMMON;

namespace DNH_PL
{
    public class PL_PICKLIST : Common
    {
        public string Picklist_No { get; set; }
        public string Picklist_Date { get; set; }
        public string Part_No { get; set; }
        public int Picklist_Qty { get; set; }
        public System.Data.DataTable dtPicklist { get; set; }
    }
}
