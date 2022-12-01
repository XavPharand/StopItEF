using System;
using System.Collections.Generic;

namespace StopItEF.Models
{
    public partial class Vote
    {
        public int VoteId { get; set; }
        public int UserId { get; set; }
        public int LinkId { get; set; }
        public int? Value { get; set; }

        public virtual Link Link { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
