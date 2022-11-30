using System;
using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Users.Models
{
    public class UserDto
    {
        public UserDto()
        {
            
        }

        public UserDto(User user)
        {
            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
            CreatedDate = user.CreatedDate;
            IsEnabled = user.IsEnabled;
        }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class UserSelector
    {
        public static Expression<Func<User, UserDto>> Selector = p => new UserDto()
        {
            Email = p.Email,
            Id = p.Id,
            CreatedDate = p.CreatedDate,
            FullName = p.FullName,
            IsEnabled = p.IsEnabled,
        };
    }
}