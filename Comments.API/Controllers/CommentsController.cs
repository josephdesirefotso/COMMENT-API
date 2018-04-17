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

        [HttpGet()]
        public IActionResult GetComments()
        {
            return Ok(CommentsDataStore.Current.Comments);

            //var temp = new JsonResult(CommentsDataStore.Current.comments);
            //temp.StatusCode = 200;
            //return temp;
        
         //   return new JsonResult(CommentsDataStore.Current.comments);
            

            //return new JsonResult(new List<object>()
            //    {
            //        new {id=1, Name="Paragraph1" },
            //        new {id=2, Name="Paragraph2" },
            //        new {id=3, Name="Paragraph3" },
            //        new {id=4, Name="Paragraph4" },
            //        new {id=5, Name="Paragraph5" }

            //    });

        }   

        [HttpGet("{id}")]

        public IActionResult GetComment(int id)
        {
            // find comment 
            var commentToReturn = CommentsDataStore.Current.Comments.FirstOrDefault(c => c.Id == id);
            if (commentToReturn == null)
            {
                return NotFound();
            }

            return Ok(commentToReturn);


            //return new JsonResult(
            //    CommentsDataStore.Current.comments.FirstOrDefault(c => c.Id == id)
            //    );
        }
            

    }
}
