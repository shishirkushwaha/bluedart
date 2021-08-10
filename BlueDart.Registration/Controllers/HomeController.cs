using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlueDart.Registration.Models;
using BlueDart.Messaging;
using BlueDart.Messaging.Events;
using BlueDart.Registration.ViewModels;
using Newtonsoft.Json;

namespace BlueDart.Registration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToActionPermanent("RegisterOrder");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderViewModel model)
        {
            Console.WriteLine(JsonConvert.SerializeObject(model));

            //Send RegisterOrderCommand To Saga Orchestrator
            var bus = BusConfigurator.ConfigureBus();

            var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri}{RabbitMqConstants.SagaQueue}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);

            await endPoint.Send<IRegisterOrderCommand>(new
            {
                PickupName = model.PickupName,
                PickupAddress = model.PickupAddress,
                PickupCity = model.PickupCity,
                DeliverName = model.DeliverName,
                DeliverAddress = model.DeliverAddress,
                DeliverCity = model.DeliverCity,
                Weight = model.Weight,
                Fragile = model.Fragile,
                Oversized = model.Oversized
            });

            return View("Thanks");
        }
    }
}
