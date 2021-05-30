using System;
using System.Collections.Generic;

namespace GradeBook
{
    public class Statistics
    {
        public double Average
        {
            get
            {
                return Sum / Count;
            }
        }
        public double High;
        public double Low;
        public char Letter
        {
            get
            {
                switch (Average)
                {
                    case var a when a >= 90:
                        return 'A';

                    case var a when a >= 80:
                        return 'B';

                    case var a when a >= 70:
                        return 'C';

                    case var a when a >= 60:
                        return 'D';

                    default:
                        return 'F';
                }
            }
        }
        public double Sum;
        public int Count;

        public void Add(double number)
        {
            Sum += number;
            Count += 1;
            High = Math.Max(High, number);
            Low = Math.Min(Low, number);
        }

        public Statistics()
        {
            High = double.MinValue;
            Low = double.MaxValue;
            Sum = 0.0;
            Count = 0;
        }
    }
}