using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace dotNetLabs.Blazor.Server.Models
{
    public abstract class UserRecord : Record
    {
        public UserRecord()
        {
            CreationDate = DateTime.UtcNow;
            ModificationDate = DateTime.UtcNow;
        }


        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        //ForeignKey
        public virtual ApplicationUser CreatedByUser { get; set; }
        [ForeignKey(nameof(CreatedByUser))]
        public string CreatedByUserId { get; set; }

        public virtual ApplicationUser ModifiedByUser { get; set; }
        [ForeignKey(nameof(ModifiedByUser))]
        public string ModifiedByUserId { get; set; }


    }
}
