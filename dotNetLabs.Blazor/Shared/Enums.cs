using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetLabs.Blazor.Shared
{
    public enum VideoPrivacy
    {
        Public = 1, //Everyone
        Unlisted = 2, // Only with URL
        Private = 3 //Noone
    }

    public enum Category
    {
        Education = 1,
        Sport = 2,
        Space = 3,
        Entertainment = 4,
        Music = 5
    }


}
