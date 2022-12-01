using System;
using System.Collections.Generic;

namespace StopItEF.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Links = new HashSet<Link>();
            Votes = new HashSet<Vote>();
        }

        public int UserId { get; set; }
        public string Pseudo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
