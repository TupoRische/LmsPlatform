using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Contacts
{
    public class ContactFormVm
    {
        [Required(ErrorMessage = "Моля, въведете име")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете имейл")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Моля, въведете съобщение")]
        public string Message { get; set; } = null!;
    }

}
