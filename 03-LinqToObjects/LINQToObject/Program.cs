using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToObject
{
    #region INTIAL CLASSES

    public enum Countries
    {
        USA,
        Italy,
    }

    public class Customer
    {
        public string Name;
        public string City;
        public Countries Country;
        public Order[] Orders;
        public override string ToString()
        {
            return String.Format("Name: {0} – City: {1} – Country: {2}",
            this.Name, this.City, this.Country);
        }
    }

    public class Order
    {
        public int IdOrder;
        public int Quantity;
        public bool Shipped;
        public string Month;
        public int IdProduct;
        public override string ToString()
        {
            return String.Format("IdOrder: {0} – IdProduct: {1} – " +
            "Quantity: {2} – Shipped: {3} – " +
            "Month: {4}", this.IdOrder, this.IdProduct,
            this.Quantity, this.Shipped, this.Month);
        }
    }

    public class Product
    {
        public int IdProduct;
        public decimal Price;
        public override string ToString()
        {
            return String.Format("IdProduct: {0} – Price: {1}", this.IdProduct,
            this.Price);
        }
    }

    #endregion

    class Program
    {

        #region DATA SEED

        static Customer[] customers = new Customer[] {

            new Customer {Name = "Paolo", City = "Brescia",
                Country = Countries.Italy, Orders = new Order[] {
                    new Order { IdOrder = 1, Quantity = 3, IdProduct = 1, Shipped = false, Month = "January"},
                    new Order { IdOrder = 2, Quantity = 5, IdProduct = 2 ,Shipped = true, Month = "May"}}
            },
            new Customer {Name = "Marco", City = "Torino",
                Country = Countries.Italy, Orders = new Order[] {
                    new Order { IdOrder = 3, Quantity = 10, IdProduct = 1 ,Shipped = false, Month = "July"},
                    new Order { IdOrder = 4, Quantity = 20, IdProduct = 3 ,Shipped = true, Month = "December"}}},
            new Customer {Name = "James", City = "Dallas",
                Country = Countries.USA, Orders = new Order[] {
                    new Order { IdOrder = 5, Quantity = 20, IdProduct = 3 ,Shipped = true, Month = "December"}}},
            new Customer {Name = "Frank", City = "Seattle",
                Country = Countries.USA, Orders = new Order[] {
                    new Order { IdOrder = 6, Quantity = 20, IdProduct = 5 ,Shipped = false, Month = "July"}}}};

        static Product[] products = new Product[] {
                        new Product {IdProduct = 1, Price = 10 },
                        new Product {IdProduct = 2, Price = 20 },
                        new Product {IdProduct = 3, Price = 30 },
                        new Product {IdProduct = 4, Price = 40 },
                        new Product {IdProduct = 5, Price = 50 },
                        new Product {IdProduct = 6, Price = 60 }
                };

        #endregion

        static void Main(string[] args)
        {
            GroupbywithCustomSelectorAndElementSelector();
            Console.ReadLine();

        }

        #region WHERE WITH INDEX

        static void QueryWithRestrictionAndIndexBasedFilter()
        {
            var expr = customers
                       .Where((c, index) => (c.Country == Countries.Italy && index >= 1))
                       .Select(c => c.Name);
            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }
        }

        static void QueryWithPagingRestriction()
        {
            int start = 1;
            int end = 3;
            var expr = customers
                        .Where((c, index) => ((index >= start) && (index < end)))
                        .Select(c => c.Name);

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }

        }

        #endregion

        #region PROJECTION OPERATORS

        static void SelectInScenarioOfSelectMany()
        {
            var orders = customers
                        .Where(c => c.Country == Countries.Italy)
                        .Select(c => c.Orders);

            foreach (var item in orders)
            {
                Console.WriteLine(item);
            }

            // Printes LINQToObject.Order[] twice which is not Indented 
            // so we replace the select with select many shown in SelectMany functions

        }

        /// <summary>
        /// The below functions has shows the flatened list of the Orders
        /// </summary>
        static void SelectMany()
        {
            var ordersList = customers
                             .Where(c => c.Country == Countries.Italy)
                             .SelectMany(c => c.Orders);

            foreach (var item in ordersList)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Select Many Function can be implemented alternative way as mentioned in the below function
        /// </summary>
        static void AlternativeForSelectMany()
        {
            var orders =
                from c in customers
                where c.Country == Countries.Italy
                from o in c.Orders
                select o;

            foreach (var item in orders)
            {
                Console.WriteLine(item);
            }
        }

        #endregion

        #region ORDER EXAMPLE AND REVERSE OPERATOR USAGE

        static void QueryWithOrderByAndThenByDesc()
        {
            var expr =
                       from c in customers
                       where c.Country == Countries.Italy
                       orderby c.Name descending, c.City
                       select new { c.Name, c.City };

            foreach (var item in expr)
            {
                Console.WriteLine("Name: {0} City: {1}", item.Name, item.City);
            }
        }

        /// <summary>
        /// here we created a MonthComparer class to customise the ordering of the list
        /// </summary>
        static void OrderByWithCustomComparer()
        {
            var orders = customers.SelectMany(c => c.Orders).OrderBy(o => o.Month, new MonthComparer());
            foreach (var item in orders)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// It is usage of simple reverse operator which reverses the entire order
        /// </summary>
        static void ReverseOperator()
        {
            var expr = customers
                        .Where(c => c.Country == Countries.Italy)
                        .OrderByDescending(c => c.Name)
                        .ThenBy(c => c.City)
                        .Select(c => new { c.Name, c.City })
                        .Reverse();

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }
        }

        #endregion

        #region GROUPING OPERATORS

        public static void SimpleGroupingWithKey()
        {
            var expr = customers.GroupBy(c => c.Country);
            foreach (IGrouping<Countries, Customer> customerGroup in expr)
            {
                Console.WriteLine("Country: {0}", customerGroup.Key);
                foreach (var item in customerGroup)
                {
                    Console.WriteLine("\t{0}", item);
                }
            }
        }

        public static void SimpleGroupingQueryExpression()
        {
            var expr = from c in customers
                       group c by c.Country;

            foreach (IGrouping<Countries, Customer> customerGroup in expr)
            {
                Console.WriteLine("Country: {0}", customerGroup.Key);
                foreach (var item in customerGroup)
                {
                    Console.WriteLine("\t{0}", item);
                }
            }

        }

        public static void GroupByCustomerNameByCountry()
        {
            var expr = customers
                .GroupBy(c => c.Country,
                (k, c) => new { Key = k, Count = c.Count() });

            foreach (var group in expr)
            {
                Console.WriteLine("Key: {0} - Count: {1}", group.Key, group.Count);
            }

        }

        public static void GroupbywithCustomSelectorAndElementSelector()
        {
            var expr = customers
                .GroupBy(
                c => c.Country, // keySelector
                c => new { OrdersCount = c.Orders.Count() }, // elementSelector
                (key, elements) => new
                { // resultSelector
                    Key = key,
                    Count = elements.Count(),
                    OrdersCount = elements.Sum(item => item.OrdersCount),
                    element = elements
                }
                );

            foreach (var group in expr)
            {
                Console.WriteLine("Key: {0} - Count: {1} - Orders Count: {2}",
                group.Key, group.Count, group.OrdersCount);
            }
        }



        #endregion




    }
}
