using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagementConsuming.Models;
using System.Text;

namespace SchoolManagementConsuming.Controllers
{
    public class EventController : Controller
    {
        HttpClient client;
        public EventController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetEvents()
        {
            List<Event> eventList = new List<Event>();
            string url = "https://localhost:7062/api/Event/GetEvents";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                eventList = JsonConvert.DeserializeObject<List<Event>>(jsonData);
            }

            var calendarEvents = eventList.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.StartDate.ToString("yyyy-MM-dd"),
                end = e.EndDate.ToString("yyyy-MM-dd"),
                description = e.Description,
                isAcademic = e.IsAcademic
            });

            return Json(calendarEvents);
        }

        public IActionResult AddEvent()
        {
            var newEvent = new Event();
            return View(newEvent);
        }

        [HttpPost]
        public IActionResult AddEvent(Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event data is null.");
            }

            string url = "https://localhost:7062/api/Event/CreateEvent";
            var jsonData = JsonConvert.SerializeObject(newEvent);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage result = client.PostAsync(url, stringContent).Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(newEvent);
        }

        public IActionResult UpdateEvent(int id)
        {
            string url = $"https://localhost:7062/api/Event/GetEventsById/{id}";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var eventObj = JsonConvert.DeserializeObject<Event>(jsonData);

                if (eventObj != null)
                {
                    return View(eventObj);
                }
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateEvent(Event updatedEvent)
        {
            string url = "https://localhost:7062/api/Event/UpdateEvent";
            var jsonData = JsonConvert.SerializeObject(updatedEvent);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage result = client.PutAsync(url, stringContent).Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(updatedEvent);
        }

        public IActionResult DeleteEvent(int id)
        {
            string url = $"https://localhost:7062/api/Event/DeleteEvent/{id}";

            HttpResponseMessage result = client.DeleteAsync(url).Result;

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
