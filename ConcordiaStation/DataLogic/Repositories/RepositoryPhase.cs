using ConcordiaStation.Data.Context;
using ConcordiaStation.Data.Exceptions;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaStation.Data.Repositories
{
    public class RepositoryPhase : IRepositoryPhase
    {
        private readonly ConcordiaLocalDbContext _context;
        public RepositoryPhase(ConcordiaLocalDbContext context) => _context = context;

        public IEnumerable<Phase> GetAll() => _context.Phases.Include(phase => phase.Scientists);

        public Phase GetById(int id) => _context.Phases.AsNoTracking().Include(phase => phase.Scientists).FirstOrDefault(phase => phase.Id == id);

        public Phase GetByTrelloId(string trelloId) => _context.Phases.Include(phase => phase.Scientists).FirstOrDefault(phase => phase.IdCardTrello == trelloId);

        public Phase Insert(Phase entity)
        {
            _context.Phases.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Phase Update(Phase phase)
        {
            var intId = (int)phase.Id;
            var oldPhase = GetById(intId) ?? throw new PhaseNotFoundException();
            oldPhase = oldPhase with
            {
                IdCardTrello = phase.IdCardTrello,
                Title = phase.Title,
                DateOfCreation = phase.DateOfCreation,
                LastActivity = phase.LastActivity,
                Description = phase.Description,
                Deadline = phase.Deadline,
                Status = phase.Status,
                Priority = phase.Priority,
                Scientists = phase.Scientists,
                Experiment = phase.Experiment
            };

            if (phase.Comments != null)
            {
                oldPhase.Comments.Clear();
                oldPhase.Comments.AddRange(phase.Comments);
            }

            _context.Phases.Update(oldPhase);
            _context.SaveChanges();
            return phase;
        }


        public bool Delete(int id)
        {
            var phase = GetById(id);
            _context.Phases.Remove(phase);
            _context.SaveChanges();
            return true;
        }
    }
}
