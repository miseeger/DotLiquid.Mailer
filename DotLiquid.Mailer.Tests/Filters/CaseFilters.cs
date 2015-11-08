using System;
using System.Linq;

namespace DotLiquid.Mailer.Tests.Filters
{

    public static class CaseFilters
    {

        /// <summary>
        /// Filter which camel-cases a string. 
        /// Usage: {{"to be camel cased | camelcase}}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Camelcase(string input)
        {
            if (input == null || input.Length < 2)
                return input;

            var words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            var result = words[0].ToLower();
            for (var i = 1; i < words.Length; i++)
            {
                result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
            }

            return result;
        }


        /// <summary>
        /// Filter which camel-cases a string. 
        /// Usage: {{"to be camel cased | camel_case}}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CamelCase(string input)
        {
            return Camelcase(input);
        }


        public static string Pascalcase(string input)
        {
            if (input == null) return null;

            if (input.Length < 2) return input.ToUpper();

            var words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            return words
                .Aggregate("",
                    (current, word) => current + (word.Substring(0, 1).ToUpper() + word.Substring(1)));
        }


        public static string Propercase(string input)
        {
            if (input == null) return null;

            if (input.Length < 2) return input.ToUpper();

            var result = input.Substring(0, 1).ToUpper();

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i])) result += " ";
                result += input[i];
            }

            return result;
        }

    }

}