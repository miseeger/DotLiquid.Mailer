using DotLiquid.Mailer.Tests.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotLiquid.Mailer.Tests
{

    [TestClass]
    public class CustomFilterTest : BaseMailerTest
    {

        [TestMethod]
        public void ShouldRegisterCamelCaseFilter()
        {
            _mailer.RegisterFilter(typeof(CaseFilters));

            Assert.AreEqual("testObject", Template.Parse("{{'test object' | camelcase }}").Render());
            Assert.AreEqual("testObject", Template.Parse("{{'test object' | camel_case }}").Render());
            Assert.AreEqual("TestObject", Template.Parse("{{'test object' | pascalcase }}").Render());
            Assert.AreEqual("Test Object", Template.Parse("{{'testObject' | propercase }}").Render());
        }


        [TestMethod]
        public void ShouldSendCustomCasedList()
        {
            _mailer.RegisterFilter(typeof(CaseFilters));

            var data = new
            {
                ItemList = new[]
                {
                    new {Name = "Those are droids you were lookin for"},
                    new {Name = "Another one bites the dust of tatooine"}
                }
            };

            Assert.IsTrue(_mailer.SendFromFile("CistomCasedItemList", "FilterTest.liquid.html", data,
                "a.b@c.de"));
        }

    }

}
