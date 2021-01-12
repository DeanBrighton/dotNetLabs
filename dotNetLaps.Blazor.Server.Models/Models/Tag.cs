using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace dotNetLabs.Blazor.Server.Models
{
    public class Tag : Record
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        public virtual Video Video{get;set;}
        [ForeignKey(nameof(Video))]
        public string VideoId { get; set; }

    }
}
