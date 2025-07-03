using DNH_PL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_DL;


namespace DNH_BL
{
    public class BL_REVISION_MASTER
    {
        public DataTable BL_ExecuteTask(PL_REVISION_MASTER objPl)
        {
            DL_REVISION_MASTER objDl = new DL_REVISION_MASTER();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
