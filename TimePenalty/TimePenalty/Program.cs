using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePenalty
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] timeTeams = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int ZZZ = Convert.ToInt32(timeTeams[0]);
            int QXX = Convert.ToInt32(timeTeams[1]);
            string[] time = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int sum = 0;
            for(int i = 0; i < 10; i++)
            {
                sum = sum + Convert.ToInt32(time[i]);
            }
            if (ZZZ + (sum * 20) > QXX)
                Console.WriteLine("Dirty debug :(");
            else
                Console.WriteLine("No chance.");
        }
    }
}
