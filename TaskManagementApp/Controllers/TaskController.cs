using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagementApp.Models;

namespace TaskManagementApp.Controllers
{
    public class TaskController : Controller
    {
        private TaskDbContext db = new TaskDbContext();

        // GET: Task
        public async Task<ActionResult> Index(string searchString)
        {
            var tasks = db.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
            }

            return View(await tasks.ToListAsync());
        }

        // GET: Task/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var taskModel = await db.Tasks.FindAsync(id);
            if (taskModel == null) return HttpNotFound();

            return View(taskModel);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                taskModel.CreatedOn = DateTime.Now;
                taskModel.UpdatedOn = DateTime.Now;

                db.Tasks.Add(taskModel);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(taskModel);
        }

        // GET: Task/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var taskModel = await db.Tasks.FindAsync(id);
            if (taskModel == null) return HttpNotFound();

            return View(taskModel);
        }

        // POST: Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                taskModel.UpdatedOn = DateTime.Now; // ✅ This line is very important
                db.Entry(taskModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(taskModel);
        }


        // GET: Task/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var taskModel = await db.Tasks.FindAsync(id);
            if (taskModel == null) return HttpNotFound();

            return View(taskModel);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var taskModel = await db.Tasks.FindAsync(id);
            db.Tasks.Remove(taskModel);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
