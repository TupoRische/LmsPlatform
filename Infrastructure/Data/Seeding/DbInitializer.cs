using Infrastructure.Data.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeding
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // Roles
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // Admin user
            var adminEmail = "admin@lms.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsApproved = true,
                    CreatedOn = DateTime.UtcNow
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }


            // 1) SCHOOLS
            if (!context.Schools.Any())
            {
                var schools = new[]
                {
        new School { Name = "Природо-математическа гимназия \"Иван Вазов\"", City = "Добрич" },
        new School { Name = "Финансово-стопанска гимназия \"Васил Левски\"", City = "Добрич" },
        new School { Name = "Средно училище \"Свети Климент Охридски\"", City = "Добрич" },
        new School { Name = "Професионална гимназия по ветеринарна медицина \"Проф. д-р Г. Павлов\"", City = "Добрич" },
        new School { Name = "Професионална гимназия по туризъм \"П. К. Яворов\"", City = "Добрич" },

        // Новите училища от разпределението:
        new School { Name = "Професионална гимназия по компютърно моделиране и компютърни системи \"Академик Благовест Сендов\"", City = "Варна" },
        new School { Name = "Професионална гимназия по електротехника", City = "Варна" },
        new School { Name = "Свищовска професионална гимназия \"Алеко Константинов\"", City = "Свищов" },
        new School { Name = "Професионална гимназия по строителство, архитектура и геодезия „Васил Левски“", City = "Варна" }
    };

                context.Schools.AddRange(schools);
                await context.SaveChangesAsync();
            }

            // helper за намиране на SchoolId по Name + City
            int GetSchoolId(string name, string city) =>
                context.Schools
                    .Where(s => s.Name == name && s.City == city)
                    .Select(s => s.Id)
                    .First();

            // 2) PROFESSIONS (с връзка към училище)
            if (!context.Professions.Any())
            {
                var map = new (string Profession, string SchoolName, string City)[]
                {
                    ("Графичен дизайнер", "Средно училище \"Свети Климент Охридски\"", "Добрич"),
                    ("Оперативен счетоводител", "Финансово-стопанска гимназия \"Васил Левски\"", "Добрич"),
                    ("Приложен програмист", "Природо-математическа гимназия \"Иван Вазов\"", "Добрич"),
                    ("Системен програмист", "Професионална гимназия по компютърно моделиране и компютърни системи \"Академик Благовест Сендов\"", "Варна"),
                    ("Електронна търговия", "Финансово-стопанска гимназия \"Васил Левски\"", "Добрич"),
                    ("Електротехник", "Професионална гимназия по електротехника", "Варна"),
                    ("Техник на комуникационни системи", "Професионална гимназия по електротехника", "Варна"),
                    ("Техник по транспортна техника", "Свищовска професионална гимназия \"Алеко Константинов\"", "Свищов"),
                    ("Строителен техник", "Професионална гимназия по строителство, архитектура и геодезия „Васил Левски“", "Варна"),
                    ("Ветеринарен техник", "Професионална гимназия по ветеринарна медицина \"Проф. д-р Г. Павлов\"", "Добрич"),
                    ("Готвач", "Професионална гимназия по туризъм \"П. К. Яворов\"", "Добрич"),
                    ("Хотелиер", "Професионална гимназия по туризъм \"П. К. Яворов\"", "Добрич"),
                };

                foreach (var (profession, schoolName, city) in map)
                {
                    var schoolId = GetSchoolId(schoolName, city);

                    context.Professions.Add(new Profession
                    {
                        Name = profession,
                        SchoolId = schoolId
                    });
                }

                await context.SaveChangesAsync();
            }

        }

    }
}
