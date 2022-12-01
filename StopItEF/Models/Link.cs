using System;
using System.Collections.Generic;

namespace StopItEF.Models
{
    public partial class Link
    {
        public Link()
        {
            Comments = new HashSet<Comment>();
            Votes = new HashSet<Vote>();
        }

        public int LinkId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateOnly? PublicationDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
