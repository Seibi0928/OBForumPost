﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OBForumPost.Domain.Repository;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OBForumPost.Persistence.DataBaseContext
{
    public sealed class PostContext : DbContext
    {
        public DbSet<PostEntity> Posts { get; set; }

        public PostContext() { }

        public PostContext(DbContextOptions<PostContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // コンストラクタよりDbContextOptions<PostContext>が注入されている場合はtrue
            if (options.IsConfigured) { return; }

            var server = Environment.GetEnvironmentVariable("DB_SERVERNAME");
            var userId = Environment.GetEnvironmentVariable("DB_USERID");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            options.UseNpgsql($"Server={server};Port={port};Database=post;User Id={userId};Password={password};");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLiteではDateTimeOffsetの比較ができないので、いったん全DateTimeOffsetをUTCのDateTimeへ変換して扱う
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                                || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        builder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }

        internal IOrderedQueryable<PostEntity> GetOrderedPostsQuery(OrderByOptions orderByOption) => orderByOption switch
        {
            OrderByOptions.None => Posts.OrderBy(x => x.PostId),
            OrderByOptions.PostedDateTimeAsc => Posts.OrderBy(x => x.PostedDateTime),
            OrderByOptions.PostedDateTimeDsc => Posts.OrderByDescending(x => x.PostedDateTime),
            OrderByOptions.UpdatedDateTimeAsc => Posts.OrderBy(x => x.UpdatedDateTime),
            OrderByOptions.UpdatedDateTimeDsc => Posts.OrderByDescending(x => x.UpdatedDateTime),
            _ => throw new NotSupportedException(),
        };
    }

    public sealed class PostEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long PostId { get; set; }
        public DateTimeOffset PostedDateTime { get; set; }
        public DateTimeOffset UpdatedDateTime { get; set; }
        public int PostStatus { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }
}
