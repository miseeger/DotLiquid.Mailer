using System.Collections.Generic;
using DotLiquid.Mailer.Tests.Model;

namespace DotLiquid.Mailer.Tests
{
    public class BaseMailerTest
    {
        protected static IMailEngine _mailer;
        protected static object _anonData;
        protected static Order _orderData;

        public BaseMailerTest()
        {
            _mailer = new MailEngine()
            {
                DefaultFromAddress = "dotliquid.mailer@mytest.org",
                IsHtml = true,
                SmtpServer = "127.0.0.1",
                SmtpPort = 25,
                UseDefaultCredentials = true,
                TemplateDir = @".\Templates"
            };

            _anonData = new
            {
                Cust = new
                {
                    Salutation = "Dear Mr. Doe",
                    Firstname = "John",
                    Lastname = "Doe"
                },
                ItemList = new[]
                    {
                        new {Name = "Apple", Quantity = 5},
                        new {Name = "Banana", Quantity = 10}
                    }
            };

            _orderData = new Order()
            {
                Cust = new Customer()
                {
                    Salutation = "Dear Mr. Doe",
                    Firstname = "John",
                    Lastname = "Doe"
                }
            };

            _orderData.Itemlist.AddRange( new List<Item>()
            {
                new Item()
                {
                    Name = "Apple",
                    Quantity = 5
                },
                new Item()
                {
                    Name = "Banana",
                    Quantity = 10
                }
            });

        }

    }
}