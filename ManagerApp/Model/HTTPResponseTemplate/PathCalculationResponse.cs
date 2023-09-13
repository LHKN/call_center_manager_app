using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Model.HTTPResponseTemplate
{
    public class PathCalculationResponse
    {
        public List<PathsCost> PathsCost { get; set; }
    }

    public class PathsCost
    {
        public string Vehicle { get; set; }
        public double Duration { get; set; }
        public double Distance { get; set; }
        public double Cost { get; set; }

    }
}
