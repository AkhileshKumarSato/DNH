using DNH_DL;
using DNH_PL;
using System.Data;

namespace DNH_BL
{
    public class BL_LABEL_REPRINTING
    {
        public DataTable BL_ExecuteTask(PL_LABEL_REPRINTING objPl)
        {
            DL_LABEL_REPRINTING objDl = new DL_LABEL_REPRINTING();
            return objDl.DL_ExecuteTask(objPl);
        }
    }
}
