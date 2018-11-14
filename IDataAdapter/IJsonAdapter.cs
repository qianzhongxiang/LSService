using System;
using System.IO;

namespace DONN.LS.IDataAdapter
{
    public interface IJsonAdapter : IAdapter
    {
        T[] Deserialize<T>(string s);
        object[] Deserialize(Type targetEntity, string s);
        T[] Deserialize<T>(System.IO.MemoryStream s);
        object[] Deserialize(Type targetEntity, MemoryStream s);

        string Serialize(object obj);
        TargetEntity[] Deserialize<TargetEntity, MiddleEntity>(string s) where TargetEntity : new() where MiddleEntity : new();
        object[] Deserialize(Type middleEntity, Type targetEntity, string s);


        TargetEntity[] Deserialize<TargetEntity, MiddleEntity>(MemoryStream s) where TargetEntity : new() where MiddleEntity : new();

        object[] Deserialize(Type middleEntity, Type targetEntity, MemoryStream s);

        string Serialize<MiddleEntity>(object obj) where MiddleEntity : new();

    }
}
