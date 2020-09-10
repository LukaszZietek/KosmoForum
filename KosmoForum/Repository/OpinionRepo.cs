using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.DbContext;
using KosmoForum.Models;
using KosmoForum.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace KosmoForum.Repository
{
    public class OpinionRepo : IOpinionRepo
    {
        private readonly ApplicationDbContext _db;

        public OpinionRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public Opinion GetOpinion(int id)
        {
            var obj = _db.Opinions.FirstOrDefault(x => x.Id == id);
            return obj;
        }

        public ICollection<Opinion> GetAllOpinionsForUser(int userId)
        {
            var values = _db.Opinions.Where(x => x.UserId == userId).ToList();
            return values;
        }

        public ICollection<Opinion> GetAllOpinionsInPost(int forumPostId)
        {
            //var values = _db.Opinions.Where(x => x.ForumPostId == forumPostId)
            //    .Include(x => x.User).ToList();
            var values = _db.Opinions
                .Where(x => x.ForumPostId == forumPostId).ToList();

            return values;
        }

        public ICollection<Opinion> GetAllOpinions()
        {
            var values = _db.Opinions.OrderBy(x => x.Id).ToList();
            return values;
        }

        public bool UpdateOpinion(Opinion obj)
        {
            _db.Opinions.Update(obj);
            return Save();
        }

        public bool DeleteOpinion(Opinion obj)
        {
            _db.Opinions.Remove(obj);
            return Save();
        }

        public bool CreateOpinion(Opinion obj)
        {
            _db.Opinions.Add(obj);
            return Save();
        }

        public bool OpinionIfExist(int id)
        {
            var value = _db.Opinions.Any(x => x.Id == id);
            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
