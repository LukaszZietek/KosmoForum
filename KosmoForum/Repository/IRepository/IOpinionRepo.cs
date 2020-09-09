using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;

namespace KosmoForum.Repository.IRepository
{
    public interface IOpinionRepo
    {
        Opinion GetOpinion(int id);
        ICollection<Opinion> GetAllOpinionsForUser(int userId);
        ICollection<Opinion> GetAllOpinionsInPost(int forumPostId);
        ICollection<Opinion> GetAllOpinions();
        bool UpdateOpinion(Opinion obj);
        bool DeleteOpinion(Opinion obj);
        bool CreateOpinion(Opinion obj);
        bool OpinionIfExist(int id);
        bool Save();
    }
}
