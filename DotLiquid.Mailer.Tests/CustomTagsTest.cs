using System;
using System.Collections.Generic;
using System.Linq;
using DotLiquid.Mailer.Tests.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotLiquid.Mailer.Tests
{

    [TestClass]
    public class CustomTagsksTest : BaseMailerTest
    {

        [TestMethod]
        public void ShouldRenderRandomTag()
        {
            var results = new List<int>();
            
            _mailer.RegisterTag<RandomInt>("random");

            for (var i = 0; i < 100000; i++)
            {
                results.Add(int.Parse(Template.Parse("{% random 25 %}").Render()));
            }

            Assert.IsTrue(results.All(r => r <= 25 && r >= 0));
        }

    }

}
