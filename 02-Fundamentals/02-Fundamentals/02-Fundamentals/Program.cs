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
            public int Age { get; set; }
        }

        static Developer[] developers = new Developer[] {
                    new Developer { Name = "Paolo", Language = "C#", Age = 32 },
                    new Developer { Name = "Marco", Language = "C#", Age = 37},
                    new Developer { Name = "Frank", Language = "VB.NET", Age = 48 },
            };



        static void Main(string[] args)
        {

            GroupAnoynomousType();
            Console.ReadLine();
        }
        /// <summary>
        /// Simple Usage
        /// </summary>
        static void simpleUsage()
        {

            var developersUsingCSharp =
                                          from d in developers
                                          where d.Language == "C#"
                                          select d.Name;

            foreach (var item in developersUsingCSharp)
            {
                Console.WriteLine(item);
            }

    
        }


        /// <summary>
        /// Group In LINQ SIMPLE Example
        /// </summary>
        static void GroupSimpleUsage()
        {
            var developersGroupedByLanguage =
                                                from d in developers
                                                group d by d.Language;

            foreach (var group in developersGroupedByLanguage)
            {
                Console.WriteLine("Language: {0}", group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine("\t{0}", item.Name);
                }
            }

            

        }

        /// <summary>
        /// Group by item with anoynomous type (AgeCluster)
        /// </summary>
        static void GroupAnoynomousType()
        {
            var developersGroupedByLanguage =
                                              from d in developers
                                              group d by new { d.Language, AgeCluster = (d.Age / 10) * 10 };

            foreach (var group in developersGroupedByLanguage)
            {
                Console.WriteLine("Language: {0}", group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine("\t{0}", item.Name);
                }
            }

        }


    }
}
