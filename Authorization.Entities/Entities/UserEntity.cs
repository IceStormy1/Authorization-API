using Authorization.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

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

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateOnly BirthDay { get; set; }

    /// <inheritdoc cref="Common.Enums.Gender"/>
    public Gender Gender { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    /// <inheritdoc cref="IdentityUserRole{Guid}"/>
    public List<IdentityUserRole<Guid>> Roles { get; set; } = new();
}