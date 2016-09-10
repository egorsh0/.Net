﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneStepToHappiness
{
    class Program
    {
        static void Main(string[] args)
        {
            string _number = Console.ReadLine();
            int fullNumber = Convert.ToInt32(_number);
            int low = fullNumber - 1;
            int hight = fullNumber + 1;

            string sLow = Convert.ToString(low);
            int lLength = sLow.Length;
            if (lLength < 6)
                for (int i = 0; i < 6 - lLength; i++)
                    sLow = "0" + sLow;
            if (low < 0)
            {
                Console.WriteLine("No");
                return;
            }

            string sHight = Convert.ToString(hight);
            int lHight = sHight.Length;
            if (lHight < 6)
                for (int i = 0; i < 6 - lHight; i++)
                    sHight = "0" + sHight;

            char[] _low = sLow.ToCharArray();
            char[] _hight = sHight.ToCharArray();

            int lFirst = Convert.ToInt32(_low[0]) + Convert.ToInt32(_low[1]) + Convert.ToInt32(_low[2]);
            int lSecond = Convert.ToInt32(_low[3]) + Convert.ToInt32(_low[4]) + Convert.ToInt32(_low[5]);
       
            int hFirst = Convert.ToInt32(_hight[0]) + Convert.ToInt32(_hight[1]) + Convert.ToInt32(_hight[2]);
            int hSecond = Convert.ToInt32(_hight[3]) + Convert.ToInt32(_hight[4]) + Convert.ToInt32(_hight[5]);

            if ((lFirst == lSecond) || (hFirst == hSecond))
                Console.WriteLine("Yes");
            else
                Console.WriteLine("No");  
        }
    }
}
