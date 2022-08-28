using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBokningApp.Core.Entities;
using GymBokningApp.Data.Data;
using GymBokningApp.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace GymBokningApp.Web.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
       private readonly UserManager<ApplicationUser> userManager;
       // private readonly IUnitOfWork uow;


        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
           // this.uow = uow;

        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymClasses.ToListAsync());
        }

        public async Task<IActionResult> BookingToggle(int? id)
        {
            //if (id is null) return BadRequest();

            ////var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = userManager.GetUserId(User);

            //if (userId == null) return BadRequest();

            //var attending = await uow.UserGymRepository.FindAsync(userId, (int)id);

            //if (attending == null)
            //{
            //    var booking = new ApplicationUserGymClass
            //    {
            //        ApplicationUserId = userId,
            //        GymClassId = (int)id
            //    };

            //    uow.UserGymRepository.Add(booking);

            //    // db.AppUserGyms.Add(booking);
            //}
            //else
            //{
            //    uow.UserGymRepository.Remove(attending);
            //    //db.AppUserGyms.Remove(attending);
            //}

            //await db.SaveChangesAsync();

            //return RedirectToAction("Index");  if (id == null) return NotFound();
            if (id == null) return NotFound();

            else
            {
                //this user is logged in             
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userId = userManager.GetUserId(User);
                //which class?
                var gymClass = await _context.GymClasses.FirstOrDefaultAsync(g => g.Id == id);

                //logged user is on this class 
                var b = await _context.ApplicationUserGymClass.FirstOrDefaultAsync(t => t.ApplicationUserId == userId && t.GymClassId == id);

                //user is not on this class: add user to this class (add into ApplicationUserGymClass db)
                if (b == null)
                {
                    var classAndMember = new ApplicationUserGymClass
                    {
                        ApplicationUser = _context.Users.ToList().FirstOrDefault(u => u.Id == userId),
                        GymClass = gymClass,
                    };
                    _context.ApplicationUserGymClass.Add(classAndMember);
                }
                else
                {
                    _context.ApplicationUserGymClass.Remove(b);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = gymClass.Id });


            }
        }
        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
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
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymClasses == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymClasses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymClasses'  is null.");
            }
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClasses.Remove(gymClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
          return (_context.GymClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
