using DNH_COMMON;

namespace DNH_PL
{
    public class PL_LABEL_REPRINTING : Common
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PartNo { get; set; }
        public string PartDis { get; set; }
        public string LotNo { get; set; }
        public string DateOfPacking { get; set; }
        public int Qty { get; set; }
        public string Operator { get; set; }
        public string Barcode { get; set; }
        public string SerialNo { get; set; }
        public string PackType { get; set; }

    }
}
