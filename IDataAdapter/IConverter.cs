using DONN.LS.Entities;
using System;

namespace DONN.LS.IDataAdapter
{
    public interface IConverter
    {
        TempLocations ToTempLoc(object original);

        //TargetEntity FromTempLoc<TargetEntity>(TempLocations original) where TargetEntity : new();

        //object FromTempLoc(Type tegetType, TempLocations original);
    }
}
