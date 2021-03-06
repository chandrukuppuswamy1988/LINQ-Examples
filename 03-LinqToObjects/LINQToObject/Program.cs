﻿using System;
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

        public override bool Equals(object obj)
        {
            if (!(obj is Product))
                return false;
            else
            {
                Product p = (Product)obj;
                return (p.IdProduct == this.IdProduct &&
                p.Price == this.Price);
            }
        }

        public override int GetHashCode()
        {
            return String.Format("{0}|{1}", this.IdProduct, this.Price)
            .GetHashCode();
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
            TakeWhile();
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

        #region JOIN OPERATORS

        public static void JoinExample()
        {
            var expr = customers
                .SelectMany(c => c.Orders)
                .Join(products,
                    o => o.IdProduct,
                    p => p.IdProduct,
                (o, p) => new
                {
                    o.Month,
                    o.Shipped,
                    p.IdProduct,
                    p.Price
                }
                );

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }


        }

        public static void JoinWithQueryExpression()
        {
            var expr = from c in customers
                       from o in c.Orders
                       join p in products on o.IdProduct equals p.IdProduct
                       select new { o.Month, o.Shipped, p.IdProduct, p.Price };

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }

        }

        #region Group Join

        public static void GroupJoinExample()
        {
            var expr = products
                .GroupJoin(
                customers.SelectMany(c => c.Orders),
                p => p.IdProduct,
                o => o.IdProduct,
                (p, orders) => new { p.IdProduct, Orders = orders }
                );
            foreach (var item in expr)
            {
                Console.WriteLine("Product: {0}", item.IdProduct);
                foreach (var order in item.Orders)
                {
                    Console.WriteLine("\t{0}", order);
                }
            }
        }

        #endregion

        public static void QueryExpressionWithJoinIntoClause()
        {
            var customersOrders =
                from c in customers
                from o in c.Orders
                select o;

            var expr =
            from p in products
            join o in customersOrders
            on p.IdProduct equals o.IdProduct
            into orders
            select new { p.IdProduct, Orders = orders };

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }

        }

        /// <summary>
        /// + as previous method just to show the sub query 
        /// concept can be applied to the query
        /// </summary>
        public static void QueryExpressionWithJoinIntoClauseCompactVersion()
        {
            var expr =
                from p in products
                join o in (
                from c in customers
                from o in c.Orders
                select o) on p.IdProduct equals o.IdProduct
                into orders
                select new { p.IdProduct, Orders = orders };


            foreach (var item in expr)
            {
                Console.WriteLine("Product Id {0}", item.IdProduct);
                foreach (var orders in item.Orders)
                {
                    Console.WriteLine(orders);
                }
            }

        }

        #endregion

        #region SET OPERATORS

        public static void DistinctOperator()
        {
            var expr =
                (
                    from c in customers
                    from o in c.Orders
                    join p in products on o.IdProduct equals p.IdProduct
                    select p
                ).Distinct();

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public static void UnionOperator()
        {
            Product[] productSetOne = {
                new Product {IdProduct = 46, Price = 1000 },
                new Product {IdProduct = 27, Price = 2000 },
                new Product {IdProduct = 14, Price = 500 } };
            Product[] productSetTwo = {
                new Product {IdProduct = 11, Price = 350 },
                new Product {IdProduct = 46, Price = 1000 } };

            var productsUnion = productSetOne.Union(productSetTwo);

            foreach (var item in productsUnion)
            {
                Console.WriteLine(item);
            }

            /// Union operator uses the equals and gehashcode operator so we need to ovveride these methods to 
            /// get the required result.






        }


        #endregion

        #region AGGREGATE OPERATORS

        public static void CountExample()
        {
            var expr =
                from c in customers
                select new { c.Name, c.Country, OrdersCount = c.Orders.Count() };

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }

        }

        public static void SumExample()
        {
            var customersOrders =
                from c in customers
                from o in c.Orders
                join p in products on o.IdProduct equals p.IdProduct
                select new { c.Name, OrderAmount = o.Quantity * p.Price };

            var expr = 
                from c in customers
                join o in customersOrders
                on c.Name equals o.Name
                into customersWithOrders
                select new
                {
                    c.Name,
                    TotalAmount = customersWithOrders.Sum(o => o.OrderAmount)
                };

            foreach (var item in expr)
            {
                Console.WriteLine(item);
            }

        }

        public static void MinMaxSimple()
        {
            var expr = 
                (from c in customers
                 from o in c.Orders
                 select o.Quantity
                 ).Min();

            Console.WriteLine(expr);
        }

        public static void MinMaxComplex()
        {
            var expr = 
                (from c in customers
                 from o in c.Orders
                 select new { o.IdProduct, o.Quantity }
                 ).Min(o => o.Quantity);

            Console.WriteLine(expr);

        }

        #endregion

        #region QUANTIFIER OPERATORS

        public static void AnyOperator()
        {
            bool result = 
                (from c in customers
                 from o in c.Orders
                 select o)
                 .Any(o => o.IdProduct == 1); 

            result = Enumerable.Empty<Order>().Any(); // Makes the boolean false since empty operator returns list of zero items

        }

        public static void AllOperator()
        {
            bool result = 
                (from c in customers
                 from o in c.Orders
                 select o)
                 .All(o => o.Quantity > 0); // returns true if all the items of the lists contains quantity greater than 0

            result = Enumerable.Empty<Order>().All(o => o.Quantity > 0); // returns true since no items found the predicate never called

        }

        public static void ContainsOperator()
        {
            var orderOfProductOne = new Order
            {
                IdOrder = 1,
                Quantity = 3,
                IdProduct =1,
                Shipped = false,
                Month = "January"
            };
            bool result = customers[0].Orders.Contains(orderOfProductOne); 
            // the above will fail since we have not override the equals and gethashcode methods for order class



        }

        #endregion

        #region PARTITION OPERATORS

        public static void TakeWhile()
        {
            // globalAmount is the total amount for all the orders

            var globalAmount = GetglobalAmount();
            var limitAmount = globalAmount * 0.8m;
            var aggregated = 0m;
            var topCustomers =
            (from c in customers
             join o in (
             from c in customers
            from o in c.Orders
            join p in products
            on o.IdProduct equals p.IdProduct
            select new { c.Name, OrderAmount = o.Quantity * p.Price }
            ) on c.Name equals o.Name
 into customersWithOrders
             let TotalAmount = customersWithOrders.Sum(o => o.OrderAmount)
             orderby TotalAmount descending
             select new { c.Name, TotalAmount }
            )
            .TakeWhile(X => {
                bool result = aggregated < limitAmount;
                aggregated += X.TotalAmount;
                return result;
            });

            foreach (var item in topCustomers)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public static decimal GetglobalAmount()
        {
            var customersOrders =
                                   from c in customers
                                   from o in c.Orders
                                   join p in products on o.IdProduct equals p.IdProduct
                                   select new { c.Name, OrderAmount = o.Quantity * p.Price };

            var expr =
                from c in customers
                join o in customersOrders
                on c.Name equals o.Name
                into customersWithOrders
                select new
                {
                    c.Name,
                    TotalAmount = customersWithOrders.Sum(o => o.OrderAmount)
                };

            var totalAmount = expr.Sum(p => p.TotalAmount);

            return totalAmount;
        }

        #endregion

    }
}
