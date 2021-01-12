using System;
using System.Collections.Generic;
using System.Text;

namespace dotNetLabs.Blazor.Server.Models
{
    public class PlaylistVideo: Record
    {


        public virtual Video Video { get; set; }

        public string VideoID  { get; set; }

        public virtual Playlist Playlist { get; set; }

        public string PlaylistId { get; set; }



    }
}
