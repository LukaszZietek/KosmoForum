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
            //--------------------------CATEGORY------------------------------------------------
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Category,CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            //----------------------------------------------------------------------------------


            //--------------------------FORUM POST----------------------------------------------
            CreateMap<ForumPost, ForumPostDto>().ReverseMap();
            CreateMap<ForumPost, ForumPostCreateDto>().ReverseMap();
            CreateMap<ForumPost, ForumPostUpdateDto>().ReverseMap();
            //----------------------------------------------------------------------------------



            //--------------------------Opinion-------------------------------------------------
            CreateMap<Opinion, OpinionDto>().ReverseMap();
            CreateMap<Opinion, OpinionCreateDto>().ReverseMap();
            CreateMap<Opinion, OpinionUpdateDto>().ReverseMap();
            //----------------------------------------------------------------------------------

            //--------------------------Image---------------------------------------------------
            CreateMap<Image, ImageDto>().ReverseMap();
            CreateMap<Image, ImageCreateDto>().ReverseMap();

            //----------------------------------------------------------------------------------
        }
    }
}
