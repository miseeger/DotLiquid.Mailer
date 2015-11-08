using System;
using System.Collections.Generic;
using System.Linq;
using DotLiquid.Mailer.Tests.Blocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotLiquid.Mailer.Tests
{

    [TestClass]
    public class CustomBlocksTest : BaseMailerTest
    {

        [TestMethod]
        public void ShouldRenderBychanceBlock()
        {
            var results = new List<string>();
            
            _mailer.RegisterTag<ByChance>("bychance");

            for (var i = 0; i < 100000; i++)
            {
                results.Add(Template.Parse("{% bychance 20 %}You're the lucky winner!{% endbychance %}").Render());
            }

            Assert.IsTrue(results.Any(r => r == "You're the lucky winner!"));
            Assert.IsTrue(results.Any(r => r == ""));
        }

    }

}
