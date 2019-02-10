using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            Cluster cluster = new Cluster(s);
            cluster.Finding_clusters();
            Console.WriteLine("Анализ файла ", s, " закончен.");
        }
    }
}
