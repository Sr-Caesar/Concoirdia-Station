using ConcordiaStation.Data.Models;

namespace ConcordiaStation.Data.Repositories.Interfaces
{
    public interface IRepositoryComment : IRepository<Comment>
    {
        public IEnumerable<Comment> GetByPhaseId(int phaseId);
    }
}