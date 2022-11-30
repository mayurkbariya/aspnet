using System;
using System.Collections.Generic;
using FBDropshipper.Domain.Interfaces;

namespace FBDropshipper.Domain.Entities
{
    public class Team : Base
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public IEnumerable<TeamMember> TeamMembers { get; set; }
        public IEnumerable<MarketPlace> MarketPlaces { get; set; }
    }

    public class TeamMember : IBase
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool CanLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class TeamMemberPermission : IBase
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int MarketPlaceId { get; set; }
        public MarketPlace MarketPlace { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}