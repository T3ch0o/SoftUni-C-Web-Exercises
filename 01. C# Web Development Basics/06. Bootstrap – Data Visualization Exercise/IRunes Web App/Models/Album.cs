namespace IRunes.Models
{
    using System.Collections;
    using System.Collections.Generic;

    public class Album : BaseModel<string>
    {
        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<Track> Tracks { get; set; } = new HashSet<Track>();
    }
}