using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Zenwire.Domain;
using Zenwire.Services;
using Zenwire.Models;

namespace Zenwire.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeeService _employeeService;

        public AppointmentController(IAppointmentService appointmentService,ICustomerService customerService, IEmployeeService employeeService)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
            _employeeService = employeeService;
        }

        // GET: /Appointment/
        public ActionResult Index()
        {
            return View(_appointmentService.Get());
        }

        public ActionResult ThankYou()
        {
            return View("ThankYou");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Schedule(ScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerService.GetByEmail(model.CustomerSearch) ??
                               new Customer() { Email = model.CustomerSearch };

                return View("Schedule", new ScheduleModel()
                {
                    Customer = customer,
                    Appointment = new Appointment()
                    {
                        Customer = customer,
                        CustomerId = customer.Id
                    }
                });
            }

            return View("Schedule", new ScheduleModel()
            {
                Customer = new Customer()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppointment(ScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerService.Add(model.Appointment.Customer);

                if (customer != null)
                {
                    model.Appointment.Customer = customer;
                    model.Appointment.CustomerId = customer.Id;

                    model.Appointment.ScheduledStart = model.Appointment.ScheduledStart + model.AppointmentTime;
                    model.Appointment.ScheduledEnd = model.Appointment.ScheduledStart.AddHours(1);

                    _appointmentService.Schedule(model.Appointment);
                    
                    if(model.Appointment.Id > 0) return View("ThankYou");
                }

                ModelState.AddModelError("Error", "Something went wrong");
                return View("Schedule", model);
            }

            return HttpNotFound();
        }

        public ActionResult Schedule()
        {
            return View("Schedule", new ScheduleModel()
            {
                Customer = new Customer()
            });
        }

        // GET: /Appointment/Details/5
        public ActionResult Details(int id = 0)
        {
            Appointment appointment = _appointmentService.Get(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: /Appointment/Create
        public ActionResult Create()
        {

            return View(new AppointmentModel()
            {
                Customers = _customerService.Get(),
                Appointment = new Appointment()
            });
        }

        // POST: /Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentModel model)
        {
            if (ModelState.IsValid)
            {
                model.Appointment.ScheduledStart = model.Appointment.ScheduledStart + model.AppointmentTime;
                _appointmentService.Schedule(model.Appointment);
            }

            return RedirectToAction("Index");
        }

        // GET: /Appointment/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Appointment appointment = _appointmentService.Get(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(new AppointmentModel()
            {
                Appointment = appointment,
                Customers = _customerService.Get()
            });
        }

        // POST: /Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppointmentModel model)
        {
            if (ModelState.IsValid)
            {
                model.Appointment.ScheduledStart = model.Appointment.ScheduledStart + model.AppointmentTime;
                
                if (model.Appointment.CustomerId == 0)
                {
                    model.Appointment.CustomerId = _appointmentService.Get(model.Appointment.Id).CustomerId;
                } 
                
                _appointmentService.Schedule(model.Appointment);
            }
            
            model.Customers = _customerService.Get();
            
            return View(model);
        }

        [HttpPost] 
        public JsonResult ChangeTime(int appointmentId, int days, int minutes, bool confirmUpdate = false)
        {
            bool IsSuccess = false;
            if (appointmentId > 0)
            {
                IsSuccess = _appointmentService.ChangeTime(appointmentId, days, minutes, confirmUpdate);
            }
            return Json(new { IsSuccess});
        }

        // GET: /Appointment/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Appointment appointment = _appointmentService.Get(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: /Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _appointmentService.Remove(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetEvents(int start, int end)
        {
            
            var culture = new CultureInfo("en-CA");
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);

            var json = from a in _appointmentService.Get()
                       where
                       a.ScheduledStart >= epoch.AddSeconds(start) &&
                       a.ScheduledStart <= epoch.AddSeconds(end)
                       select new
                       {
                           id = a.Id,
                           title = a.EmployeeId > 0 ? a.Employee.Fullname : "Unassigned",
                           customer = a.Customer.Fullname,
                           address = a.Customer.Address,
                           city = a.Customer.City,
                           phone = a.Customer.Phone,
                           start = a.ScheduledStart.ToUniversalTime().ToString("r",culture),
                           end = a.ScheduledStart.AddHours(1).ToUniversalTime().ToString("r", culture)
                       };

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAvailableHours(DateTime? date, int? appointmentId, int duration)
        {
            return Json(_appointmentService.GetAvailableHours(date, appointmentId, duration).Select(x => 
                new {
                    text  = DateTime.Today.Add(x).ToString("hh:mm tt"),
                    value = string.Format("{0:D2}:{1:D2}", x.Hours, x.Minutes)
                    }
                ), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Assign(int id)
        {
            Appointment appointment = _appointmentService.Get(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(new AppointmentModel()
            {
                Appointment = appointment,
                Customers = _customerService.Get(),
                Employees = _employeeService.Get()
            });
        }

        // POST: /Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(AppointmentModel model)
        {
            if (ModelState.IsValid)
            {
                _appointmentService.AssignToEmployee(model.Appointment);
            }
            return RedirectToAction("Index");
        }

    }
}