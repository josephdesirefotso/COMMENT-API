using Comments.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API
{
    public class CommentsDataStore
    { 
        public static CommentsDataStore Current {get; } = new CommentsDataStore();
        public List<CommentDto> Comments { get; set; }

        public CommentsDataStore()
        {
        // init dummy data
        Comments = new List<CommentDto>()
            {
                new CommentDto()
                {
                    Id = 1,
                    Name = "Paragraph1",
                    Description = "The fonction called is not given the expected result",
                    Replies = new List<ReplyDto>()
                    {
                        new ReplyDto() {
                            Id = 1,
                            Name = "Paragraph1",
                            Description = "The fonction called is not given the expected result",
                        },
                        new ReplyDto() {
                            Id = 2,
                            Name = "Paragraph2",
                            Description = "The compliler is not working properly",
                        },
                         new ReplyDto() {
                            Id = 3,
                            Name = "Paragraph3",
                            Description = "I did not understand the exercise",
                        },
                         new ReplyDto() {
                            Id = 4,
                            Name = "Paragraph4",
                            Description = "I do have a beter way to solve it",
                        },
                         new ReplyDto() {
                            Id = 5,
                            Name = "Paragraph5",
                            Description = "Great tool!!!"
                        },

                    }

                },
                new CommentDto()
        {
            Id = 2,
                    Name = "Paragraph2",
                    Description = "The compliler is not working properly"
                },
                new CommentDto()
        {
            Id = 3,
                    Name = "Paragraph3",
                    Description = "I did not understand the exercise"
                },
                new CommentDto()
        {
            Id = 4,
                    Name = "Paragraph4",
                    Description = "I do have a beter way to solve it"
                },
                new CommentDto()
        {
            Id = 5,
                    Name = "Paragraph5",
                    Description = "Great tool!!!"
                },

            };
}
    }
}

