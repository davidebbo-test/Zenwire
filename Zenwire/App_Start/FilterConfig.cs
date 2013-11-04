using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace Zenwire
{
    [ExcludeFromCodeCoverage]
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}