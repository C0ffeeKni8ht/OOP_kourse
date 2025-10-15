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

        Console.WriteLine("=== Current file text ===");
        Console.WriteLine(textBeforeHash);
        Console.WriteLine();

        var transform = new TextTransform();

        string result = transform.Process(textBeforeHash);

        Console.WriteLine("=== Analys result ===");
        Console.WriteLine(result);

        string newContent = textBeforeHash + "\n#\n" + result;

        File.WriteAllText(filePath, newContent, Encoding.UTF8);
    }
}
