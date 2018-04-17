using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       //public IEnumerable<ReplyDto> Replies { get; set; }
       public int NumberOfReplies { get
            {
                return Replies.Count;
            }
                
       }

        public ICollection<ReplyDto> Replies { get; set; }
        = new List<ReplyDto>();
    }
}
