using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Models;

namespace ConcordiaStation.Data.Services
{
    public interface IServiceComment
    {
        public IEnumerable<Comment> GetCommentsByPhaseId(int phaseId);
        public DateTime GetLastActivity(Comment lastComment);
        public Comment InsertComment(Comment comment);
    }
}