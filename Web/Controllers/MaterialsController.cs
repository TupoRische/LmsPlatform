using Core.Contracts;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Contracts;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======
>>>>>>> ee95621cecddaaaf9564213ba5eb43054605cad8
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IMaterialService materials;
        private readonly ICommentService comments;

        public MaterialsController(IMaterialService materials, ICommentService comments)
        {
            this.materials = materials;
            this.comments = comments;
        }

<<<<<<< HEAD
        [Authorize]
=======
>>>>>>> ee95621cecddaaaf9564213ba5eb43054605cad8
        public async Task<IActionResult> Details(int id)
        {
            var model = await materials.GetByIdAsync(id);
            if (model == null) return NotFound();

            model.Comments = await comments.GetByMaterialAsync(id);

            return View(model);
        }
        public async Task<IActionResult> Index(int professionId)
         => View(await materials.GetByProfessionAsync(professionId));
    }
}
