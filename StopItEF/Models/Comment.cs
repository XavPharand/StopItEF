using System;
using System.Collections.Generic;

namespace StopItEF.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? UserId { get; set; }
        public int? LinkId { get; set; }
        public string? Text { get; set; }
        public DateOnly? PublicationDate { get; set; }

        public virtual Link? Link { get; set; }
        public virtual User? User { get; set; }
    }
}
