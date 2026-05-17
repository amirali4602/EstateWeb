using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category entity)
    {
        return new CategoryDto { Id = entity.Id, Name = entity.Name };
    }

    public static Category ToEntity(this CategoryDto dto)
    {
        return new Category { Id = dto.Id, Name = dto.Name };
    }
}
