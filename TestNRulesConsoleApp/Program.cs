using NRules;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNRulesConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load rules
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(PreferredCustomerDiscountRule).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

            //Load domain model
            var customer = new Customer("John Doe") { IsPreferred = true };
            var order1 = new Order(123456, customer, 2, 25.0);
            var order2 = new Order(123457, customer, 1, 100.0);
            var order3 = new Order(123458, customer, 1, 100.0, false);

            //Insert facts into rules engine's memory
            session.Insert(customer);
            session.Insert(order1);
            session.Insert(order2);
            session.Insert(order3);

            //Start match/resolve/act cycle
            session.Fire();
        }
    }

    public class Customer
    {
        public string Name { get; private set; }
        public bool IsPreferred { get; set; }

        public Customer(string name)
        {
            Name = name;
        }

        public void NotifyAboutDiscount()
        {
            Console.WriteLine("Customer {0} was notified about a discount", Name);
        }
    }

    public class Order
    {
        public int Id { get; private set; }
        public Customer Customer { get; private set; }
        public int Quantity { get; private set; }
        public double UnitPrice { get; private set; }
        public double PercentDiscount { get; private set; }
        public bool IsDiscounted { get { return PercentDiscount > 0; } }
        public bool IsOpen { get; private set; }

        public double Price
        {
            get { return UnitPrice * Quantity * (1.0 - PercentDiscount / 100.0); }
        }

        public Order(int id, Customer customer, int quantity, double unitPrice, bool isOpen = true)
        {
            Id = id;
            Customer = customer;
            Quantity = quantity;
            UnitPrice = unitPrice;
            IsOpen = isOpen;
        }

        public void ApplyDiscount(double percentDiscount)
        {
            PercentDiscount = percentDiscount;
            Console.WriteLine(" set percentDiscount to {0}", percentDiscount);
        }
    }

    public class PreferredCustomerDiscountRule : Rule
    {
        public override void Define()
        {
            Customer customer = null;
            IEnumerable<Order> orders = null;

            When()
                .Match<Customer>(() => customer, c => c.IsPreferred)
                .Query(() => orders, x => x
                    .Match<Order>(
                        o => o.Customer == customer,
                        o => o.IsOpen,
                        o => !o.IsDiscounted)
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => ApplyDiscount(orders, 10.0))
                .Do(ctx => ctx.UpdateAll(orders));
        }

        private static void ApplyDiscount(IEnumerable<Order> orders, double discount)
        {
            foreach (var order in orders)
            {
                order.ApplyDiscount(discount);
            }
        }
    }

    public class DiscountNotificationRule : Rule
    {
        public override void Define()
        {
            Customer customer = null;

            When()
                .Match<Customer>(() => customer)
                .Exists<Order>(o => o.Customer == customer, o => o.PercentDiscount > 0.0);

            Then()
                .Do(_ => customer.NotifyAboutDiscount());
        }
    }
}
