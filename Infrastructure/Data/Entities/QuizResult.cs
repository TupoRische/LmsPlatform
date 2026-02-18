using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities
{
    public class QuizResult
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Ако въпросникът е за логнати потребители:
        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        // Препоръчана професия
        public int RecommendedProfessionId { get; set; }
        public Profession RecommendedProfession { get; set; } = null!;

        // Запазваме отговорите като JSON (лесно и чисто)
        [Required]
        public string AnswersJson { get; set; } = "{}";
    }

}
