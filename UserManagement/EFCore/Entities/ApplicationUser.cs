﻿using Microsoft.AspNetCore.Identity;

namespace EFCore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] ProfilePicture { get; set; }
    }
}