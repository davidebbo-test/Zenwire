using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Xml.Linq;
using Zenwire.Domain;
using Zenwire.Repositories;
using Zenwire.Services;

namespace Zenwire.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public NotificationController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public ContentResult AppointmentConfirmation(int id = 0)
        {
            Appointment appointment = _appointmentService.Get(id);

            if (appointment != null)
            {
                var response = new XElement("Response");

                var appointmentDate = appointment.ScheduledStart.ToString("dddd, , MMMM, , d, ,");
                var appointmentTime = appointment.ScheduledStart.ToString("h, , m, ,");

                var appointmentAmPm = appointment.ScheduledStart.ToString("tt");

                appointmentAmPm = (appointmentAmPm == "AM") ? "A, M" : "P, M";


                //var orderStatus = "COMPLETED!";
                //var orderNumber = string.Join(". ", id.ToString().ToCharArray());

                var say = new XElement("Say",
                    "Hello. This is an automated call from ZEN WIRE, your consultation appointment for " +
                    appointmentDate + " at " + appointmentTime + appointmentAmPm + " has been confirmed!");

                say.Add(new XAttribute("voice", "man"));
                say.Add(new XAttribute("loop", "2"));

                response.Add(say);

                var pause = new XElement("Pause");
                pause.Add(new XAttribute("length", 1));

                response.Add(pause);

                say = new XElement(new XElement("Say", "We look forward to seeing you! Have a great day!"));
                response.Add(say);

                pause = new XElement("Pause");
                pause.Add(new XAttribute("length", 2));

                response.Add(pause);

                return new ContentResult
                {
                    Content = response.ToString(),
                    ContentType = "text/xml"
                };
            }

            return null;
        }
    }
}
