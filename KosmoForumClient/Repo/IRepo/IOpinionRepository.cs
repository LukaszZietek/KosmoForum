﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;

namespace KosmoForumClient.Repo.IRepo
{
    public interface IOpinionRepository : IRepository<Opinion>
    {
        Task<Tuple<string,IEnumerable<Opinion>>> GetUserOpinion(string url, string token);
    }
}
