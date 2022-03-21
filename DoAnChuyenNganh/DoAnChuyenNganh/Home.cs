using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh
{
    class Home
    {
        public string Lamp { get; set; }
        public string Fan { get; set; }
        public string AirConditioner { get; set; }

        public Home()
        {
        }

        public Home(string lamp, string fan, string airConditioner)
        {
            Lamp = lamp;
            Fan = fan;
            AirConditioner = airConditioner;
        }
    }
}
