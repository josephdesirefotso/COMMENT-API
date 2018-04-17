using Comments.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Models
{
    [Route("api/comments")]
    public class RepliesController : Controller 
    {
        private ILogger<RepliesController> _logger;
        private IMailService _mailService; 

        public RepliesController(ILogger<RepliesController> logger,
            IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService; 
        }

        [HttpGet("{commentId}/replies")]
        public IActionResult GetReplies(int commentId)
        {
            try
            {
                //throw new Exception("Exception sample");

                var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
                if (comment == null)
                {
                    _logger.LogInformation($"Comment with id {commentId} wasn't found when accessing replies.");
                    return NotFound();
                }
                return Ok(comment.Replies);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting replies for comment with id {commentId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
                
                
                // throw;
            }

            //var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
            //if (comment == null)
            //{
            //    _logger.LogInformation($"Comment with id {commentId} wasn't found when accessing replies.");
            //    return NotFound();
            //}
            //return Ok(comment.Replies);
        }

        [HttpGet("{commentId}/replies/{id}", Name = "GetReply")]
        public IActionResult GetReply(int commentId, int id)
        {
            var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var reply = comment.Replies.FirstOrDefault(r => r.Id == commentId);

            if (reply == null)
            {
                return NotFound();
            }

            return Ok(reply);
        }

        [HttpPost("{commentId}/replies")]
        public IActionResult CreateReply(int commentId,
            [FromBody] ReplyForCreationDto reply)
        {
            if (reply == null)
            {
                return BadRequest();
            }

            if (reply.Description == reply.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            // To be improved
            var maxReplyId = CommentsDataStore.Current.Comments.SelectMany(
                c => c.Replies).Max(r => r.Id);

            //int maxReplyId = 0;
            var finalReply = new ReplyDto()
            {
                Id = ++maxReplyId,
                //Id = ++maxReply,
                Name = reply.Name,
                Description = reply.Description
            };

            comment.Replies.Add(finalReply);

            return CreatedAtRoute("GetReply", new
           {
                commentId, id = finalReply.Id}, finalReply);

        }

        [HttpPut("{commentId}/replies/{id}")]
        public IActionResult UpdateReply(int commentId, int id, 
            [FromBody] ReplyForUpdateDto reply)
        {
            if (reply == null)
            {
                return BadRequest();
            }

            if (reply.Description == reply.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var replyFromStore = comment.Replies.FirstOrDefault(p =>
            p.Id == id);

            if (replyFromStore == null)
            {
                return NotFound();
            }

            replyFromStore.Name = reply.Name;
            replyFromStore.Description = reply.Description;

            return NoContent();
        }

        [HttpPatch("{commentId}/replies/{id}")]

        public IActionResult PartiallyUpdateReply(int commentId, int id,
            [FromBody] JsonPatchDocument<ReplyForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var replyFromStore = comment.Replies.FirstOrDefault(p =>
            p.Id == id);

            if (replyFromStore == null)
            {
                return NotFound();
            }

            var replyToPatch =
                new ReplyForUpdateDto()
                {
                    Name = replyFromStore.Name,
                    Description = replyFromStore.Description
                };

            patchDoc.ApplyTo(replyToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (replyToPatch.Description == replyToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(replyToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            replyFromStore.Name = replyToPatch.Name;
            replyFromStore.Description = replyToPatch.Description;

            return NoContent();

        }

        [HttpDelete("{commentId}/replies/{id}")]
        public IActionResult DeleteReply(int commentId, int id)
        {
            var comment = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var replyFromStore = comment.Replies.FirstOrDefault(p => p.Id == id);
            if (replyFromStore == null)
            {
                return NotFound();
            }

            comment.Replies.Remove(replyFromStore);
            _mailService.Send("Reply deleted.",
                $"Reply {replyFromStore.Name} with id {replyFromStore.Id} was deleted.");


            return NoContent();
        
        }
    }
}

