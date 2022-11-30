using System.Linq.Expressions;
using FBDropshipper.Application.Shared;
using FBDropshipper.Domain.Entities;

namespace FBDropshipper.Application.Categories.Models;

public class CategoryDropDownDto : DropDownDto<int>
{
    
}
public class CategoryDto
{
    
}

public class CategorySelector
{
    public static readonly Expression<Func<Category, CategoryDropDownDto>> SelectorDropDown = p =>
        new CategoryDropDownDto()
        {
            Id = p.Id,
            Name = p.Name
        };
}