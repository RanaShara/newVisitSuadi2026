using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using newVisitSuadi2026.Data;
using newVisitSuadi2026.Models;


namespace newVisitSuadi2026.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardContext _context;
               public DashboardController(DashboardContext context)
        {
            _context = context;
           
        }

        // ================= Dashboard =================
        public IActionResult Index()
        {
            return View();
        }
       // ================= TourCompany =================
public IActionResult TourCompany()
{
     var Tourguid = _context.TourGuide.ToList();
        
            ViewBag.getData = Tourguid;

            var TourCompanys = _context.TourCompany
                .Join(
                    _context.TourGuide,
                    p => p.Id,
                    c => c.TourCompanyId,

                    (p, c) => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        TourGuideName = c.Name,
                        
                    }
                ).ToList();

            ViewBag.GetCompany = TourCompanys;
     
     return View();
}

[HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult CreateNewTourCompany(TourCompany company)
{
    _context.TourCompany.Add(company);
    _context.SaveChanges();
    return RedirectToAction("TourCompany");
}

public IActionResult DeleteTourCompany(int id)
{
    var company = _context.TourCompany.SingleOrDefault(c => c.Id == id);

    if (company != null)
    {
        _context.TourCompany.Remove(company);
        _context.SaveChanges();
    }

    return RedirectToAction("TourCompany");
}

public IActionResult EditTourCompany(int id)
{
    var company = _context.TourCompany.SingleOrDefault(c => c.Id == id);
    return View(company);
}

[HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult UpdateTourCompany(TourCompany company)
{
    _context.TourCompany.Update(company);
    _context.SaveChanges();

    return RedirectToAction("TourCompany");
}
   // ================= TourGuide =================
public IActionResult TourGuide()
{
    ViewBag.getCompany = _context.TourCompany.ToList();

    var guides = _context.TourGuide
        .Join(
            _context.TourCompany,
            g => g.TourCompanyId,
            c => c.Id,
            (g, c) => new
            {
                Id = g.Id,
                Name = g.Name,
                CompanyName = c.Name
            }
        )
        .ToList();

    ViewBag.GetGuide = guides;

    return View();
}
[HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult CreateNewTourGuide(TourGuide guide)
{
    _context.TourGuide.Add(guide);
    _context.SaveChanges();

    return RedirectToAction("TourGuide");
}

public IActionResult DeleteTourGuide(int id)
{
    var guide = _context.TourGuide.SingleOrDefault(g => g.Id == id);

    if (guide != null)
    {
        _context.TourGuide.Remove(guide);
        _context.SaveChanges();
    }

    return RedirectToAction("TourGuide");
}

public IActionResult EditTourGuide(int id)
{
    ViewBag.getCompany = _context.TourCompany.ToList();

    var guide = _context.TourGuide.SingleOrDefault(g => g.Id == id);

    return View(guide);
}

[HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult UpdateTourGuide(TourGuide guide)
{
    _context.TourGuide.Update(guide);
    _context.SaveChanges();

    return RedirectToAction("TourGuide");
}


        // ================= Packages =================
 public IActionResult Package()
{
    ViewBag.getData = _context.City.ToList();
    ViewBag.getCompany = _context.TourCompany.ToList();
    ViewBag.getGuide = _context.TourGuide.ToList();

    var packages = _context.Package
.Join(
_context.City,
p => p.CityId,
c => c.Id,
(p, c) => new
{
Package = p,
CityName = c.Name
}
)
.Join(
_context.TourCompany,
pc => pc.Package.TourCompanyId,
tc => tc.Id,
(pc, tc) => new
{
pc.Package,
pc.CityName,
CompanyName = tc.Name
}
)
.Join(
_context.TourGuide,
pct => pct.Package.TourGuideId,
tg => tg.Id,
(pct, tg) => new
{
Id = pct.Package.Id,
Name = pct.Package.Name,
Description = pct.Package.Description,
Price = pct.Package.Price,
Details = pct.Package.Details,
ImagePath = pct.Package.ImagePath,
CityName = pct.CityName,
CompanyName = pct.CompanyName,
GuideName = tg.Name
}
)
.ToList();


    ViewBag.GetPackage = packages;

    return View();
}

[HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult CreateNewPackage(Package package, IFormFile Photo)
{
    if (Photo != null && Photo.Length > 0)
    {
        // اسم فريد للصورة
        var fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);

        // المسار داخل wwwroot/uploads
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        // لو المجلد مو موجود
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            Photo.CopyTo(stream);
        }

        // المسار اللي ينحفظ بالـ DB
        package.ImagePath = "/uploads/" + fileName;
    }
    else
    {
        package.ImagePath = "/uploads/default.png"; // اختياري
    }

    _context.Package.Add(package);
    _context.SaveChanges();
    return RedirectToAction("Package");
}


        public IActionResult DeletePackage(int id)
        {
            var package = _context.Package.SingleOrDefault(p => p.Id == id);
            if (package != null)
            {
                _context.Package.Remove(package);
                _context.SaveChanges();
            }
            return RedirectToAction("Package");
        }

        public IActionResult EditPackage(int id)
        {
            ViewBag.getData = _context.City.ToList();
            ViewBag.getCompany = _context.TourCompany.ToList();
              ViewBag.getGuide = _context.TourGuide.ToList();
            var package = _context.Package.SingleOrDefault(p => p.Id == id);
            return View(package);
            
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
      
public IActionResult UpdatePackage(Package package, IFormFile Photo)
{
    var existingPackage = _context.Package.FirstOrDefault(p => p.Id == package.Id);
    if (existingPackage == null) return NotFound();

    existingPackage.Name = package.Name;
    existingPackage.Description = package.Description;
    existingPackage.Price = package.Price;
    existingPackage.Details = package.Details;
    existingPackage.CityId = package.CityId;
    existingPackage.TourCompanyId = package.TourCompanyId;
existingPackage.TourGuideId = package.TourGuideId;

    if (Photo != null && Photo.Length > 0)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            Photo.CopyTo(stream);
        }

        existingPackage.ImagePath = "/uploads/" + fileName;
    }

    _context.SaveChanges();
    return RedirectToAction("Package");
}


        // ================= Cities =================
        public IActionResult City()
        {
            return View(_context.City.ToList());
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult CreateNewCity(City city)
        {
            _context.City.Add(city);
            _context.SaveChanges();
            return RedirectToAction("City");
        }

        public IActionResult DeleteCity(int id)
        {
            var city = _context.City.SingleOrDefault(c => c.Id == id);
            if (city != null)
            {
                _context.City.Remove(city);
                _context.SaveChanges();
            }
            return RedirectToAction("City");
        }

        public IActionResult EditCity(int id)
        {
            var city = _context.City.SingleOrDefault(c => c.Id == id);
            return View(city);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdateCity(City city)
        {
            _context.City.Update(city);
            _context.SaveChanges();
            return RedirectToAction("City");
        }
    }
}
