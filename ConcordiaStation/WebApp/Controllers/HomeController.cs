using ConcordiaStation.Data.Services.Interfaces;
using ConcordiaStation.WebApp;
using ConcordiaStation.WebApp.SecurityServices.Interfaces;
using ConcordiaStation.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SynchronizerBackground _synchronizerBackground;
        private readonly IServiceExperiment _serviceExperiment;
        private readonly IServicePhase _servicePhase;
        private readonly IServiceToken _sToken;

        public HomeController(ILogger<HomeController> logger,IServiceExperiment serviceExpetiment, IServicePhase servicePhase, IServiceExperiment serviceExperiment, IServiceToken serviceToken, SynchronizerBackground synchronizerBackground)
        {
            _logger = logger;
            _servicePhase = servicePhase;
            _serviceExperiment = serviceExperiment;
            _sToken = serviceToken;
            _synchronizerBackground = synchronizerBackground;
            _synchronizerBackground = synchronizerBackground;
        }

        public async Task<IActionResult> Index()
        {
            //if (Request.Cookies.TryGetValue("token", out string token) && _sToken.ValidateToken(token))
            //{
                _logger.LogInformation("Token reded succesfully");
                _logger.LogInformation("Start synchronization...");
                var cancellationToken = HttpContext.RequestAborted;
                await _synchronizerBackground.StartAsync(cancellationToken);
                _logger.LogInformation("Synchronization finished.");
                return View(BuildExperimentModel());
            //}
            //else
            //{
            //    _logger.LogInformation("Session expired.");
            //    //return RedirectToAction("Index", "Login");
               
            //}
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GoToDetails()
        {
            _logger.LogInformation("Redirect to PhaseDetail.Index");
            return RedirectToAction("Details", "Phase");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private CardIndexViewModel BuildExperimentModel()
        {
            _logger.LogInformation("Building view model...");
            var experiments = _serviceExperiment.GetAllExperiments().ToList();
            var phases = _servicePhase.GetAllPhases().ToList();

            foreach (var experiment in experiments)
            {
                _serviceExperiment.UpdatePriorityOnDeadline(experiment);
            }

            return new CardIndexViewModel(experiments, phases);
        }
    }
}