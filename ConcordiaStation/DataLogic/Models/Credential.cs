using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcordiaStation.Data.Models
{
    public record Credential(int? Id = default, string Email = "", string Password = "") : Entity(Id)
    {
        public Scientist Scientist { get; init; } = default;
    }
}
