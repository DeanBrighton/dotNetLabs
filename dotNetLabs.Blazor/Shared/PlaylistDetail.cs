using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dotNetLabs.Blazor.Shared
{
    public class PlaylistDetail
    {
        public string Id { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        //public IEnumerable<VideoDetail> Videos { get; set; }


    }
}
