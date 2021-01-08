using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeManagementApp.Data;
using TimeManagementApp.Models;
using TimeManagementApp.Services;

namespace TimeManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                                        .Include(e => e.Content)
                                        .Select(e => new EventView()
                                        {
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
                .Select(e => new EventDetailView()
                {
                    Id = e.Id,
                    Title = e.Content.Title,
                    Date = e.Content.EventDate.ToString("dd.MM.yyyy"),
                    Description = e.Content.Description,
                    Address = e.Content.Address,
                    Phone = e.Content.Phone
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // GET: Event/Participate/5
        [Authorize]
        public async Task<IActionResult> Participate(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .Include(m => m.EventParticipants)
                    .ThenInclude(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // POST: Event/Participate/5
        [HttpPost, ActionName("Participate")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ParticipateConfirmed(string id)
        {
            var eventModel = await _context.Events
                                        .Include(m => m.Content)
                                        .Include(m => m.EventParticipants)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }
            if (eventModel.EventParticipants == null)
            {
                eventModel.EventParticipants = new List<EventParticipant>();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                eventModel.EventParticipants.Add(new EventParticipant() 
                {
                    Event = eventModel,
                    User = user
                });
                await _context.SaveChangesAsync();
                EmailSender sender = new EmailSender();
                string invitation = $"Dear {user.FirstName} <br/> Here is your Event Invitation <br/> Address: {eventModel.Content?.Address} <br/> on {eventModel.Content?.EventDate.ToString("dd.MM.yyyy")}";
                await sender.SendEmailAsync(user.Email, "Invitation", invitation);
            }

            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
