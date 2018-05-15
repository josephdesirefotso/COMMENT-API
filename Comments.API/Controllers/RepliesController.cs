using AutoMapper;
using Comments.API.Models;
using Comments.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Controllers
{
    [Route("api/comments")]
    public class RepliesController : Controller 
    {
        private ILogger<RepliesController> _logger;
        private IMailService _mailService;
        private ICommentRepository _commentRepository;
       // private ICommentRepository commentRepository;

        public RepliesController(ILogger<RepliesController> logger,
            IMailService mailService,
            ICommentRepository commentRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _commentRepository = commentRepository;

        }

        [HttpGet("{commentId}/replies")]
        public IActionResult GetReplies(int commentId)
        {
            try
            {
 
                if (!_commentRepository.CommentExists(commentId))
                {
                    _logger.LogInformation($"Comment with id {commentId} was not found when accessing replies.");
                    return NotFound();
                }

                var repliesForComment = _commentRepository.GetRepliesForComment(commentId);
                var repliesForCommentResults =
                                Mapper.Map<IEnumerable<ReplyDto>>(repliesForComment);

                return Ok(repliesForCommentResults);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting replies for comment with id {commentId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");

            }

        }

        [HttpGet("{commentId}/replies/{id}", Name = "GetReply")]
        public IActionResult GetReply(int commentId, int id)
        {
            if (!_commentRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var reply = _commentRepository.GetReplyForComment(commentId, id);

            if (reply == null)
            {
                return NotFound();
            }

            var replyResult = Mapper.Map<ReplyDto>(reply);

            return Ok(replyResult);

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

            if (!_commentRepository.CommentExists(commentId))
            {
                return NotFound();
            }
            var finalReply = Mapper.Map<Entities.Reply>(reply);
            _commentRepository.AddReplyForComment(commentId, finalReply);
            
            if (!_commentRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //comment.Replies.Add(finalReply);
            var createdReplyToReturn = Mapper.Map<Models.ReplyDto>(finalReply);

            return CreatedAtRoute("GetReply", new
            { commentId = commentId, id = createdReplyToReturn.Id }, createdReplyToReturn);

               //commentId, id = finalReply.Id }, finalReply);
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

            if (!_commentRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var replyEntity = _commentRepository.GetReplyForComment(commentId, id);
            if (replyEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(reply, replyEntity);

            if (!_commentRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

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

            if (!_commentRepository.CommentExists(commentId))
            {
                return NotFound();
            }

            var replyEntity = _commentRepository.GetReplyForComment(commentId, id);
            if (replyEntity == null)
            {
                return NotFound();
            }
            var replyToPatch = Mapper.Map<ReplyForUpdateDto>(replyEntity);

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

            Mapper.Map(replyToPatch, replyEntity);

            if (!_commentRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            
            return NoContent();

        }

        [HttpDelete("{commentId}/replies/{id}")]
        public IActionResult DeleteReply(int commentId, int id)
        {
            if (!_commentRepository.CommentExists(commentId))
            {
                return NotFound();
            }
            
            var replyEntity = _commentRepository.GetReplyForComment(commentId, id);
            if (replyEntity == null)

            {
                return NotFound();
            }

            _commentRepository.DeleteReply(replyEntity);
            if (!_commentRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //comment.Replies.Remove(replyFromStore);
            _mailService.Send("Reply deleted.",
                $"Reply {replyEntity.Name} with id {replyEntity.Id} was deleted.");


            return NoContent();
        
        }
    }
}

