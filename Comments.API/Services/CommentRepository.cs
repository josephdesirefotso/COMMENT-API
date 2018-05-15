using System;
using System.Collections.Generic;
using System.Linq;
using Comments.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comments.API.Services
{
    public class CommentRepository : ICommentRepository
    {
        private CommentContext _context;
        public CommentRepository(CommentContext context)
        {
            _context = context;
        }

        public void AddReplyForCommment(int commentId, Reply reply)
        {
            // throw new NotImplementedException();
            var comment = GetComment(commentId, false);
            comment.Replies.Add(reply);
        }

        public bool CommentExists(int commentId)
        {
            return _context.Comments.Any(c => c.Id == commentId);
        }

        public IEnumerable<Comment> GetComments()
        {
            return _context.Comments.OrderBy(c => c.Name).ToList();
        }
        public Comment GetComment(int commentId, bool includeReplies)
        {
            if (includeReplies)
            {
                return _context.Comments.Include(c => c.Replies)
                    .Where(c => c.Id == commentId).FirstOrDefault();
            }

            return _context.Comments.Where(c => c.Id == commentId).FirstOrDefault();
         
        }

        public IEnumerable<Reply> GetRepliesForComment(int commentId)
        {
            return _context.Replies
                    .Where(r => r.CommentId == commentId).ToList();
        }

        public Reply GetReplyForComment(int commentId, int replyId)
        {
            return _context.Replies
                    .Where(r => r.CommentId == commentId && r.Id == replyId).FirstOrDefault();
        }


        public void AddReplyForComment(int commentId, Reply reply)
        {
            throw new NotImplementedException();
        }

        public void DeleteReply(Reply reply)
        {
            _context.Replies.Remove(reply);
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
