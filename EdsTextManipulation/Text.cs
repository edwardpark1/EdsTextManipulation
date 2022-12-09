using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdsTextManipulation
{
    public abstract class Text
    {
		private string newLine = Environment.NewLine;
		private string val = "";
		protected static readonly IEnumerable<char> vowels = new List<char>() { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
		private Dictionary<char, int> charOccurences = new Dictionary<char, int>();

		private static Dictionary<char, string> specialKeyReference = new Dictionary<char, string>()
		{
			{ ' ', "Space"},
			{ '\r', "Carriage Return"},
			{ '\n', "New Line"},
			{ '\t', "Tab"},
			{ '\0', "Null"},
            { '\v', "Vertical tab"},
            { '\f', "Form Feed"},
            { '\b', "Backspace"},
            { '\a', "Alert"}


        };

		public string Value
		{
			get { return val; }
			protected set
            {
				if(value.Length > TextEditForm.MAX_TEXT_LENGTH)
                {
					throw new ArgumentOutOfRangeException(nameof(value), $"greater than {string.Format("{0:#,###0.#}", TextEditForm.MAX_TEXT_LENGTH)}", 
						$"Operation Exceeds the allowed max text length");
                }

                val = value;
                charOccurences = GetAllCharOccurences(value);
            }
        }

		public int VowelCount
		{
			get => Value.Count(v => IsVowel(v));
		}

		public int LetterCount
		{
			get => Value.Count(v => IsAlpha(v));
		}

		public int ConsonantCount
		{
			get => Value.Count(v => IsConsonant(v));
		}

		public int WordCount
		{
			get => Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
		}

		//Palindrome: a word, sentence, or long written work that reads the same backwards
		public bool IsPalindrome
		{
			get => !Value.Zip(Value.Reverse(), (orig, reverse) => new { Original = char.ToLower(orig), Reversed = char.ToLower(reverse) })
					.Where(s => s.Original != s.Reversed).Any();
		}

		//Pangram: a phrase or sentence containing all 26 letters of the alphabet
		public bool IsPangram
		{
			get => Value.Where(c => IsAlpha(c)).Select(c => char.ToLower(c)).Distinct().Count() == 26;
		}

		//Isogram: a word, phrase, or sentence that has no letter used more than once
		public bool IsIsogram
		{
			get => !Value.Where(c => IsAlpha(c)).Select(c => char.ToLower(c))
				.GroupBy(c => c).Select(c => new { Char = c.Key, Count = c.Count() })
				.Where(c => c.Count > 1).Any();
		}

		public Text(string value)
        {
			Value = value;
        }

		public static bool IsAlpha(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');

		public static bool IsVowel(char c) => vowels.Where(v => v == c).Any();

		public static bool IsConsonant(char c) => IsAlpha(c) && !IsVowel(c);

		public static bool IsNumeric(char c) => c >= '0' && c <= '9';

		public Dictionary<char, int> GetAllCharOccurences(string input) => input.GroupBy(c => c)
			.Select(c => new { Char = c.Key, Count = c.Count() }).ToDictionary(c => c.Char, c => c.Count);

		public string DisplayStatistics(string header) => $"{header} Text Statistics{newLine}{newLine}" +
														$"Character count: {Value.Length}{newLine}" +
														$"Word count: {WordCount}{newLine}" +
														$"Letter count: {LetterCount}{newLine}" +
														$"Vowel count: {VowelCount}{newLine}" +
														$"Consonant count: {ConsonantCount}{newLine}" +
														$"Palindrome?: {IsPalindrome}{newLine}" +
														$"Pangram?: {IsPangram}{newLine}" +
														$"Isogram?: {IsIsogram}{newLine}{newLine}" +
														$"{GetCharFreqs()}";

		public string GetCharFreqs()
		{
			StringBuilder result = new StringBuilder($"Character occurences:{newLine}");

			var query = GetAllCharOccurences(Value).OrderBy(c => c.Key);

			foreach (var element in query)
			{
				result.Append($"[{GetSpecialName(element.Key)}] = {element.Value}{newLine}");
			}

			return result.ToString();
		}

		public static string GetSpecialName(char c)
		{
			string val;
			if (specialKeyReference.TryGetValue(c, out val))
			{
				return val;
			}
			else
			{
				return c.ToString();
			}
		}
	}
}
