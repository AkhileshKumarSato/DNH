using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_COMMON;

namespace DNH_PL
{
    public class PL_LABEL_PRINTIING : Common
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
        public List<PL_LABEL_PRINTIING> lstPL_Printing = new List<PL_LABEL_PRINTIING>();
    }
}
