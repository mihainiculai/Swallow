﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models.DatabaseModels
{
    [Index(nameof(PublicUsername), IsUnique = true)]
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? GoogleId { get; set; }
        [MaxLength(100)]
        public string? PublicUsername { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [NotMapped]
        public string FullName { get => $"{FirstName ?? ""} {LastName ?? ""}".Trim(); }
        [Required]
        public bool CustomProfilePicture { get; set; } = false;
        [Required]
        public bool Public { get; set; } = false;
        [MaxLength(255)]
        public string? ProfilePictureURL { get; set; }

        public virtual ICollection<Trip> Trips { get; } = new List<Trip>();
        public virtual ICollection<UserPlan> UserPlans { get; } = new List<UserPlan>();

        public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public virtual ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
        public virtual ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}