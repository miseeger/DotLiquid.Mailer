using System;
using System.Linq;

namespace DotLiquid.Mailer
{

    // These functions are taken from the onybo/DotLiquid-ViewModel
    // project on GitHub (https://github.com/onybo/DotLiquid-ViewModel)
    // written by Tim Jones (http://timjones.tw) which is provided
    // also using the MIT License.

    public static class LiquidFunctions
    {

        public static void RegisterViewModel(Type rootType)
        {
            rootType
                .Assembly
                .GetTypes()
                .Where(t => t.Namespace == rootType.Namespace)
                .ToList()
                .ForEach(RegisterSafeTypeWithAllProperties);
        }


        private static void RegisterSafeTypeWithAllProperties(Type type)
        {
            Template.RegisterSafeType(type,
                type
                    .GetProperties()
                    .Select(p => p.Name)
                    .ToArray());
        }


        public static string RenderViewModel(this Template template, object root)
        {
            return template.Render(
                Hash.FromAnonymousObject(root));
        }

    }

}