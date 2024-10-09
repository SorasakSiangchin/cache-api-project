using AutoMapper;
using CacheProject.Dtos;
using CacheProject.Models;

namespace CacheProject;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
    }
}