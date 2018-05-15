using Comments.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comments.API.Models;


namespace Comments.API
{
    public static class CommentExtensions
    {
        public static void EnsureSeedDataForContext(this CommentContext context)
        {
            if (context.Comments.Any())
            {
                return;
            }

            // init seed data 
            var comments = new List<Comment>()
            {
                new Comment()
                {
                    Name = "Paragraph1",
                    Description = "The fonction called is not given the expected result.",
                    Seen = false,
                    Replies = new List<Reply>()
                    {
                                new Reply() {
                                Name = "Paragraph2",
                                Seen = false,
                                Description = "The compliler is not working properly."
                            },
                            new Reply() {
                                Name = "Paragraph3",
                                Seen = false,
                                Description = "I did not understand the exercise."
                            },
                    }
                        
                },
                new Comment()
                {
                    Name = "Paragraph2",
                    Description = "The compliler is not working properly",
                    Seen = false,
                    Replies = new List<Reply>()
                    {
                        new Reply()
                        {
                            Name = "Paragraph2",
                            Seen = false,
                            Description = "Restart VS."
                        },
                        new Reply() {
                            Name = "Paragraph3",
                            Seen = false,
                            Description = "An explicit conversions exists, are you missing a cast?"
                            },
                    }
                }
           };  

            context.Comments.AddRange(comments);
            context.SaveChanges();
        }
    }
}
