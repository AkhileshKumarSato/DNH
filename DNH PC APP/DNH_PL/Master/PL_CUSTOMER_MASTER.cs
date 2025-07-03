using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNH_COMMON;

namespace DNH_PL
{
    public class PL_CUSTOMER_MASTER:Common
    {
        public string Cust_Code { get; set; }
        public string Cust_Name { get; set; }
        public string Cust_Address { get; set; }
        public bool Kanban { get; set; }

    }
}
