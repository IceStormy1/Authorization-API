using Authorization.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI;

public class RegisterInputModel : BaseAccountModel
{
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// Имя 
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия 
    /// </summary>
    [Required]
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
    /// Email
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    [DataType(DataType.Date)]
    public DateOnly BirthDay { get; set; }

    /// <inheritdoc cref="Authorization.Common.Enums.Gender"/>
    public Gender Gender { get; set; }
}