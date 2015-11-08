using System;
using System.Collections.Generic;
using System.IO;

namespace DotLiquid.Mailer.Tests.Blocks
{

    public class ByChance : Block
    {
        private int _max;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
            _max = Convert.ToInt32(markup);
        }


        public override void Render(Context context, TextWriter textWriter)
        {
            var rnd = new Random().Next(_max);

            if ( rnd == 0)
            {
                base.Render(context, textWriter);
            }
        }

    }

}