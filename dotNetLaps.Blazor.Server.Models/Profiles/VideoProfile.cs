using AutoMapper;
using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotNetLabs.Blazor.Server.Infrastructure;

namespace dotNetLabs.Blazor.Server.Profiles
{
    public class VideoProfile : Profile
    {

        private readonly EnvironmentOptions _env;

        public VideoProfile(EnvironmentOptions env)
        {
            _env = env;

            CreateMap<Video, VideoDetail>()
                .ForMember(dest => dest.Tags, map => map.MapFrom(v => v.Tags.Select(t => t.Name).ToList()))
                .ForMember(dest => dest.ThumbURL, map => map.MapFrom(v => $"{_env.ApiUrl}/{v.ThumbURL}"));
        }

    }
}
