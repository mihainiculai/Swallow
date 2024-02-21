﻿using Swallow.DTOs.Authentication;
using Swallow.Models.DatabaseModels;

namespace Swallow.Utils.Authentication
{
    public class UserResponse
    {
        public static UserDto Create(User user)
        {
            return new UserDto
            {
                Username = user.PublicUsername,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                ProfilePictureURL = user.ProfilePictureURL,
                Public = user.Public 
            };
        }
    }
}