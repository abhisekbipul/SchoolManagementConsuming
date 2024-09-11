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
            List<Event> eventList = new List<Event>();
            string url = "https://localhost:7062/api/Event/GetEvents";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<Event>>(jsonData);
                if (obj != null)
                {
                    eventList = obj;
                }
            }

            var calendarEvents = eventList.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                startDate = e.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                endDate = e.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                description = e.Description,
                isAcademic = e.IsAcademic
            });
            return View(eventList);
        }

        public IActionResult AddEvent()
        {
            var newEvent = new Event();
            return View(newEvent);
        }

        [HttpPost]
        public IActionResult AddEvent(Event newEvent)
        {
            string url = "https://localhost:7062/api/Event/CreateEvent"; 
            var jsonData = JsonConvert.SerializeObject(newEvent);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage result = client.PostAsync(url, stringContent).Result;

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return BadRequest();
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
