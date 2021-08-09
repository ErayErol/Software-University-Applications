namespace MiniFootball.Data.Models
{
    using System.Collections.Generic;

    public class Country
    {
        public int Id { get; init; }
        
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; init; } = new HashSet<City>();
        
        public virtual ICollection<Field> Fields { get; init; } = new HashSet<Field>();
    }
}
