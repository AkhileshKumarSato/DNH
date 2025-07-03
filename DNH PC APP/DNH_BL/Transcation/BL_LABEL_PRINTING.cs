using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_DL;
using DNH_PL;

namespace DNH_BL
{
    public class BL_LABEL_PRINTING
    {
        public DataTable BL_ExecuteTask(PL_LABEL_PRINTIING objPl)
        {
            DL_LABEL_PRINTING objDl = new DL_LABEL_PRINTING();
            return objDl.DL_ExecuteTask(objPl);
        }
    }
}
