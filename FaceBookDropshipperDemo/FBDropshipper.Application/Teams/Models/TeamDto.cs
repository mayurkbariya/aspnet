using System;
using System.Linq.Expressions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Teams.Models
{
    public class TeamDto
    {
        public TeamDto()
        {
            
        }

        public TeamDto(Team team)
        {
            Id = team.Id;
            Name = team.Name;
            CreatedDate = team.CreatedDate;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public static class TeamSelector
    {
        public static readonly Expression<Func<Team, TeamDto>> Selector = p => new TeamDto()
        {
            Id = p.Id,
            Name = p.Name,
            CreatedDate = p.CreatedDate
        };
    }
}