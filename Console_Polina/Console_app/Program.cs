using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ввести полный адрес начального файла с данными: ");
            string extract = Console.ReadLine();
            string[] extr = extract.Split('\\');
            string start = "";
            for (int i = 0; i < extr.Length - 1; i++)
            {
                start += extr[i] + '\\';
            }
            extract = extr[extr.Length - 1];
            extract = extract.Remove(0, 6);
            string first_str = extract.Remove(extract.IndexOf(" less 0.3 chosen"));
            int first = int.Parse(first_str);
            Console.WriteLine("Ввести номер последнего файла: ");
            int last = int.Parse(Console.ReadLine());
            for(int i = first; i <= last; i++)
            {
                string t = start + "Abell_" + i + " less 0.3 chosen.txt";
                if (File.Exists(t))
                {
                    Cluster cluster = new Cluster(t);
                    cluster.Finding_clusters();
                    Console.WriteLine();
                    Console.WriteLine("Анализ файла " + t + " закончен.");
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
    }
}
