using Microsoft.AspNetCore.Mvc;
using Quizz.Core.Contracts;
using Quizz.Core.Models;

namespace Quizz.Controllers
{
    public class QuizsController : Controller
    {
        private readonly IQuizzService service;

        public QuizsController(IQuizzService _service)
        {
            service = _service;
        }

        // GET: Quizs
        public async Task<IActionResult> Index()
        {
            return View(await service.GetQuizzesAsync());
        }

        // GET: Quizs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await service.GetQuizzAsync(id ?? 0);

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Quizs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] QuizzViewModel quiz)
        {
            if (ModelState.IsValid)
            {
                await service.CreateQuizzAsync(quiz);
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quizs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await service.GetQuizzAsync(id ?? 0);

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] QuizzViewModel quiz)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await service.EditQuizAsync(quiz);
                }
                catch (ArgumentException)
                {
                    return NotFound();
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quizs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await service.GetQuizzAsync(id ?? 0);

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            { 
                await service.DeleteQuizzAsync(id);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
