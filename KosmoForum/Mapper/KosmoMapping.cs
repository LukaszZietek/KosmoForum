using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KosmoForum.Models;
using KosmoForum.Models.Dtos;

namespace KosmoForum.Mapper
{
    public class KosmoMapping : Profile
    {
        public KosmoMapping()
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Category,CategoryCreateDto>().ReverseMap();
            CreateMap<ForumPost, ForumPostDto>().ReverseMap();
        }
    }
}
