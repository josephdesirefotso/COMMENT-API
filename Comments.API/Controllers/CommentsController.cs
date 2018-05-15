using AutoMapper;
using Comments.API.Entities;
using Comments.API.Models;
using Comments.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Controllers
{
    [Route("api/comments")]
    public class CommentsController : Controller
    {
        private ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }


        [HttpGet()]
        public IActionResult GetComments()
        {

            var commentEntities = _commentRepository.GetComments();

            var results = Mapper.Map<IEnumerable<CommentWithoutRepliesDto>>(commentEntities);

            return Ok(results);

    }

        [HttpGet("{id}")]

        public IActionResult GetComment(int id, bool includeReplies = false)
        {
            var comment = _commentRepository.GetComment(id, includeReplies);
            if (comment == null)
            {
                return NotFound();
            }
            if (includeReplies)
            {
                var commentResult = Mapper.Map<CommentDto>(comment);
                return Ok(commentResult);

            }


            var commentWithoutRepliesResult = Mapper.Map<CommentWithoutRepliesDto>(comment);
            return Ok(commentWithoutRepliesResult);


               
        }

        //[HttpPut("{commentId}/{id}")]
        //public IActionResult UpdateComments(int commentId, int id,
        //[FromBody] CommentDto comment)
        //{
        //    if (comment == null)
        //    {
        //        return BadRequest();
        //    }

        //    if (comment.Description == comment.Name)
        //    {
        //        ModelState.AddModelError("Description", "The provided description should be different from the name.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var comments = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == commentId);

        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    var repliesFromStore = comment.Replies.FirstOrDefault(r =>
        //    r.Id == id);

        //    if (replyFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    commentFromStore.Name = comment.Name;
        //    commentFromStore.Description = comment.Description;

        //    return NoContent();
        //}

        [HttpPatch("{commentId}/{id}")]
        public IActionResult PartiallyUpdateComment(int commentId, int id,
            [FromBody] JsonPatchDocument<CommentForUpdateDto> patchDoc)
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

            var commentFromStore = comment.Replies.FirstOrDefault(c => c.Id == id);
            if (commentFromStore == null)
            {
                return NotFound();
            }

            var commentToPatch =
                   new CommentForUpdateDto()
                   {
                       Name = commentFromStore.Name,
                       Description = commentFromStore.Description
                   };

            patchDoc.ApplyTo(commentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentToPatch.Description == commentToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(commentToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            commentFromStore.Name = commentToPatch.Name;
            commentFromStore.Description = commentToPatch.Description;

            return NoContent();
        }


    }
}
