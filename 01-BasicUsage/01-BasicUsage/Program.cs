using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace _01_BasicUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ShowLargefilesWithLinq(@"C:\Windows");
            Console.ReadLine();

        }
        /// <summary>
        /// Shows the top 5 largest files in the given path 
        /// </summary>
        /// <param name="path"></param>
        public static void ShowLargefilesWithLinq(string path)
        {
            var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file;

            foreach(var file in query.Take(5))
            {
                Console.WriteLine($"{file.Name ,-20} : {file.Length}");
            }

        }
    }
}
