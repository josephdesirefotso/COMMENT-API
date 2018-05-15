using Comments.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Services
{
   public interface ICommentRepository
    {
        bool CommentExists(int commentId);
        IEnumerable<Comment> GetComments();
        Comment GetComment(int commentId, bool includeReplies);
        IEnumerable<Reply> GetRepliesForComment(int commentId);
        Reply GetReplyForComment(int commentId, int replyId);
        void AddReplyForComment(int commentId, Reply reply);
        void DeleteReply(Reply reply);
        bool Save();
    }
}
