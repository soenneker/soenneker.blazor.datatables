using Microsoft.Extensions.Logging;
using Soenneker.Blazor.DataTables.Demo.Dtos;
using Soenneker.Utils.Delay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;

namespace Soenneker.Blazor.DataTables.Demo.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly List<UserDto> _users;

    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
        // Generate some demo data
        _users = Enumerable.Range(1, 100)
                           .Select(i => new UserDto
                           {
                               Id = i,
                               Name = $"User {i}",
                               Email = $"user{i}@example.com",
                               Role = i % 3 == 0 ? "Admin" : i % 3 == 1 ? "Editor" : "Viewer",
                               CreatedAt = DateTime.UtcNow.AddDays(-i)
                           })
                           .ToList();
    }

    public async Task<(List<UserDto> Data, int TotalRecords, int TotalFilteredRecords)> GetUsers(int pageNumber, int pageSize, string? orderBy = null,
        string? orderDirection = null, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        // Simulate server delay
        await DelayUtil.Delay(500, _logger, cancellationToken).NoSync();

        IQueryable<UserDto> query = _users.AsQueryable();

        // Apply search if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(u => u.Name.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm) || u.Role.ToLower().Contains(searchTerm));
        }

        // Get total records before filtering
        int totalRecords = _users.Count;

        // Get total records after filtering
        int totalFilteredRecords = query.Count();

        // Apply ordering if provided
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query = orderBy.ToLower() switch
            {
                "id" => orderDirection?.ToLower() == "desc" ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                "name" => orderDirection?.ToLower() == "desc" ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "email" => orderDirection?.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "role" => orderDirection?.ToLower() == "desc" ? query.OrderByDescending(u => u.Role) : query.OrderBy(u => u.Role),
                "createdat" => orderDirection?.ToLower() == "desc" ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
                _ => query
            };
        }

        // Apply pagination
        List<UserDto> data = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return (data, totalRecords, totalFilteredRecords);
    }

    /// <summary>
    /// Gets all users without pagination. Used for continuation token demo.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>All users in the system.</returns>
    public async Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        // Simulate server delay
        await DelayUtil.Delay(100, _logger, cancellationToken).NoSync();

        return _users.ToList();
    }
}