using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace dotNetLabs.Blazor.Shared
{
    public class VideoDetail
    {

        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string VideoURL { get; set; }

        
        public string ThumbURL { get; set; } // From server to client

        public IFormFile ThumbFile { get; set; } //File from client
        
        
        public int Views { get; set; }

        public int Likes { get; set; }

        public DateTime PublishingDate { get; set; }

        public VideoPrivacy VideoPrivacy { get; set; }

        public Category Category { get; set; }

        //public virtual List<PlaylistVideo> PlaylistVideos { get; set; }

                public virtual List<string> Tags { get; set; }

        public IEnumerable<CommentDetail> Comments { get; set; }


    }
}
