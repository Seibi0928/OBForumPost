﻿using System;
using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Shared;

namespace OBFormPost.Application.ViewModel.PostLists
{
    public sealed class PostViewModel
    {
        public long Id { get; init; }
        public DateTime PostedDateTime { get; init; }
        public DateTime UpdatedDateTime { get; init; }
        public PostStatus Status { get; init; }
        public string Title { get; init; }

        public static PostViewModel CreateFromPost(Post post)
        {
            return new PostViewModel
            {
                Id = post.Id,
                PostedDateTime = post.PostedDateTime.DateTime,
                UpdatedDateTime = post.UpdatedDateTime.DateTime,
                Status = post.PostStatus,
                Title = post.Title
            };
        }
    }
}
