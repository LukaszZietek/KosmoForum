using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Repository.IRepository
{
    public interface IImageRepo
    {
        bool DeleteImage(int id);

        bool ifExists(int id);

        bool Save();
    }
}
