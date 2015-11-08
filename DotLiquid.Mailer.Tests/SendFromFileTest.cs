using DotLiquid.Mailer.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotLiquid.Mailer.Tests
{
    [TestClass]
    public class SendFromFileTest : BaseMailerTest
    {

        [TestMethod]
        public void ShouldSendEmailWithAnonymousFromDefaultAddress()
        {
            Assert.IsTrue(_mailer.SendFromFile("ShouldSendEmailWithAnonymous", 
                "OrderConfirmation.liquid.html", _anonData, "john.doe@nowhere.net"));
        }


        [TestMethod]
        public void ShouldSendEmailFromDefaultAddress()
        {
            Assert.IsTrue(_mailer.SendFromFile<Order>("ShouldSendEmail",
                "OrderConfirmation.liquid.html", _orderData, "john.doe@nowhere.net"));
        }


        [TestMethod]
        public void ShouldSendEmailWithAnonymousFromNamedAddress()
        {
            Assert.IsTrue(_mailer.SendFromFile("ShouldSendEmailWithAnonymous",
                "OrderConfirmation.liquid.html", _anonData, "john.doe@nowhere.net",
                "named.sender@mytest.org"));
        }


        [TestMethod]
        public void ShouldSendEmailFromNamedAddress()
        {
            Assert.IsTrue(_mailer.SendFromFile<Order>("ShouldSendEmail",
                "OrderConfirmation.liquid.html", _orderData, "john.doe@nowhere.net",
                "named.sender@mytest.org"));
           
        }

    }

}
