using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DONN.LS.Entities
{
    public class TempLocations : TeminalInfo
    {
        public Guid Id { get; set; }
        public decimal Duration { get; set; }
        //for filtering data by interval of one minute
        public int? CustomInterval { get; set; }
        public DateTime CollectTime { get; set; }
        public DateTime? SendTime { get; set; }
        public double? Speed { get; set; }
        public string Region { get; set; }
        public int? Floor { get; set; }
        //public string Name { get; set; }
        public int? Direction { get; set; }
        public int? RelativePosition1 { get; set; }
        public int? RelativePosition2 { get; set; }
        public int? RelativePosition3 { get; set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string OriginalData { get; set; }
    }
}
