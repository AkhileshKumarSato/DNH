using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_COMMON;
namespace DNH_PL
{
    public class PL_PICKING_UPLOAD_DOWNLOAD:Common
    {
        public string PicklistNo { get; set; }
        public System.Data.DataTable dtPickingDataDownload { get; set; }
    }
}
