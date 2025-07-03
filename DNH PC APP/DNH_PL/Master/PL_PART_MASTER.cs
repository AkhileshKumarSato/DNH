using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_COMMON;

namespace DNH_PL
{
    public class PL_PART_MASTER : Common
    {
        public string Part_No { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string PackType { get; set; }
        public string UnitPerPiece { get; set; }

    }
}
