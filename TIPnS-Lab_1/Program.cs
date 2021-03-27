using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TIPnS_Lab_1
{
    class Program
    {
        public class Probality
        {
            char c;
            int count;
        }
        public static List<Probality> probalities = new List<Probality>();
        static string fileinput(string filename)
        {
            string message = File.ReadAllText(filename);
            return message;
        }
        static IEnumerable<KeyValuePair<char, double>> getDictionary(string str)
        {
            return str.Where(x => char.IsLetter(x))
                      .GroupBy(x => char.ToLower(x))
                      .Select(x => new KeyValuePair<char, double>(x.Key, (double)x.Count() / str.Length))
                      .OrderByDescending(x => x.Value);
        }
        static double entropy(string str)
        {
            var dictionary = getDictionary(str);
            var realEntropy = -1 * dictionary.Select(x => x.Value).Aggregate((double)0, (x, y) => x + y * Math.Log(y, 2));
            return realEntropy;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя файла");
            string text = fileinput(Console.ReadLine());
            Console.WriteLine(entropy(text));
        }
    }
}
