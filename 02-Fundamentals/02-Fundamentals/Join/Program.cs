using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Join
{
    // Simple Entity Classes

    #region ENTITY CLASS

    public class Category
    {
        public Int32 IdCategory { get; set; }
        public String Name { get; set; }
        public string Year { get; set; }
    }

    public class Product
    {
        public String IdProduct { get; set; }
        public Int32 IdCategory { get; set; }
        public String Description { get; set; }
        public string Year { get; set; }
    }

    #endregion

    class Program
    {

        #region DATA SEED

        static Category[] categories = new Category[] {
           new Category { IdCategory = 1, Name = "Pasta", Year="2007"},
           new Category { IdCategory = 2, Name = "Beverages", Year="2007"},
           new Category { IdCategory = 3, Name = "Other food", Year="2017"},
        };

        static Product[] products = new Product[] {
            new Product { IdProduct = "PASTA01", IdCategory = 1, Description = "Tortellini", Year="2007" },
            new Product { IdProduct = "PASTA02", IdCategory = 1, Description = "Spaghetti", Year="2007" },
            new Product { IdProduct = "PASTA03", IdCategory = 1, Description = "Fusilli", Year="2007" },
            new Product { IdProduct = "BEV01", IdCategory = 2, Description = "Water", Year="2015" },
            new Product { IdProduct = "BEV02", IdCategory = 2, Description = "Orange Juice", Year="2017" },
        };

        #endregion

        static void Main(string[] args)
        {

            LetKeywordUsage();
            Console.ReadLine();

        }

        /// <summary>
        /// simple inner join 
        /// </summary>
        static void QueryWithJoin()
        {
            var categoriesAndProducts =
                                        from c in categories
                                        join p in products on c.IdCategory equals p.IdCategory
                                        select new
                                        {
                                            c.IdCategory,
                                            CategoryName = c.Name,
                                            Product = p.Description
                                        };

            foreach (var item in categoriesAndProducts)
            {
                Console.WriteLine(item);
            }

        }

        /// <summary>
        /// Join using group 
        /// </summary>
        static void GroupJoinExample()
        {
            var categoriesAndProducts =
                                        from c in categories
                                        join p in products on c.IdCategory equals p.IdCategory
                                        into productsByCategory
                                        select new
                                        {
                                            c.IdCategory,
                                            CategoryName = c.Name,
                                            Products = productsByCategory
                                        };

            foreach (var category in categoriesAndProducts)
            {
                Console.WriteLine("{0} - {1}", category.IdCategory, category.CategoryName);
                foreach (var product in category.Products)
                {
                    Console.WriteLine("\t{0}", product.Description);
                }
            }

        }

        /// <summary>
        /// simple Left join Query using DefaultIfEmpty Extension
        /// </summary>
        static void QueryWithLeftOuterJoin()
        {
            var categoriesAndProducts =
            from c in categories
            join p in products on c.IdCategory equals p.IdCategory
            into productsByCategory
            from pc in productsByCategory.DefaultIfEmpty(
            new Product
            {
                IdProduct = String.Empty,
                Description = String.Empty,
                IdCategory = 0
            })
            select new { c.IdCategory, CategoryName = c.Name, Product = pc.Description };
            foreach (var item in categoriesAndProducts)
            {
                Console.WriteLine(item);
            }

        }
        
        /// <summary>
        /// Join with Composite Key Mapping
        /// </summary>
        static void QueryWithCompositeKeyMapping()
        {
            var categoriesAndProducts =
                                       from c in categories
                                       join p in products
                                       on new { c.IdCategory, c.Year } equals new { p.IdCategory, p.Year }
                                       into productsByCategory
                                       select new { c.IdCategory, CategoryName = c.Name };

            foreach (var item in categoriesAndProducts)
            {
                Console.WriteLine(item);
            }

        }

        /// <summary>
        /// Let clause usages -- 
        /// The let clause allows you to store the result of a subexpression in a variable that can be used
        /// somewhere else in the query
        /// </summary>
        static void LetKeywordUsage()
        {
            var categoriesByProductsNumberQuery =
                                                    from c in categories
                                                    join p in products on c.IdCategory equals p.IdCategory
                                                    into productsByCategory
                                                    let ProductsCount = productsByCategory.Count()
                                                    orderby ProductsCount
                                                    select new { c.IdCategory, ProductsCount };

            foreach (var item in categoriesByProductsNumberQuery)
            {
                Console.WriteLine(item);
            }
        }

    }
}


