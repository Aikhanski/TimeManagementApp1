using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeManagementApp.Data;
using TimeManagementApp.Models;

namespace TimeManagementApp.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EventController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var events = await _context.Events
                                            .Include(e => e.Content)
                                            .Where(x => x.Creator == user)
                                            .Select( e => new EventView() { 
                                                Id = e.Id,
                                                Title = e.Content.Title,
                                                Date = e.Content.EventDate.ToString("dd.MM.yyyy"),
                                                Description = e.Content.Description
                                            })
                                            .ToListAsync();
            if (events == null)
            {
                return Challenge();
            }
            return View(events);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .Include(x => x.Content)
                .Include(x => x.EventParticipants)
                    .ThenInclude(y => y.User)
                .Select(e => new EventDetailView()
                {
                    Id = e.Id,
                    Title = e.Content.Title,
                    Date = e.Content.EventDate.ToString("dd.MM.yyyy"),
                    Description = e.Content.Description,
                    Address = e.Content.Address,
                    Phone = e.Content.Phone,
                    Participants = e.EventParticipants
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventForm form)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var eventModel = new Event(user);
                _context.Events.Add(eventModel);
                var content = new EventContent(eventModel.Id, form.Title, form.EventDate, eventModel, form.Description, form.Address, form.Phone);
                _context.EventContents.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(form);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var eventModel = await _context.EventContents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }
            var eventForm = new EventEditForm()
            {
                Id = eventModel.Id,
                Title = eventModel.Title,
                EventDate = eventModel.EventDate,
                Description = eventModel.Description,
                Address = eventModel.Address,
                Phone = eventModel.Phone
            };

            return View(eventForm);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventEditForm form)
        {
            if (ModelState.IsValid)
            {
                var content = await _context.EventContents.FirstOrDefaultAsync(x => x.Id == form.Id);
                if (content == null)
                {
                    return NotFound();
                }
                content.Title = form.Title;
                content.EventDate = form.EventDate;
                content.Description = form.Description;
                content.Address = form.Address;
                content.Phone = form.Phone;
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(form.Id))
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

                
            return View(form);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var content = await _context.EventContents.FirstOrDefaultAsync(p => p.Event.Id == id);
            if (content != null)
            {
                _context.EventContents.Remove(content);
                await _context.SaveChangesAsync();
            }
            var eventModel = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel != null)
            {
                _context.Events.Remove(eventModel);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
               

        private bool EventExists(string id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
