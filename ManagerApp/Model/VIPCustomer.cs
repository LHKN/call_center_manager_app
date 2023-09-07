using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    public class VIPCustomer : Customer
    {
        public VIPCustomer() { this.Type = Type.VIP; }      
    }
}
