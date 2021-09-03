using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.API.Data.Entiies;
using Vehicles.API.Data.Entities;

namespace Vehicles.API.Controllers
{
    public class VehicleTypesController : Controller
    {
        private readonly DataContext _context;

        public VehicleTypesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.vehicleTypes.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(vehicleType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicada"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(vehicleType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.vehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(vehicleType);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException dbUpdateException)
                    {
                        if (dbUpdateException.InnerException.Message.Contains("duplicada"))
                        {
                            ModelState.AddModelError(string.Empty, "Ya existe este tipo de vehículo.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        }
                    }
                    catch (Exception exception)
                    {
                        ModelState.AddModelError(string.Empty, exception.Message);
                    }
                }
            }
            return View(vehicleType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.vehicleTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            _context.vehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleTypeExists(int id)
        {
            return _context.vehicleTypes.Any(e => e.Id == id);
        }
    }
}
