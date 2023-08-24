using ConcordiaStation.Data.Context;
using ConcordiaStation.Data.Models;
using ConcordiaStation.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConcordiaStation.Data.Repositories
{
    public class RepositoryComment : IRepositoryComment
    {
        private readonly ConcordiaLocalDbContext _context;
        public RepositoryComment(ConcordiaLocalDbContext context) => _context = context;
        public IEnumerable<Comment> GetAll() => _context.Comments.Include(x => x.Scientist) ?? new List<Comment>().AsEnumerable();

        public Comment GetById(int id) => _context.Comments.AsNoTracking().SingleOrDefault(x => x.Id == id);
        public IEnumerable<Comment> GetByPhaseId(int phaseId)
        {
            return _context.Comments
                .Include(c => c.Phase)
                .Include(x => x.Scientist)
                .Where(c => c.Phase.Id == phaseId)
                .ToList();
        }
        public Comment GetByTrelloId(string trelloId)
        {
            throw new NotImplementedException();
        }

        public Comment Insert(Comment comment)
        {
            var newComment = _context.Comments.Add(comment);
            _context.SaveChanges();
            return newComment.Entity;
        }

        public Comment Update(Comment comment)
        {
            var toBeUpdated = _context.Comments.AsNoTracking().SingleOrDefault(x => x.Id == comment.Id);
            _context.Comments.Update(toBeUpdated! with { Content = comment.Content, PublicationDate = DateTime.Now });
            _context.SaveChanges();

            return toBeUpdated;
        }


        public bool Delete(int id)
        {
            var toDelete = _context.Comments.SingleOrDefault(x => x.Id == id);
            if (toDelete is not null)
            {
                _context.Comments.Remove(toDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
