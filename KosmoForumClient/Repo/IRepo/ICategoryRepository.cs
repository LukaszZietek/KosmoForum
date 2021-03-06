﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForumClient.Models;

namespace KosmoForumClient.Repo.IRepo
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Tuple<string,Category>> GetAsyncByTitle(string url, string title, string token);
    }
}
