using ConcordiaStation.Data.Services;
using ConcordiaStation.Data.Services.Interfaces;
using ConcordiaStation.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConcordiaStation.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly IServiceComment _serviceComment;
        private readonly IServicePhase _servicePhase;
        private readonly ILogger<PhaseController> _logger;
        public CommentController(ILogger<PhaseController> logger, IServiceComment serviceComment, IServicePhase servicePhase)
        {
            _logger = logger;
            _serviceComment = serviceComment;
            _servicePhase = servicePhase;

        }
        public IActionResult ShowAllComment(int phaseId)
        {
            _logger.LogInformation("Task.Index Was Called");
            return View(BuildViewModel(phaseId));
        }
        public AllCommentViewModel BuildViewModel(int phaseId)
        {
            var myModel = _servicePhase.GetPhaseById(phaseId);
            return new AllCommentViewModel
            {
                Comments = _serviceComment.GetCommentsByPhaseId(phaseId),

                Scientists = myModel.Scientists
            };
        }
    }
}
