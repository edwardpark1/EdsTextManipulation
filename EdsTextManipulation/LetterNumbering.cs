using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class LetterNumbering
    {
        private List<int> number;
        private int incrementIndex;

        //Each time this gets called, we have a running list of numbers that represent a list of integers as placevalues
        public string Letter
        {
            get
            {
                string result = "";

                foreach (var item in number)
                {
                    result = GetLetter(item) + result;
                }

                for (var i = 0; i <= incrementIndex; i++)
                {
                    if (number[i] == 26)
                    {
                        number[i] = 1;

                        if (incrementIndex == number.Count - 1)
                        {
                            number.Add(1);
                        }
                        else
                        {
                            incrementIndex++;
                            continue;
                        }
                    }
                    else
                    {
                        ++number[i];
                    }
                }
                incrementIndex = 0;
                return result;
            }
        }

        public LetterNumbering()
        {
            number = new List<int>();
            number.Add(1);
            incrementIndex = 0;
        }

        private string GetLetter(int num)
        {
            if (num == 0 || num == 26)
            {
                return "Z";
            }

            return ((char)((num % 26) + 64)).ToString();
        }
    }
}
