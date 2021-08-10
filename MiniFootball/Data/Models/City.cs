namespace MiniFootball.Data.Models
{
    using System.Collections.Generic;

    public class City
    {
        public int Id { get; init; }
        
        public string Name { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int AdminId { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual ICollection<Field> Fields { get; init; } = new HashSet<Field>();
    }
}
