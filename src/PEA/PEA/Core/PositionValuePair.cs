using System;
using System.Collections.Generic;

namespace Pea.Core
{ 
    public class PositionValuePair<TP> where TP: IComparable
    {
        public TP Position { get; set; }
        public int Value { get; set; }

        public PositionValuePair(TP position, int value)
        {
            Position = position;
            Value = value;
        }

        public class ComparerByPosition : IComparer<PositionValuePair<TP>> 
        {
            private ComparerByPosition() { }

            private static ComparerByPosition _instance;
            public static ComparerByPosition Instance
            {
                get
                {
                    if (_instance == null) _instance = new ComparerByPosition();
                    return _instance;
                }
            }

            public int Compare(PositionValuePair<TP> x, PositionValuePair<TP> y)
            {
                return x.Position.CompareTo(y.Position);
            }
        }
    }
}
