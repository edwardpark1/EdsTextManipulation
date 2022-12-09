using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public static class RomanNumber
    {
        public const int MAX_ROMAN_NUMBER = 3999;

        private static readonly Dictionary<int, string> DictionaryRoman = new Dictionary<int, string>()
        {
            { 1, "I" },
            { 5, "V" },
            { 10, "X" },
            { 50, "L" },
            { 100, "C" },
            { 500, "D" },
            { 1000, "M" }
        };

        public static string GetRomanValue(int num)
        {
            string strNum = num.ToString();
            string result = "";

            int powerOf10Val;
            int currentNum = num;

            //Highest that Roman numerals can go is up to 3999
            //may decide to extend the range in the future
            if (currentNum > MAX_ROMAN_NUMBER)
            {
                return num.ToString();
            }

            //start from least to most significant digit and convert each placeholder
            for (int j = 1; j <= strNum.Length; j++)
            {
                powerOf10Val = currentNum % ((int)Math.Pow(10, j));
                result = ConvertToRomanDigit(powerOf10Val) + result;
                currentNum -= powerOf10Val;
            }

            return result;
        }

        private static string ConvertToRomanDigit(int num)
        {
            int runningNum = num;
            string result = "";
           
            if (runningNum.ToString().First() == '4' || runningNum.ToString().First() == '9')
            {
                int subtractingTerm = (int)Math.Pow(10, runningNum.ToString().Length - 1);

                //result will always have a value
                if (!DictionaryRoman.TryGetValue(subtractingTerm, out result))
                {
                    throw new ArgumentNullException($"{nameof(subtractingTerm)} value does not exist inside {nameof(DictionaryRoman)}");
                }

                runningNum += subtractingTerm;
            }

            //generate descending Roman literal sequence
            while (runningNum > 0)
            {
                var query = DictionaryRoman.OrderByDescending(i => i.Key).Where(i => runningNum >= i.Key).First();

                result += query.Value;
                runningNum -= query.Key;
            }

            return result;
        }
    }
}