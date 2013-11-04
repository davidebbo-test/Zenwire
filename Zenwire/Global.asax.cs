using System.Diagnostics.CodeAnalysis;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Zenwire.Repositories;

namespace Zenwire
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    [ExcludeFromCodeCoverage]
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleTable.EnableOptimizations = false;
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Database.SetInitializer<ZenwireContext>(null);

            // !IMPORTANT! DO NOT UNCOMMENT THE FOLLOWING LINES IN PRODUCTION
            //Database.SetInitializer<ZenwireContext>(new DropCreateDatabaseIfModelChanges<ZenwireContext>());
            Database.SetInitializer<ZenwireContext>(new DropCreateDatabaseAlways<ZenwireContext>());

            using (var context = new ZenwireContext())
            {
                context.Database.Initialize(false);
            }

            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }
    }
}