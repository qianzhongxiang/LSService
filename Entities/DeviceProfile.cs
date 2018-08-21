using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Entities
{
    public class DeviceProfile
    {
        public string Uid { get; set; }
        public string Type { get; set; }

        public DeviceStatus DevState { get; set; }

        public Guid IdLoactionData { get; set; }

        public int Interval { get; set; }
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// format:yyyMMdd
        /// </summary>
        public int Day { get; set; }



        [NotMapped]
        private TempLocations location;
        [NotMapped]

        public TempLocations LocationItem
        {
            get => location; set
            {
                location = value; IdLoactionData = location.Id; UpdateTime = location.CollectTime;
                //Day = int.Parse(UpdateTime.ToString("yyyyMMdd"));
            }
        }

        public override string ToString()
        {
            return $"{Uid}_{Type}";
        }

    }
}
