using DotLiquid.Mailer.Tests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotLiquid.Mailer.Tests
{

    [TestClass]
    public class SendTest : BaseMailerTest
    {

        private const string _template =
            "<!DOCTYPE html>" +
            "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
            "<body>" +
            "{{Cust.Salutation" +
            "    }" +
            "},<br/><br/>" +
            "thank you for the following order made for {{Cust.Firstname}} {{Cust.Lastname}}:" +
            "" +
            "<ul>" +
            "    {% for Item in Itemlist -%}" +
            "    <li>" +
            "        {{Item.Quantity}} x {{Item.Name}}" +
            "    </li>" +
            "    {% endfor -%}" +
            "</ul>" +
            "" +
            "Your Sales Company." +
            "</body>" +
            "</html>";


        [TestMethod]
        public void ShouldSendEmailWithAnonymousFromDefaultAddress()
        {
            Assert.IsTrue(_mailer.Send("ShouldSendEmailWithAnonymous", _template, _anonData, 
                "john.doe@nowhere.net"));
        }


        [TestMethod]
        public void ShouldSendEmailFromDefaultAddress()
        {
            Assert.IsTrue(_mailer.Send<Order>("ShouldSendEmail", _template, _orderData, 
                "john.doe@nowhere.net"));
        }


        [TestMethod]
        public void ShouldSendEmailWithAnonymousFromNamedAddress()
        {
            Assert.IsTrue(_mailer.Send("ShouldSendEmailWithAnonymous", _template, _anonData, 
                "john.doe@nowhere.net", "named.sender@mytest.org"));
        }


        [TestMethod]
        public void ShouldSendEmailFromNamedAddress()
        {
            Assert.IsTrue(_mailer.Send<Order>("ShouldSendEmail", _template, _orderData, 
                "john.doe@nowhere.net", "named.sender@mytest.org"));
        }


        [TestMethod]
        public void ShouldSendEmailAtHighPriority()
        {
            _mailer.IsHigh = true;
            Assert.IsTrue(_mailer.Send<Order>("ShouldSendEmailHigh", _template, _orderData,
                "john.doe@nowhere.net", "named.sender@mytest.org"));
            Assert.IsFalse(_mailer.IsHigh);
        }

    }

}
