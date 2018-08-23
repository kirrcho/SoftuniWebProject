using AutoMapper;
using MyAnimeWorld.Common.Admin.BindingModels;
using MyAnimeWorld.Common.Animes.ViewModels;
using MyAnimeWorld.Common.Main.ViewModels;
using MyAnimeWorld.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Common.Utilities.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<AddAnimeBindingModel, AnimeSeries>().ForMember(p => p.Categories,options => options.Ignore());
            this.CreateMap<AnimeSeries, AnimeDetailsViewModel>();
            this.CreateMap<AnimeSeries, AnimeSeriesViewModel>().ForMember(p => p.AnimeSeriesId, options => options.MapFrom(p => p.Id));
            this.CreateMap<Comment, CommentViewModel>()
                .ForMember(p => p.Username, options => options.MapFrom(p => p.User.UserName))
                .ForMember(p => p.Content, options => options.MapFrom(p => p.CommentContent))
                .ForMember(p => p.AvatarUrl, options => options.MapFrom(p => p.User.AvatarUrl));
        }
    }
}
