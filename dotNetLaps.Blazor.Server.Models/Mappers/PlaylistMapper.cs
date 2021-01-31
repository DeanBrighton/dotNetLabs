﻿using dotNetLabs.Blazor.Server.Models;
using dotNetLabs.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotNetLabs.Blazor.Server.Mappers
{
    public static class PlaylistMapper
    {
        public static PlaylistDetail ToPlayListDetail(this Playlist playlist,IEnumerable<VideoDetail> playlistVideos=null, bool includeVideos = false)
        {
            return new PlaylistDetail
            {
                Id = playlist.Id,
                Description = playlist.Description,
                Name = playlist.Name,
                Videos = includeVideos ? playlistVideos : null
            };
        }

    }
}
