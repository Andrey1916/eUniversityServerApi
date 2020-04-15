using System;
using System.ComponentModel.DataAnnotations;

namespace eUniversityServer.Models.BindingModels
{
    public class SignInBindingModel
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
    }

    public class SignUpBindingModel
    {
        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Patronymic { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstNameEng { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastNameEng { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        [MaxLength(256)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }

    public class UserBindingModel : IBindingModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Patronymic { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstNameEng { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastNameEng { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(16)]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
