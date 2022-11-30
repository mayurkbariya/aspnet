using System;
using System.Linq.Expressions;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.MarketPlaces.Models
{
    public class MarketPlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public int MarketPlaceType { get; set; }
        public DateTime CreatedDate { get; set; }

        public MarketPlaceDto(MarketPlace place)
        {
            Id = place.Id;
            Name = place.Name;
            TeamId = place.TeamId;
            MarketPlaceType = place.MarketPlaceType;
            CreatedDate = place.CreatedDate;
        }

        public MarketPlaceDto()
        {
        }
    }

    public class MarketPlaceSelector
    {
        public static Expression<Func<MarketPlace, MarketPlaceDto>> Selector = p => new MarketPlaceDto()
        {
            Id = p.Id,
            Name = p.Name,
            CreatedDate = p.CreatedDate,
            TeamId = p.TeamId,
            MarketPlaceType = p.MarketPlaceType
        };
    }
    
}