using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBlogToRestart.Models;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace TheBlogToRestart.Data
{
    public class LiveRepo : IRepository, IDisposable
    {
        BlogContext context = new BlogContext();

        public void Delete(int id)
        {
            Post postQuery;
            postQuery = (from posts in context.Posts select posts).Where(i => i.PostId == id).Single();
            context.Posts.Remove(postQuery);
            context.SaveChanges();

        }

        public void Dispose()
        {
            context.Dispose();
        }

        public bool DoesExsist(int id)
        {
            Post postQuery;

            postQuery = (from posts in context.Posts select posts).FirstOrDefault(i => i.PostId == id);
            if (postQuery == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IEnumerable<Tag> GetAllTags()
        {
            var tagQuery = (from tags in context.Tags select tags).ToList();
            return tagQuery;
        }

        public IEnumerable<Post> GetFive()
        {
            IEnumerable<Post> postQuery;

            postQuery = (from posts in context.Posts select posts).Include("Tags").Where(i => i.Published == true).OrderByDescending(i => i.PostId).Skip(1).Take(4).ToList();

            return postQuery;
        }
        public IEnumerable<Post> GetTen()
        {
            IEnumerable<Post> postQuery;
            postQuery = (from posts in context.Posts select posts).Include("Tags").Where(i => i.Published == true).OrderByDescending(i => i.PostId)/*.Skip(5)*/.Take(10).ToList();

            return postQuery;
        }

        public Post GetOne(int id)
        {
            Post postQuery;

            postQuery = (from posts in context.Posts select posts).FirstOrDefault(i => i.PostId == id);

            return postQuery;
        }

        public Post GetRecent()
        {
            Post postQuery;

            postQuery = (from posts in context.Posts select posts).Include("Tags").Where(i => i.Published == true).OrderByDescending(i => i.PostId).FirstOrDefault();
            return postQuery;
        }

        public void Save(Post post)
        {

            if (post.PostId <= 0)
            {
                var tagQuery = (from tags in context.Tags select tags).ToList();
                List<Tag> listTags = new List<Tag>();
                foreach (var contextTag in tagQuery)
                {
                    if (post.Tags.Any(i => i.TagId == contextTag.TagId))
                    {
                        listTags.Add(contextTag);
                    }
                }
                post.Tags = listTags;
                foreach (var tag in listTags)
                {
                    tag.Posts.Add(post);
                }
                context.Posts.Add(post);
                
                try
                {
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //
            }
            else
            {
                //have to check for working
                var tagQuery = (from tags in context.Tags select tags).ToList();
                List<Tag> listTags = new List<Tag>();
                List<Tag> oldTags = new List<Tag>();
                //var postToEdit = Context.GetData.GetOne(post.PostId);
                var postToEdit = context.Posts.Where(p => p.PostId == post.PostId).FirstOrDefault();
                postToEdit.Address = post.Address;
                postToEdit.Author = post.Author;
                postToEdit.Content = post.Content;
                postToEdit.Description = post.Description;
                postToEdit.EndDate = post.EndDate;
                postToEdit.Image = post.Image;
                postToEdit.PostDate = post.PostDate;
                postToEdit.Published = post.Published;
                postToEdit.Tags = post.Tags;

                foreach (var contextTag in tagQuery)
                {
                    if (post.Tags.Any(t => t.TagId == contextTag.TagId))
                    {
                        listTags.Add(contextTag);
                    }
                }
                postToEdit.Tags.Clear();
                postToEdit.Tags = listTags;
                foreach (var tag in listTags)
                {
                    if (tag.Posts.Any(i => i.PostId == postToEdit.PostId))
                    {

                    }
                    else
                    {
                        tag.Posts.Add(postToEdit);
                    }
                    
                }
                postToEdit.Title = post.Title;
                context.Entry(postToEdit).State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        public void SaveTag(Tag tag)
        {
            using (BlogContext c = new BlogContext())
            {
                var hash = tag.HashTag;
                tag.HashTag = "#" + hash;
                c.Tags.Add(tag);
                c.SaveChanges();
            }
        }

        public IEnumerable<Post> Search(string hashtag)
        {
            var postQuery = (from posts in context.Posts select posts).Where(i => i.Tags.Any(c => c.HashTag == hashtag) && i.Published == true || i.Content.Contains(hashtag) || i.Title.Contains(hashtag)).ToList();
            return postQuery;
        }

        public IEnumerable<Post> GetUnpublishedPosts()
        {
            var postQuery = (from posts in context.Posts select posts).Where(i => i.Published == false);
            return postQuery;
        }

        public IEnumerable<Post> GetPublishedPosts()
        {
            var postQuery = (from posts in context.Posts select posts).Where(i => i.Published == true);
            return postQuery;
        }

        public List<Post> LoadPosts()
        {
            throw new NotImplementedException();
        }
        //public IEnumerable<User> GetUsers()
        //{
        //    var userQuery = (from 
        //}
    }
}
