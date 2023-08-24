using ConcordiaStation.Data.Context;
using ConcordiaStation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaStation.Data.Repositories
{
    public class RepositoryCredential : IRepositoryCredential
    {
        private readonly ConcordiaLocalDbContext _context;
        public RepositoryCredential(ConcordiaLocalDbContext context) => _context = context;

        public IEnumerable<Credential> GetAll() => _context.Credentials.Include(x => x.Scientist) ?? new List<Credential>().AsEnumerable();
        public Credential GetById(int id) => _context.Credentials.AsNoTracking().SingleOrDefault(x => x.Id == id);
        public Credential GetByTrelloId(string trelloId) => throw new NotImplementedException();
        public Credential Insert(Credential userCredentials)
        {
            var newUserCredential = _context.Credentials.Add(userCredentials);
            _context.SaveChanges();
            return newUserCredential.Entity;
        }
        public Credential Update(Credential userCredentials)
        {
            var toBeUpdeted = _context.Credentials.AsNoTracking().SingleOrDefault(x => x.Id == userCredentials.Id);
            _context.Credentials.Update(toBeUpdeted! with { Password = userCredentials.Password });
            _context.SaveChanges();
            return toBeUpdeted;
        }
        public bool Delete(int id) => throw new NotImplementedException();
    }
}
