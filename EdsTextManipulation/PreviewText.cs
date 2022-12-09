using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public class PreviewText : Text
    {
        public delegate string ExecuteInsertText(string original, string insText);

        public PreviewText(string input)
            : base(input) { }

        public void RenderReverse()
        {
            var query = Value.Reverse();
            Value = string.Join("", query);

            //fix the swapping between the new line characters
            if(Value.Contains("\n\r"))
            {
                RenderReplaceText("\n\r", "\r\n", false);
            }

            //for future feature for expanding reverse abilities
            //var query = Value.Split(' ').Select(s => new { Word = Reverse(s) }).Select(c => c.Word);
            //Value = string.Join(' ', query);
        }

        public static string Reverse(string s)
        {
            char[] cArray = s.ToCharArray();
            Array.Reverse(cArray);
            return new string(cArray);
        }

        public void RenderToLower()
        {
            Value = Value.ToLower();
        }

        public void RenderToUpper()
        {
            Value = Value.ToUpper();
        }

        public void RenderPascalCase()
        {
            var query = Value.Split(' ').Select(s => new { Word = CapitalizeWord(s) }).Select(c => c.Word);
            Value = string.Join(' '.ToString(), query);

            query = Value.Split("\r\n").Select(s => new { Word = CapitalizeWord(s) }).Select(c => c.Word);
            Value = string.Join("\r\n".ToString(), query);
        }

        public string InsertTextBetweenSeparator(string input, string[] separator, string strInsert, ExecuteInsertText modifyText)
        {
            var query = input.Split(separator, StringSplitOptions.None)
                             .Select(s => new { Word = modifyText(s, strInsert) })
                             .Select(s => s.Word);

            return String.Join(separator[0], query);
        }

        public void PrefixSuffixText(string strInsert, ExecuteInsertText modifyText)
        {
            string[] newLine = { "\r\n" };
            string[] space = { " " };

            var query = Value.Split(newLine, StringSplitOptions.None)
                             .Select(s => new { Line = InsertTextBetweenSeparator(s, space, strInsert, modifyText) })
                             .Select(z => z.Line);

            Value = string.Join(newLine[0], query);
        }
        
        public string PrefixText(string original, string insText) => insText + original;
        public string SuffixText(string original, string insText) => original + insText;
        public string InsertTextBothSides(string original, string insText) => (original != "") ? insText + original + insText : original;

        public void TrimLeft(char c) => Value = Value.TrimStart(c);
        public void TrimRight(char c) => Value = Value.TrimEnd(c);
        public void TrimBothSides(char c) => Value = Value.Trim(c);

        public void RemoveEmptyLines()
        {
            string[] separator = { "\r\n" };
            var query = Value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            Value = string.Join(separator[0], query);
        }

        public void RenderReplaceText(string oldText, string newText, bool ignoreCasing)
        {
            if (oldText != newText)
            {
                Value = Value.Replace(oldText, newText,
                    (ignoreCasing) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }
        }

        public void LineNumberingNumbers(bool startAtZero, string numSeparator = " ")
        {
            int i = (startAtZero) ? 0 : 1;
            IEnumerable<string> query;

            query = Value.Split(Environment.NewLine).Select(s => $"{i++}{numSeparator}{s}");
            Value = string.Join(Environment.NewLine, query);
        }

        public void LineNumberingRoman(string numSeparator = " ", bool isLowerCase = false)
        {
            int i = 1;
            IEnumerable<string> query;

            query = Value.Split(Environment.NewLine).Select(s => $"{((isLowerCase) ? RomanNumber.GetRomanValue(i++).ToLower() : RomanNumber.GetRomanValue(i++)) }{numSeparator}{s}");
            Value = string.Join(Environment.NewLine, query);
        }
        
        public void LineNumberingLetter(string numSeparator = " ", bool isLowerCase = false)
        {
            IEnumerable<string> query;

            LetterNumbering letterNum = new LetterNumbering();

            query = Value.Split(Environment.NewLine).Select(s => $"{ ((isLowerCase) ? letterNum.Letter.ToLower() : letterNum.Letter) }{numSeparator}{s}");
            Value = string.Join(Environment.NewLine, query);
        }

        private string CapitalizeWord(string input = "") => (input.Length > 0) ? char.ToUpper(input[0]) + input.Substring(1, input.Length - 1) : "";
    }
}