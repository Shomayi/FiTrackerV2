using FiTrackerV2.BLL;
using FiTrackerV2.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FiTrackerV2.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ExerciseService _service;

        public ExerciseController(ExerciseService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var exercises = await _service.GetAllExercisesAsync();
            return View(exercises);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Exercise model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.CreateExerciseAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _service.GetExerciseAsync(id);
            if (exercise == null) return NotFound();
            return View(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Exercise model)
        {
            if (!ModelState.IsValid) return View(model);
            await _service.UpdateExerciseAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteExerciseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
