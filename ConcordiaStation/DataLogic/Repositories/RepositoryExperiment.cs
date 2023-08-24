using ConcordiaStation.Data.Context;
using ConcordiaStation.Data.Dto;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ConcordiaStation.Data.Repositories
{
    public class RepositoryExperiment : IRepositoryExperiment
    {
        private readonly ConcordiaLocalDbContext _context;
        public RepositoryExperiment(ConcordiaLocalDbContext context) => _context = context;

        public IEnumerable<Experiment> GetAll() => _context.Experiments.Include(x => x.Phases) ?? new List<Experiment>().AsEnumerable();

        public Experiment GetById(int id) => _context.Experiments.AsNoTracking().SingleOrDefault(x => x.Id == id);

        public Experiment GetByTrelloId(string trelloId) => _context.Experiments.AsNoTracking().SingleOrDefault(x => x.IdListTrello == trelloId);

        public Experiment Insert(Experiment experiment)
        {
            var newCard = _context.Experiments.Add(experiment);
            _context.SaveChanges();
            return newCard.Entity;
        }

        public Experiment Update(Experiment experiment)
        {
            var toBeUpdeted = _context.Experiments.AsNoTracking().SingleOrDefault(x => x.Id == experiment.Id);
            _context.Experiments.Update(toBeUpdeted! with {
                Title = experiment.Title,
                Phases = experiment.Phases
            });
            _context.SaveChanges();
            return experiment;
        }

        public bool Delete(int id)
        {
            var toDelete = _context.Experiments.SingleOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                _context.Experiments.Remove(toDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
