using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using dotNetLabs.Blazor.Shared;

namespace dotNetLabs.Blazor.Server.Models
{
    public class Video: UserRecord
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string VideoURL { get; set; }

        [Required]
        [StringLength(255)]
        public string ThumbURL { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public DateTime PublishingDate { get; set; }

        public VideoPrivacy VideoPrivacy { get; set; }

        public Category Category { get; set; }

        public virtual List<PlaylistVideo> PlaylistVideos { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Tag> Tags { get; set; }
    }
}
