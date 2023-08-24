using ConcordiaStation.Data.Models;

namespace ConcordiaStation.WebApp.ViewModels
{
    public class AllCommentViewModel
    {
        public AllCommentViewModel(IEnumerable<Comment> comment, IEnumerable<Scientist> scientists)
        {
            Comments = comment;
            Scientists = scientists;

        }
        public AllCommentViewModel() : this(new List<Comment>(), new List<Scientist>())
        {
            
        }
        public IEnumerable<Comment> Comments { get; set; }   
        public IEnumerable<Scientist> Scientists { get; set; }   
    }
}
