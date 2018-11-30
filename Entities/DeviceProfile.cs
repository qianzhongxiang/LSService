using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DONN.LS.Entities
{
    public class DeviceProfile
    {
        public string Uid { get; set; }
        public string Type { get; set; }

/// <summary>
/// desolate
/// </summary>
/// <value></value>
        public DeviceStatus DevState { get; set; }

        public Guid IdLoactionData { get; set; }

        public int Interval { get; set; }
        /// <summary>
        /// 系统自动生成，不需要手动填写 
        /// </summary>
        /// <value></value>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 系统自动生成，不需要手动填写 format:yyyMMdd
        /// </summary>
        public int Day { get; set; }

        [Timestamp]
        public byte[] TS { get; set; }

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
