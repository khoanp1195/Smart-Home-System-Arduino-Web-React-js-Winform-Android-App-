using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh
{
    class Data
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string Status { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public Data() { }

        public Data(float temperature, float humidity, string status, string day, string time)
        {
            this.Temperature = temperature;
            this.Humidity = humidity;
            this.Status = status;
            this.Day = day;
            this.Time = time;
        }
    }
}
