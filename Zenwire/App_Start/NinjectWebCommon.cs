using System.Diagnostics.CodeAnalysis;
using Zenwire.Domain.Commissions;
using Zenwire.Gateways;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Zenwire.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Zenwire.App_Start.NinjectWebCommon), "Stop")]

namespace Zenwire.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Reflection;
    using System.Web.Mvc;
    using Zenwire.Controllers;
    using Zenwire.Services;
    using Zenwire.Repositories;
    using Zenwire.Domain;
    using Ninject.Web.Mvc;
    using Zenwire.Models;

    [ExcludeFromCodeCoverage]
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ZenwireContext>().ToSelf().InRequestScope();

            #region Controllers
            kernel.Bind<IController>().To<AppointmentController>();
            kernel.Bind<IController>().To<ShiftController>();
            kernel.Bind<IController>().To<CustomerController>();
            kernel.Bind<IController>().To<AccountController>();
            kernel.Bind<IController>().To<NotificationController>();
            kernel.Bind<IController>().To<ProductController>();
            kernel.Bind<IController>().To<ReferralController>();
            #endregion

            #region Services
            kernel.Bind<IAppointmentService>().To<AppointmentService>();
            kernel.Bind<IReferralService>().To<ReferralService>();
            kernel.Bind<ICustomerService>().To<CustomerService>();
            kernel.Bind<IEmployeeService>().To<EmployeeService>();
            kernel.Bind<IShiftService>().To<ShiftService>();
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<INotificationService>().To<NotificationService>();
            #endregion

            #region Repositories
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            kernel.Bind<IRepository<Employee>>().To<Repository<Employee>>();
            kernel.Bind<IRepository<Shift>>().To<Repository<Shift>>();
            kernel.Bind<IRepository<PayCode>>().To<Repository<PayCode>>();
            #endregion

            #region Gateways
            kernel.Bind<INotificationGateway>().To<NotificationGateway>();
            #endregion

            #region Models
            kernel.Bind<ShiftModel>().ToSelf().InRequestScope();
            kernel.Bind<ReferralModel>().ToSelf().InRequestScope(); 
            #endregion

            //kernel.Bind<ZenwireContext>().ToConstructor(_ => new ZenwireContext());
            //kernel.Bind<IZenwireContext>().To<ZenwireContext>();
            kernel.Load(Assembly.GetExecutingAssembly());
        }
    }
}
