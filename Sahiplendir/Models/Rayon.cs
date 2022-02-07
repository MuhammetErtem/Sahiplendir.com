using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class Rayon : NamedBaseEntity
    {
        //NavigationProperty 

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

    }
}
