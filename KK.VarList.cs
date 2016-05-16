using System.Collections.Generic;

namespace KK
{
    class VarList : SortedDictionary<string, string>
    {
        public new string this[string name]
        {
            get
            {
                try
                {
                    return base[name];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                base[name] = value;
            }
        }
    }
}