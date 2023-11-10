using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lab2.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        int pageSize = 2;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }
        public IActionResult Index(int? mid)
        {
            var learners = (IQueryable<Learner>)db.Learners
                .Include(m => m.Major);

            if (mid != null)
            {
                learners = (IQueryable<Learner>)db.Learners
                    .Where(l => l.MajorID == mid)
                    .Include(m => m.Major);
            }

            int pageNumber = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            ViewBag.PageNum = pageNumber;

            var result = learners.Take(pageSize).ToList();
            return View(result);

        }
        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners
                .Where(l => l.MajorID == mid)
                .Include(m => m.Major).ToList();
            return PartialView("LearnerTable", learners);
        }

        // create
        public IActionResult Create()
        {
            var majors = new List<SelectListItem>(); //cach 1
            foreach (var item in db.Majors)
            {
                majors.Add(new SelectListItem {
                   Text = item.MajorName,
                   Value = item.MajorID.ToString(),
                });
            }
            ViewBag.MajorID = majors;
            //ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName"); //cach 2
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if(ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        //edit
        public IActionResult Edit(int id)
        {
            if(id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Find(id);
            if(learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,
            [Bind("LearnerID, FirstMidName, LastName, MajorID, EnrollmentDate")] Learner learner)
        {
            if(id != learner.LearnerID)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }
        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e  => e.LearnerID == id)).GetValueOrDefault(); 
        }

        //delete
        public IActionResult Delete(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }
            if(learner.Enrollments.Count()>0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }
            return View(learner);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learners == null)
            {
                return Problem("Entity set 'Learners' is null");
            }
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
            }
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            var learners = (IQueryable<Learner>)db.Learners;

            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);

            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
                ViewBag.Mid = mid;
            }

            if (keyword != null)
            {
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(keyword.ToLower()));
                ViewBag.Keyword = keyword;
            }

            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);

            ViewBag.PageNum = pageNum;

            var result = learners.Skip(pageSize * (page - 1))
                        .Take(pageSize).Include(m => m.Major);  
           
            return PartialView("LearnerTable", result);
        }

    }
}
