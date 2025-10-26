using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextAnalyzer
{
    public enum HighlightMode
    {
        Equal,
        MoreVowels,
        MoreConsonants
    }

    abstract class Transform
    {
        public abstract string Process(string inputText);
    }

    class TextTransform : Transform
    {
        private static readonly char[] vowels =
        { 'a','e','i','o','u','y','а','е','є','и','і','ї','о','у','ю','я' };

        public override string Process(string inputText)
        {
            string textWithoutDigits = new string(inputText.Where(c => !char.IsDigit(c)).ToArray());
            string[] words = textWithoutDigits
                .Split(new[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?', ';', ':', '"', '"' },
                       StringSplitOptions.RemoveEmptyEntries);
            var equal = words.Where(HasEqual).ToList();
            var moreVowels = words.Where(HasMoreVowels).ToList();
            var moreConsonants = words.Where(HasMoreConsonants).ToList();


            StringBuilder result = new StringBuilder();
            result.AppendLine("=== Number of consonants = number of vowels ===");
            result.AppendLine(string.Join(" ", equal.Select(w => $"{{{w}}}")));
            result.AppendLine("\n=== More vowels ===");
            result.AppendLine(string.Join(" ", moreVowels.Select(w => $"{{{w}}}")));
            result.AppendLine("\n=== More consonants ===");
            result.AppendLine(string.Join(" ", moreConsonants.Select(w => $"{{{w}}}")));

            return result.ToString();
        }

        public string ProcessWithHighlight(string inputText, HighlightMode mode)
        {
            string textWithoutDigits = new string(inputText.Where(c => !char.IsDigit(c)).ToArray());
            
            var tokens = Regex.Split(textWithoutDigits, @"(\s+|[.,!?;:\""\-—])");
            
            StringBuilder result = new StringBuilder();
            
            foreach (string token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token) || !token.Any(char.IsLetter))
                {
                    result.Append(token);
                }
                else
                {
                    
                    bool shouldHighlight = mode switch
                    {
                        HighlightMode.Equal => HasEqual(token),
                        HighlightMode.MoreVowels => HasMoreVowels(token),
                        HighlightMode.MoreConsonants => HasMoreConsonants(token),
                        _ => false
                    };

                    if (shouldHighlight)
                    {
                        result.Append($"{{{token}}}");
                    }
                    else
                    {
                        result.Append(token);
                    }
                }
            }

            return result.ToString();
        }

        private static bool HasEqual(string word)
        {
            CountLetters(word, out int v, out int c);
            return v == c && v > 0;
        }

        private static bool HasMoreVowels(string word)
        {
            CountLetters(word, out int v, out int c);
            return v > c;
        }

        private static bool HasMoreConsonants(string word)
        {
            CountLetters(word, out int v, out int c);
            return c > v;
        }

        private static void CountLetters(string word, out int vowelsCount, out int consonantsCount)
        {
            vowelsCount = 0;
            consonantsCount = 0;

            foreach (char ch in word.ToLower())
            {
                if (char.IsLetter(ch))
                {
                    if (vowels.Contains(ch))
                        vowelsCount++;
                    else
                        consonantsCount++;
                }
            }
        }
    }
}
