using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using DotLiquid.Mailer.Tests.Blocks;
using DotLiquid.Mailer.Tests.Filters;
using DotLiquid.Mailer.Tests.Model;
using DotLiquid.Mailer.Tests.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotLiquid.Mailer.Tests
{

    [TestClass]
    public class AutofacIocTest : BaseMailerTest
    {

        private static IContainer _container;


        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new MailEngine()
            {
                DefaultFromAddress = "dotliquid.mailer@mytest.org",
                IsHtml = true,
                SmtpServer = "127.0.0.1",
                SmtpPort = 25,
                UseDefaultCredentials = true,
                TemplateDir = @".\Templates"
            })
            .As<IMailEngine>();

            _container = builder.Build();
        }


        [TestMethod]
        public void ShouldResolveRegisteredMailer()
        {
            var mailer = _container.Resolve<IMailEngine>();

            Assert.IsNotNull(mailer);  
        }


        [TestMethod]
        public void ShouldSendEmailWithAnonymousFromDefaultAddress()
        {
            var mailer = _container.Resolve<IMailEngine>();

            Assert.IsTrue(mailer.SendFromFile("ShouldSendEmailWithAnonymous",
                "OrderConfirmation.liquid.html", _anonData, "john.doe@nowhere.net"));
        }


        [TestMethod]
        public void ShouldSendEmailFromNamedAddress()
        {
            var mailer = _container.Resolve<IMailEngine>();

            Assert.IsTrue(mailer.SendFromFile<Order>("ShouldSendEmail",
                "OrderConfirmation.liquid.html", _orderData, "john.doe@nowhere.net",
                "named.sender@mytest.org"));
        }


        [TestMethod]
        public void ShouldRegisterAndRenderCustomBlocks()
        {
            var mailer = _container.Resolve<IMailEngine>();
            var results = new List<string>();

            mailer.RegisterTag<ByChance>("bychance");

            for (var i = 0; i < 100000; i++)
            {
                results.Add(Template.Parse("{% bychance 20 %}You're the lucky winner!{% endbychance %}").Render());
            }

            Assert.IsTrue(results.Any(r => r == "You're the lucky winner!"));
            Assert.IsTrue(results.Any(r => r == ""));
        }


        [TestMethod]
        public void ShouldRegisterAndRenderCustomFilters()
        {
            var mailer = _container.Resolve<IMailEngine>();

            mailer.RegisterFilter(typeof(CaseFilters));

            Assert.AreEqual("testObject", Template.Parse("{{'test object' | camelcase }}").Render());
            Assert.AreEqual("testObject", Template.Parse("{{'test object' | camel_case }}").Render());
            Assert.AreEqual("TestObject", Template.Parse("{{'test object' | pascalcase }}").Render());
            Assert.AreEqual("Test Object", Template.Parse("{{'testObject' | propercase }}").Render());
        }


        [TestMethod]
        public void ShouldRegisterAndRenderCustomTags()
        {
            var mailer = _container.Resolve<IMailEngine>();
            var results = new List<int>();

            mailer.RegisterTag<RandomInt>("random");

            for (var i = 0; i < 100000; i++)
            {
                results.Add(int.Parse(Template.Parse("{% random 25 %}").Render()));
            }

            Assert.IsTrue(results.All(r => r <= 25 && r >= 0));
        }

    }

}
