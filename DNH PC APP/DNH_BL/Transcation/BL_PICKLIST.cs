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
   public class BL_PICKLIST
    {
        public DataTable BL_ExecuteTask(PL_PICKLIST objPl)
        {
            DL_PICKLIST objDl = new DL_PICKLIST();
            return objDl.DL_ExecuteTask(objPl);
        }
    }
}
