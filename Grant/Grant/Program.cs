using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grant
{
    class Program
    {
        static void Main(string[] args)
        {
            int sumMark = 0;
            int five = 0;
            int three = 0;
            int numberExams = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < numberExams; i++)
            {
                int mark = Convert.ToInt32(Console.ReadLine());
                if (mark == 3)
                {
                    three++;
                }
                if (mark == 5)
                    five++;

                sumMark += mark;
            }

            if(three > 0)
                Console.WriteLine("None");
            else
            {
                if (five == numberExams)
                    Console.WriteLine("Named");
                else
                {
                    float gpa = (float)sumMark / numberExams;
                    if (gpa >= 4.5)
                    {
                        Console.WriteLine("High");
                    }
                    else
                        Console.WriteLine("Common");
                }
            }
        }
    }
}
