using Comments.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Controllers
{
    public class DummyController : Controller
    {
        private CommentContext _ctx; 

        public DummyController(CommentContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/testdatabase")]

        public IActionResult TestDatabase()
        {
            return Ok();
        }

    }
}
