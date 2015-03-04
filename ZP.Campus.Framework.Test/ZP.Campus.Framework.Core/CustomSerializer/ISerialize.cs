using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectExtension.Test
{
    public interface ISerialize<T>
    {
        string Serialize(T obj);
        T Deserialize(string stream);
    }
}
