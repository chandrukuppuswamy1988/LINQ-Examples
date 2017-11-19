using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Fundamentals
{
    class Program
    {
        public class Developer
        {
            public string Name;
            public string Language;
            public int Age;
        }

        static void Main(string[] args)
        {
            Developer[] developers = new Developer[] {
                                        new Developer {Name = "Paolo", Language = "C#"},
                                        new Developer {Name = "Marco", Language = "C#"},
                                        new Developer {Name = "Frank", Language = "VB.NET"}
            };
            var developersUsingCSharp =
                                          from d in developers
                                          where d.Language == "C#"
                                          select d.Name;
            foreach (var item in developersUsingCSharp)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();


        }
    }
}
