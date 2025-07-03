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
    public class BL_CUSTOMER_MASTER
    {
        public DataTable BL_ExecuteTask(PL_CUSTOMER_MASTER objPl)
        {
            DL_CUSTOMER_MASTER objDl = new DL_CUSTOMER_MASTER();
            return objDl.DL_ExecuteTask(objPl);
        }


    }
}
