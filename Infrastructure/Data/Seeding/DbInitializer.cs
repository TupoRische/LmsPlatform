using Infrastructure.Data.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Seeding;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Apply migrations safely
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        await SeedRoles(roleManager);
        await SeedSchools(context);
        var users = await SeedUsers(userManager, context);
        await SeedProfessions(context);
        await SeedMaterialCategories(context);
        await SeedMaterials(context, users);
    }

    // ---------------- ROLES ----------------

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Teacher", "Student" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ---------------- SCHOOLS ----------------

    private static async Task SeedSchools(ApplicationDbContext context)
    {
        if (context.Schools.Any())
            return;

        context.Schools.AddRange(
    new School
    {
        Name = "Природо-математическа гимназия \"Иван Вазов\"",
        Abbreviation = "ПМГ „Иван Вазов“",
        City = "Добрич",
        Description = "Профилирана гимназия с фокус върху математика, информатика и природни науки."
    },
new School
{
    Name = "Финансово-стопанска гимназия \"Васил Левски\"",
    Abbreviation = "ФСГ „Васил Левски“",
    City = "Добрич",
    Description = "Професионално обучение в областта на икономика, счетоводство и бизнес администрация."
},
new School
{
    Name = "Средно училище \"Свети Климент Охридски\"",
    Abbreviation = "СУ „Свети Климент Охридски“",
    City = "Добрич",
    Description = "Общообразователно училище с разнообразни профили и извънкласни дейности."
},
new School
{
    Name = "Професионална гимназия по ветеринарна медицина \"Проф. д-р Г. Павлов\"",
    Abbreviation = "ПГВМ „Проф. д-р Г. Павлов“",
    City = "Добрич",
    Description = "Специализирано обучение по ветеринарна медицина и селско стопанство."
},
new School
{
    Name = "Професионална гимназия по туризъм \"П.К. Яворов\"",
    Abbreviation = "ПГТ „П.К. Яворов“",
    City = "Добрич",
    Description = "Подготовка на кадри в туризма, ресторантьорството и хотелиерството."
},
new School
{
    Name = "Професионална гимназия по компютърно моделиране и компютърни системи \"Академик Благовест Сендов\"",
    Abbreviation = "ПГКМКС „Акад. Благовест Сендов“",
    City = "Варна",
    Description = "Обучение в сферата на софтуерните технологии, програмирането и компютърните системи."
},
new School
{
    Name = "Професионална гимназия по електротехника",
    Abbreviation = "ПГ по електротехника",
    City = "Варна",
    Description = "Професионално образование в областта на електротехниката, автоматизацията и електрониката."
},
new School
{
    Name = "Свищовска професионална гимназия \"Алеко Константинов\"",
    Abbreviation = "СПГ „Алеко Константинов“",
    City = "Свищов",
    Description = "Професионална подготовка в технически и икономически специалности."
},
new School
{
    Name = "Професионална гимназия по строителство, архитектура и геодезия „Васил Левски“",
    Abbreviation = "ПГСАГ „Васил Левски“",
    City = "Варна",
    Description = "Специализирано обучение в строителството, архитектурата и геодезията."
}
);

        await context.SaveChangesAsync();
    }

    // ---------------- USERS ----------------

    private static async Task<(ApplicationUser admin,
                           ApplicationUser teacher1,
                           ApplicationUser teacher2)>
    SeedUsers(UserManager<ApplicationUser> userManager,
              ApplicationDbContext context)
    {
        var pmg = context.Schools.First(s => s.Abbreviation == "ПМГ „Иван Вазов“");
        var fsg = context.Schools.First(s => s.Abbreviation == "ФСГ „Васил Левски“");
<<<<<<< HEAD
        var su = context.Schools.First(s => s.Abbreviation == "СУ „Свети Климент Охридски“");
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

        async Task<ApplicationUser> CreateUser(
    string email,
    string first,
    string last,
    string? role,
    bool approved,
    string password,
    bool requestedTeacher = false,
    int? schoolId = null)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = first,
                    LastName = last,
                    CreatedOn = DateTime.UtcNow,
                    IsApproved = approved,
                    RequestedTeacher = requestedTeacher,
                    SchoolId = schoolId
                };

                await userManager.CreateAsync(user, password);

                if (!string.IsNullOrEmpty(role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
<<<<<<< HEAD
            else
            {
                var shouldUpdate = false;

                if (user.FirstName != first)
                {
                    user.FirstName = first;
                    shouldUpdate = true;
                }

                if (user.LastName != last)
                {
                    user.LastName = last;
                    shouldUpdate = true;
                }

                if (user.IsApproved != approved)
                {
                    user.IsApproved = approved;
                    shouldUpdate = true;
                }

                if (user.RequestedTeacher != requestedTeacher)
                {
                    user.RequestedTeacher = requestedTeacher;
                    shouldUpdate = true;
                }

                if (user.SchoolId != schoolId)
                {
                    user.SchoolId = schoolId;
                    shouldUpdate = true;
                }

                if (shouldUpdate)
                {
                    await userManager.UpdateAsync(user);
                }

                if (!string.IsNullOrEmpty(role) && !await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

            return user;
        }

        var admin = await CreateUser(
            "admin@lms.com",
            "System",
            "Admin",
            "Admin",
            true,
            "Admin123!");

        var teacher1 = await CreateUser(
            "teacher1@lms.com",
            "Иван",
            "Иванов",
            "Teacher",
            true,
            "Teacher123!",
            false,
            pmg.Id);

        var teacher2 = await CreateUser(
            "teacher2@lms.com",
            "Мария",
            "Петрова",
            "Teacher",
            true,
            "Teacher123!",
            false,
            fsg.Id);

        // Pending teacher (без Teacher роля)
        await CreateUser(
            "teacher3@lms.com",
            "Георги",
            "Стоянов",
            null,
            false,
            "Teacher123!",
            true,
            pmg.Id);

        // Students
        await CreateUser(
            "student1@lms.com",
            "Петър",
            "Петров",
            "Student",
            true,
<<<<<<< HEAD
            "Student123!",
            false,
            pmg.Id);
=======
            "Student123!");
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

        await CreateUser(
            "student2@lms.com",
            "Анна",
            "Илиева",
            "Student",
            true,
<<<<<<< HEAD
            "Student123!",
            false,
            su.Id);
=======
            "Student123!");
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

        return (admin, teacher1, teacher2);
    }

    // ---------------- PROFESSIONS ----------------

    private static async Task SeedProfessions(ApplicationDbContext context)
    {
        if (context.Professions.Any())
            return;

        int GetSchoolId(string name, string city) =>
            context.Schools
<<<<<<< HEAD
                .First(s => s.Name == name && s.City == city)
                .Id;

        context.Professions.AddRange(
=======
                .Where(s => s.Name == name && s.City == city)
                .Select(s => s.Id)
                .First();

        if (!context.Professions.Any())
        {
            context.Professions.AddRange(
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

                new Profession
                {
                    Name = "Графичен дизайнер",
                    Description = "Създава визуални материали: лого, плакати, брошури, банери и дизайн за социални мрежи.",
                    SchoolId = GetSchoolId(
                        "Средно училище \"Свети Климент Охридски\"", "Добрич")
                },

                new Profession
                {
                    Name = "Оперативен счетоводител",
                    Description = "Обработва първични документи, фактури, каса/банка и подпомага счетоводното отчитане.",
                    SchoolId = GetSchoolId(
                        "Финансово-стопанска гимназия \"Васил Левски\"", "Добрич")
                },

                new Profession
                {
                    Name = "Приложен програмист",
                    Description = "Разработва уеб/десктоп приложения, работи с бази данни и поддръжка на софтуер.",
                    SchoolId = GetSchoolId(
                        "Природо-математическа гимназия \"Иван Вазов\"", "Добрич")
                },

                new Profession
                {
                    Name = "Системен програмист",
                    Description = "Работи със системен софтуер, мрежи, конфигурация и поддръжка на компютърни системи.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по компютърно моделиране и компютърни системи \"Академик Благовест Сендов\"", "Варна")
                },

                new Profession
                {
                    Name = "Електронна търговия",
                    Description = "Организира онлайн продажби, управление на продукти, поръчки, маркетинг и обслужване на клиенти.",
                    SchoolId = GetSchoolId(
                        "Финансово-стопанска гимназия \"Васил Левски\"", "Добрич")
                },

                new Profession
                {
                    Name = "Електротехник",
                    Description = "Монтаж и поддръжка на електроинсталации, табла, измервания и безопасност.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по електротехника", "Варна")
                },

                new Profession
                {
                    Name = "Техник на комуникационни системи",
                    Description = "Инсталира и поддържа комуникационни мрежи, устройства и кабелни системи.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по електротехника", "Варна")
                },

                new Profession
                {
                    Name = "Техник по транспортна техника",
                    Description = "Диагностика и поддръжка на транспортна техника, основни ремонти и технически прегледи.",
                    SchoolId = GetSchoolId(
                        "Свищовска професионална гимназия \"Алеко Константинов\"", "Свищов")
                },

                new Profession
                {
                    Name = "Строителен техник",
                    Description = "Организация на строителни дейности, измервания, документация и контрол на качеството.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по строителство, архитектура и геодезия „Васил Левски“", "Варна")
                },

                new Profession
                {
                    Name = "Ветеринарен техник",
                    Description = "Подпомага ветеринарната дейност: грижа за животни, манипулации, хигиена и документация.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по ветеринарна медицина \"Проф. д-р Г. Павлов\"", "Добрич")
                },

                new Profession
                {
                    Name = "Готвач",
                    Description = "Приготвя храна по технологични рецепти, организация на кухня и контрол на качеството.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по туризъм \"П.К. Яворов\"", "Добрич")
                },

                new Profession
                {
                    Name = "Хотелиер",
                    Description = "Работа с гости, резервации, настаняване, обслужване и организация на хотелския процес.",
                    SchoolId = GetSchoolId(
                        "Професионална гимназия по туризъм \"П.К. Яворов\"", "Добрич")
                }
            );

<<<<<<< HEAD
        await context.SaveChangesAsync();
=======
            await context.SaveChangesAsync();
        }
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
    }

    // ---------------- CATEGORIES ----------------

    private static async Task SeedMaterialCategories(ApplicationDbContext context)
    {
        if (context.MaterialCategories.Any())
            return;

        context.MaterialCategories.AddRange(
            new MaterialCategory { Name = "Теория" },
            new MaterialCategory { Name = "Практика" },
            new MaterialCategory { Name = "Презентации" },
            new MaterialCategory { Name = "Задачи" }
        );

        await context.SaveChangesAsync();
    }

    // ---------------- MATERIALS ----------------

    private static async Task SeedMaterials(
        ApplicationDbContext context,
        (ApplicationUser admin,
         ApplicationUser teacher1,
         ApplicationUser teacher2) users)
    {
        if (context.Materials.Any())
            return;

        var category = context.MaterialCategories.First();
        var profession = context.Professions.First();

        context.Materials.AddRange(

            new Material
            {
                Title = "Въведение в C#",
                Description = "Основи на C# програмирането",
                TeacherId = users.teacher1.Id,
                ProfessionId = profession.Id,
                MaterialCategoryId = category.Id,
                CreatedOn = DateTime.UtcNow,
                FilePath = "/uploads/materials/IntroToCSharp.pptx"
            },

            new Material
            {
                Title = "HTML Основи",
                Description = "Структура на HTML документ",
                TeacherId = users.teacher2.Id,
                ProfessionId = profession.Id,
                MaterialCategoryId = category.Id,
                CreatedOn = DateTime.UtcNow,
                FilePath = "/uploads/materials/HtmlBasics.pdf"
            }
        );

        await context.SaveChangesAsync();
    }
<<<<<<< HEAD
}

=======
}
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
