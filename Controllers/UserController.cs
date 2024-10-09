using AutoMapper;
using CacheProject.Data;
using CacheProject.Dtos;
using CacheProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CacheProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public UserController(DataContext context, IMemoryCache cache , IMapper mapper)
    {
        _context = context;
        _cache = cache;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        string cacheKey = "users";
        List<User> users;

        // ตรวจสอบว่ามีข้อมูลใน cache หรือไม่
        var cacheData = _cache.Get<List<User>>(cacheKey);

        if (cacheData != null)
        {
            return Ok(_mapper.Map<List<UserDto>>(cacheData).Select(r =>
            {
                r.IsCache = true;
                return r;
            })); 
        }
        
        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        users = await _context.Users.ToListAsync();
        _cache.Set(cacheKey, users, expirationTime);
        
        return Ok(_mapper.Map<List<UserDto>>(users).Select(r =>
        {
            r.IsDb= true;
            return r;
        })); 
        
        // if (!_cache.TryGetValue(cacheKey, out users))
        // {
        //     // หากไม่มีใน cache ให้ดึงข้อมูลจาก database
        //     users = await _context.Users.ToListAsync();
        //
        //     // เก็บข้อมูลลงใน cache พร้อมกำหนดระยะเวลาหมดอายุ
        //     var cacheEntryOptions = new MemoryCacheEntryOptions()
        //         .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        //     _cache.Set(cacheKey, users, cacheEntryOptions);
        // }

        
    }
}