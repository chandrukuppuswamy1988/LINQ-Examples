using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromInLinq
{
    class Program
    {
        // Simple Entity Classes

        public class Customer
        {
            public String Name { get; set; }
            public String City { get; set; }
            public Order[] Orders { get; set; }
        }
        public class Order
        {
            public Int32 IdOrder { get; set; }
            public Decimal EuroAmount { get; set; }
            public String Description { get; set; }
        }

        static void Main(string[] args)
        {
            queryWithJoin();

            Console.ReadLine();

        }
        /// <summary>
        /// Simple method Illustrate to have 2 from Clauses in the LINQ statement
        /// </summary>
        static void queryWithJoin()
        {
            Customer[] customers = new Customer[] {
            new Customer {
                Name = "Paolo", City = "Brescia",
                Orders = new Order[]
                {
                    new Order { IdOrder = 1, EuroAmount = 100, Description = "Order 1" },
                    new Order { IdOrder = 2, EuroAmount = 150, Description = "Order 2" },
                    new Order { IdOrder = 3, EuroAmount = 230, Description = "Order 3" },
                }
            },
            new Customer {
                Name = "Marco", City = "Torino",
                Orders = new Order[] {
                    new Order { IdOrder = 4, EuroAmount = 320, Description = "Order 4" },
                    new Order { IdOrder = 5, EuroAmount = 170, Description = "Order 5" },
            }
            } };
            var ordersQuery =
            from c in customers
            from o in c.Orders
            select new { c.Name, o.IdOrder, o.EuroAmount };
            foreach (var item in ordersQuery)
            {
                Console.WriteLine(item);
            }
        }
    }
}
