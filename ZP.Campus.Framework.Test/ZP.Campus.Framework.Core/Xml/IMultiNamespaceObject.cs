using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZP.Campus.Framework.Core.Xml
{
    /// <summary>
    /// 多命名空间对象接口
    /// </summary>
    public interface IMultiNamespaceObject
    {
        /// <summary>
        /// 命名空间别名（key：别名，value：命名空间）
        /// 请标识[XmlIgnore]以防止序列化此属性
        /// </summary>
        Dictionary<string, string> NamespaceAlias
        {
            get;
        }
    }
}
