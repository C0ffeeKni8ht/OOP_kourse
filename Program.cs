using System;
using System.IO;
using System.Linq;
using System.Text;
using TextAnalyzer;

class Program
{
    static void Main()
    {
        string filePath = "Transform.txt";
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File Transform.txt not found");
            return;
        }
        string fileContent = File.ReadAllText(filePath);
        int hashIndex = fileContent.IndexOf('#');

        if (hashIndex == -1)
        {
            Console.WriteLine("There is no # symbol in the file.");
            return;
        }

        string textBeforeHash = fileContent.Substring(0, hashIndex).Trim();

        var transform = new TextTransform();
        StringBuilder fileOutput = new StringBuilder();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1. Display original text");
            Console.WriteLine("2. Number of vowels = number of consonants in words");
            Console.WriteLine("3. Number of vowels > number of consonants in words");
            Console.WriteLine("4. Number of vowels < number of consonants in words");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect option: ");

            string? choice = Console.ReadLine();
            if (choice == null) choice = "";

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("=== Original text ===");
                    Console.WriteLine(textBeforeHash);

                    fileOutput.AppendLine("Original text:");
                    fileOutput.AppendLine(textBeforeHash);
                    fileOutput.AppendLine();
                    break;
                case "2":
                    string resultEqual = transform.ProcessWithHighlight(textBeforeHash, HighlightMode.Equal);
                    Console.WriteLine("=== Number of vowels = number of consonants ===");
                    Console.WriteLine(resultEqual);

                    fileOutput.AppendLine("Number of vowels equals number of consonants in words:");
                    fileOutput.AppendLine(resultEqual);
                    fileOutput.AppendLine();
                    break;
                case "3":
                    string resultMoreVowels = transform.ProcessWithHighlight(textBeforeHash, HighlightMode.MoreVowels);
                    Console.WriteLine("=== More vowels ===");
                    Console.WriteLine(resultMoreVowels);

                    fileOutput.AppendLine("Number of vowels is greater than number of consonants in words:");
                    fileOutput.AppendLine(resultMoreVowels);
                    fileOutput.AppendLine();
                    break;
                case "4":
                    string resultMoreConsonants = transform.ProcessWithHighlight(textBeforeHash, HighlightMode.MoreConsonants);
                    Console.WriteLine("=== More consonants ===");
                    Console.WriteLine(resultMoreConsonants);

                    fileOutput.AppendLine("Number of vowels is less than number of consonants in words:");
                    fileOutput.AppendLine(resultMoreConsonants);
                    fileOutput.AppendLine();
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("Exit");

                    string newContent = textBeforeHash + "\n#\n" + fileOutput.ToString();
                    File.WriteAllText(filePath, newContent, Encoding.UTF8);
                    continue;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
