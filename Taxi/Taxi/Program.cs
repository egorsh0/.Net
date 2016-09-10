using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int moneyPetya = Convert.ToInt32(data[0]);
            int moneyTaxi = Convert.ToInt32(data[2]);
            int maxTaxi = moneyTaxi;
            bool isAgree = false;
            while(isAgree != true)
            {
                moneyPetya = moneyPetya + Convert.ToInt32(data[1]);
                if (moneyPetya > moneyTaxi)
                {
                    moneyPetya = moneyTaxi;
                    isAgree = true;
                    break;
                }
                else
                {
                    moneyTaxi = moneyTaxi - Convert.ToInt32(data[3]);
                    if (moneyTaxi != moneyPetya)
                    {
                        if (moneyTaxi < moneyPetya)
                        {
                            moneyTaxi = moneyPetya;
                            isAgree = true;
                        }
                    }
                    else
                    {
                        moneyPetya = moneyPetya + Convert.ToInt32(data[1]);
                        isAgree = true;
                        break;
                    }
                }
            }
            Console.WriteLine(moneyPetya);
        }
    }
}
