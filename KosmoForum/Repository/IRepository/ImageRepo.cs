using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.DbContext;

namespace KosmoForum.Repository.IRepository
{
    public class ImageRepo : IImageRepo
    {
        private readonly ApplicationDbContext _db;

        public ImageRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool DeleteImage(int id)
        {
            var obj = _db.Images.FirstOrDefault(x => x.Id == id);

            _db.Images.Remove(obj);

            return Save();
        }

        public bool ifExists(int id)
        {
            return _db.Images.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
