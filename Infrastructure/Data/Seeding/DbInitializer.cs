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
                    new School
                    {
                        Name = "Природо-математическа гимназия \"Иван Вазов\"",
                        City = "Добрич",
                        Description = "Профилирана гимназия с фокус върху математика, информатика и природни науки."
                    },
                    new School
                    {
                        Name = "Финансово-стопанска гимназия \"Васил Левски\"",
                        City = "Добрич",
                        Description = "Професионално обучение в областта на икономика, счетоводство и бизнес администрация."
                    },
                    new School
                    {
                        Name = "Средно училище \"Свети Климент Охридски\"",
                        City = "Добрич",
                        Description = "Общообразователно училище с разнообразни профили и извънкласни дейности."
                    },
                    new School
                    {
                        Name = "Професионална гимназия по ветеринарна медицина \"Проф. д-р Г. Павлов\"",
                        City = "Добрич",
                        Description = "Специализирано обучение по ветеринарна медицина и селско стопанство."
                    },
                    new School
                    {
                        Name = "Професионална гимназия по туризъм \"П. К. Яворов\"",
                        City = "Добрич",
                        Description = "Подготовка на кадри в туризма, ресторантьорството и хотелиерството."
                    },
                    new School
                    {
                        Name = "Професионална гимназия по компютърно моделиране и компютърни системи \"Академик Благовест Сендов\"",
                        City = "Варна",
                        Description = "Обучение в сферата на софтуерните технологии, програмирането и компютърните системи."
                    },
                    new School
                    {
                        Name = "Професионална гимназия по електротехника",
                        City = "Варна",
                        Description = "Професионално образование в областта на електротехниката, автоматизацията и електрониката."
                    },
                    new School
                    {
                        Name = "Свищовска професионална гимназия \"Алеко Константинов\"",
                        City = "Свищов",
                        Description = "Професионална подготовка в технически и икономически специалности."
                    },
                    new School
                    {
                        Name = "Професионална гимназия по строителство, архитектура и геодезия „Васил Левски“",
                        City = "Варна",
                        Description = "Специализирано обучение в строителството, архитектурата и геодезията."
                    }
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
                var map = new (string Profession, string Description, string SchoolName, string City)[]
                {
                    ("Графичен дизайнер",
                        "Създава визуални материали: лого, плакати, брошури, банери и дизайн за социални мрежи.",
                        "Средно училище \"Свети Климент Охридски\"", "Добрич"),

                    ("Оперативен счетоводител",
                        "Обработва първични документи, фактури, каса/банка и подпомага счетоводното отчитане.",
                        "Финансово-стопанска гимназия \"Васил Левски\"", "Добрич"),

                    ("Приложен програмист",
                        "Разработва уеб/десктоп приложения, работи с бази данни и поддръжка на софтуер.",
                        "Природо-математическа гимназия \"Иван Вазов\"", "Добрич"),

                    ("Системен програмист",
                        "Работи със системен софтуер, мрежи, конфигурация и поддръжка на компютърни системи.",
                        "Професионална гимназия по компютърно моделиране и компютърни системи \"Академик Благовест Сендов\"", "Варна"),

                    ("Електронна търговия",
                        "Организира онлайн продажби, управление на продукти, поръчки, маркетинг и обслужване на клиенти.",
                        "Финансово-стопанска гимназия \"Васил Левски\"", "Добрич"),

                    ("Електротехник",
                        "Монтаж и поддръжка на електроинсталации, табла, измервания и безопасност.",
                        "Професионална гимназия по електротехника", "Варна"),

                    ("Техник на комуникационни системи",
                        "Инсталира и поддържа комуникационни мрежи, устройства и кабелни системи.",
                        "Професионална гимназия по електротехника", "Варна"),

                    ("Техник по транспортна техника",
                        "Диагностика и поддръжка на транспортна техника, основни ремонти и технически прегледи.",
                        "Свищовска професионална гимназия \"Алеко Константинов\"", "Свищов"),

                    ("Строителен техник",
                        "Организация на строителни дейности, измервания, документация и контрол на качеството.",
                        "Професионална гимназия по строителство, архитектура и геодезия „Васил Левски“", "Варна"),

                    ("Ветеринарен техник",
                        "Подпомага ветеринарната дейност: грижа за животни, манипулации, хигиена и документация.",
                        "Професионална гимназия по ветеринарна медицина \"Проф. д-р Г. Павлов\"", "Добрич"),

                    ("Готвач",
                        "Приготвя храна по технологични рецепти, организация на кухня и контрол на качеството.",
                        "Професионална гимназия по туризъм \"П. К. Яворов\"", "Добрич"),

                    ("Хотелиер",
                        "Работа с гости, резервации, настаняване, обслужване и организация на хотелския процес.",
                        "Професионална гимназия по туризъм \"П. К. Яворов\"", "Добрич"),
                };

                foreach (var (profession, description, schoolName, city) in map)
                {
                    var schoolId = GetSchoolId(schoolName, city);

                    context.Professions.Add(new Profession
                    {
                        Name = profession,
                        Description = description,
                        SchoolId = schoolId
                    });
                }

                await context.SaveChangesAsync();
            }


            //Материали
            if (!context.MaterialCategories.Any())
            {
                context.MaterialCategories.AddRange(
                    new MaterialCategory { Name = "Теория" },
                    new MaterialCategory { Name = "Практика" },
                    new MaterialCategory { Name = "Презентации" },
                    new MaterialCategory { Name = "Задачи" },
                    new MaterialCategory { Name = "Допълнителни материали" }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}