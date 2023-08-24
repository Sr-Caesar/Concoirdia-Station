using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Services;
using ConcordiaStation.Data.Services.Interfaces;
using ConcordiaStation.WebApp.SecurityServices.Interfaces;
using ConcordiaStation.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaStation.WebApp.Controllers
{
    public class PhaseController : Controller
    {
        private readonly ILogger<PhaseController> _logger;
        private readonly IServicePhase _servicePhase;
        private readonly IServiceComment _serviceComment;
        private readonly IServiceToken _sToken;

        public PhaseController(ILogger<PhaseController> logger, IServicePhase servicePhase, IServiceComment serviceComment, IServiceToken serviceToken)
        {
            _logger = logger;
            _servicePhase = servicePhase;
            _serviceComment = serviceComment;
            _sToken = serviceToken;
        }
        public IActionResult Details(int phaseId)
        {
            if (Request.Cookies.TryGetValue("token", out string token) && _sToken.ValidateToken(token))
            {
                _logger.LogInformation("Task.Index Was Called");
                //return View(_repositoryPhase.GetById(phaseId));
                return View(BuildViewModel(phaseId));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult UpdateStatus(PhaseDetailViewModel viewData)
        {
            var phase = _servicePhase.GetPhaseById(viewData.Id);
            var newStatus = viewData.Status;
            _servicePhase.UpdateStatus(phase, newStatus);
            return View("Details", BuildViewModel(viewData.Id));
        }

        public IActionResult InsertNewComment(PhaseDetailViewModel viewData)
        {
            var phase = _servicePhase.GetPhaseById(viewData.Id);
            var scientists = phase.Scientists.ToList();
            var random = new Random();
            var randomScientist = scientists[random.Next(scientists.Count())];

            var myComment = new Comment()
            {
                Content = viewData.NewComment,
                PublicationDate = DateTime.Now,
                PhaseId = phase.Id,
                ScientistId = randomScientist.Id
            };

            _serviceComment.InsertComment(myComment);

            return View("Details", BuildViewModel(viewData.Id));
        }
        public PhaseDetailViewModel BuildViewModel(int phaseId)
        {
            var myModel = _servicePhase.GetPhaseById(phaseId);
            return new PhaseDetailViewModel
            {
                Id = (int)myModel.Id,
                Title = myModel.Title,
                Description = myModel.Description,
                Deadline = myModel.Deadline,
                Priority = myModel.Priority,
                Status = myModel.Status,
                Scientists = myModel.Scientists,
                Experiment = myModel.Experiment,
                Comments = _serviceComment.GetCommentsByPhaseId(phaseId),
                NewComment = string.Empty
            };

        }
    }
 }

