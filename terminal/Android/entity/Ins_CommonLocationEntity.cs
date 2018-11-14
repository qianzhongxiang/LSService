using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.LS.Terminal.Android.Entity
{
    public enum LocationState
    {
    }
    public enum VehicleState
    {

    }
    public class Ins_CommonLocationEntity : Ins_BaseEntity
    {
        public DateTime Date { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public ushort Speech { get; set; }
        public ushort Direction { get; set; }
        //public LocationState LocState { get; set; }

        //public VehicleState VehicleState { get; set; }

    }
}
