using System;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Entities.Entities;

public class UserEntity : IdentityUser<Guid>, IHasCreatedAt, IHasUpdatedAt
{
    /// <summary>
    /// Имя 
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия 
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// СНИЛС
    /// </summary>
    public string Snils { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }
}