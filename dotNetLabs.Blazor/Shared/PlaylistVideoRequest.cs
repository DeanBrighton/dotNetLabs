using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dotNetLabs.Blazor.Shared
{
    public class PlaylistVideoRequest
    {
        [Required]
        public  string  PlaylistId { get; set; }
        [Required]
        public string VideoId { get; set; }

    }
}
