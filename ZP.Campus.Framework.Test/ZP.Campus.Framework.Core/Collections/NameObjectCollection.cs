using System;
using System.Collections;
using System.Collections.Specialized;

namespace ZP.Campus.Framework.Core
{
    public class NameObjectCollection : NameObjectCollectionBase
    {
        public NameObjectCollection()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        public NameObjectCollection(IEqualityComparer equalityComparer)
            : base(equalityComparer)
        {
        }
    }
}