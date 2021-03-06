﻿using OBFormPost.Application.ViewModel.PostLists;
using OBForumPost.Domain.PostLists;
using OBForumPost.Domain.Shared;
using System;
using Xunit;

namespace OBFormPost.Application.Test.ViewModel
{
    public sealed class PostViewModelTest
    {
        public sealed class CreateFromPostTest
        {
            [Fact]
            public void PostからViewModelを生成できること()
            {
                var domainModel = new Post
                {
                    Id = 100,
                    PostedDateTime = DateTimeOffset.Parse("2019/03/22 15:00 +9:00"),
                    UpdatedDateTime = DateTimeOffset.Parse("2021/09/22 15:00 +9:00"),
                    Author = new Author(),
                    PostStatus = PostStatus.Open,
                    Title = "たいとる"
                };
                var viewModel = PostViewModel.CreateFromPost(domainModel);

                Assert.Equal(domainModel.Id, viewModel.Id);
                Assert.Equal(domainModel.PostedDateTime.DateTime, viewModel.PostedDateTime);
                Assert.Equal(domainModel.UpdatedDateTime.DateTime, viewModel.UpdatedDateTime);
                Assert.Equal(domainModel.PostStatus, viewModel.Status);
                Assert.Equal(domainModel.Title, viewModel.Title);
            }
        }
    }
}
