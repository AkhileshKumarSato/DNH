using DNH_DL;
using DNH_PL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNH_BL
{
    public class BL_PICKING_UPLOAD_DOWNLOAD
    {
        public DataTable BL_ExecuteTask(PL_PICKING_UPLOAD_DOWNLOAD objPl)
        {
            DL_PICKING_UPLOAD_DOWNLOAD objDl = new DL_PICKING_UPLOAD_DOWNLOAD();
            return objDl.DL_ExecuteTask(objPl);
        }
    }
}
