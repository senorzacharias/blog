using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBlogToRestart.Models;
using System.Data.SqlClient;


namespace TheBlogToRestart.Data
{
    public class WarehouseOfStorage : IRepository
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool DoesExsist(int blogId)
        {
            throw new NotImplementedException();
        }

        public void SaveTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetAllTags()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetFive()
        {
            throw new NotImplementedException();
        }

        public Post GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public Post GetRecent()
        {
            throw new NotImplementedException();
        }

        public void Save(Post post)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> Search(int hashTagId)
        {
            throw new NotImplementedException();
        }
    }
}
