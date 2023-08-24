using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using ConcordiaStation.Data.Context;
using Microsoft.EntityFrameworkCore;
using ConcordiaStation.Data.Exceptions;

namespace ConcordiaStation.Data.Repositories
{
    public class RepositoryScientist : IRepositoryScientist
    {
        private readonly ConcordiaLocalDbContext _context;
        public RepositoryScientist(ConcordiaLocalDbContext context) => _context = context;

        public IEnumerable<Scientist> GetAll()
        {
            return _context.Scientists
                .Include(x => x.Phase)
                ?? new List<Scientist>().AsEnumerable();
        }

        public Scientist GetById(int id)  => _context.Scientists.AsNoTracking().SingleOrDefault(x => x.Id == id);

        public Scientist GetByTrelloId(string trelloId) => _context.Scientists.AsNoTracking().SingleOrDefault(x => x.IdTrello == trelloId);

        public Scientist Insert(Scientist scientist)
        {
            var myscientist = _context.Scientists.Add(scientist);
            _context.SaveChanges();
            return myscientist.Entity;
        }

        public Scientist Update(Scientist scientist)
        {
            var toBeUpdeted = _context.Scientists.AsNoTracking().SingleOrDefault(x => x.Id == scientist.Id) ?? throw new ScientistNotFoundException();
            _context.Scientists.Update(toBeUpdeted with { GivenName = scientist.GivenName, FamilyName = scientist.FamilyName });
            _context.SaveChanges();
            return toBeUpdeted;
        }

        public bool Delete(int id) 
        {
            var firedOne = _context.Scientists.SingleOrDefault(x => x.Id == id);
            if(firedOne != null)
            {
                _context.Scientists.Remove(firedOne);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
