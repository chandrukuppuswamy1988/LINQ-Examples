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
        Customer[] customers = new Customer[] {

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

        Product[] products = new Product[] {
                        new Product {IdProduct = 1, Price = 10 },
                        new Product {IdProduct = 2, Price = 20 },
                        new Product {IdProduct = 3, Price = 30 },
                        new Product {IdProduct = 4, Price = 40 },
                        new Product {IdProduct = 5, Price = 50 },
                        new Product {IdProduct = 6, Price = 60 }
                };

        static void Main(string[] args)
        {
        }
    }
}
