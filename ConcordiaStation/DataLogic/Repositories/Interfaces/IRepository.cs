using ConcordiaStation.Data.Models;

namespace ConcordiaStation.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        public TEntity GetByTrelloId(string trelloId);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        bool Delete(int id);
    }
}
