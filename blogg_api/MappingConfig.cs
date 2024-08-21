using AutoMapper;
using blogg_api.Models;
using blogg_api.Models.DTOs;

namespace blogg_api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<BlogTag, BlogTagCreateDTO>().ReverseMap();
            CreateMap<BlogTag, BlogTagUpdateDTO>().ReverseMap();
            CreateMap<BlogContent, BlogContentCreateDTO>().ReverseMap();
            CreateMap<BlogContent, BlogContentUpdateDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostCreateDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostUpdateDTO>().ReverseMap();
        }
    }
}
