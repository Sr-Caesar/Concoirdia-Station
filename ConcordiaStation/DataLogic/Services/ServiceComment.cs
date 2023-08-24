using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using Serilog;

namespace ConcordiaStation.Data.Services
{
    public class ServiceComment : IServiceComment
    {
        private readonly IRepositoryComment _repositoryComment;
        public ServiceComment(IRepositoryComment repositoryComment) => _repositoryComment = repositoryComment;

        public ServiceComment()
        {
        }

        public DateTime GetLastActivity(Comment lastComment) => lastComment.PublicationDate;

        public IEnumerable<Comment> GetCommentsByPhaseId(int phaseId)
        {
            try
            {
                return _repositoryComment.GetByPhaseId(phaseId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Si è verificato un errore durante il recupero della Fase");
                return Enumerable.Empty<Comment>();
            }
        }

        public Comment InsertComment(Comment comment)
        {
            if (_repositoryComment.GetAll().Any(x => x.Id == comment.Id))
            {
                throw new ArgumentException("L'ID del commento esiste già.", nameof(comment.Id));
            }
            if (comment.PhaseId == null || comment.ScientistId == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                return _repositoryComment.Insert(comment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Il commento che si è cercato di inserire non è valido");
                return comment;
            }
        }
    }
}
