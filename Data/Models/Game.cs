namespace MessiFinder.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        [Key]
        public string Id { get; set; }

        public int PlaygroundId { get; set; }
        public virtual Playground Playground { get; set; }

        public DateTime Date { get; set; }
    }
}
