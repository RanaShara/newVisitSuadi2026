using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using newVisitSuadi2026.Models;
using newVisitSuadi2026.Data;

namespace newVisitSuadi2026.Controllers;
  public class HomeController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

   
        }

    public IActionResult Index()
{
    var GetPackage = _context.Package
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

    var Cities = _context.City
        .Select(c => new
        {
            c.Id,
            c.Name
        })
        .ToList();

    var Companies = _context.TourCompany
        .Select(c => new
        {
            c.Id,
            c.Name
        })
        .ToList();

    ViewBag.GetPackage = GetPackage;
    ViewBag.Cities = Cities;
    ViewBag.Companies = Companies;

    return View();
}


        public IActionResult Details(int Id)
{
    var packageDetails = _context.Package
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
        .FirstOrDefault(p => p.Id == Id);

    if (packageDetails == null)
    {
        return NotFound();
    }

    return View(packageDetails);
}
        public IActionResult Payment(int id)
        {
            var package = _context.Package.Join(
                _context.City,
                Package => Package.CityId,
                City => City.Id,
                (Package, City) => new
                {
                    Id = Package.Id,
                    Name = Package.Name,
                    Price = Package.Price,
                    CityName = City.Name,
                }).FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            ViewBag.Package = package;
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmPayment(int packageId, string cardNumber, string expiryDate, string cvv)
        {
            var package = _context.Package.FirstOrDefault(p => p.Id == packageId);

            if (package == null)
            {
                TempData["ErrorMessage"] = "��� ��� ����� ������ �����. ������ ��� ������.";
                return RedirectToAction("Index");
            }

            
            TempData["SuccessMessage"] = $"�� ����� ����� ������: {package.Name}. �����: {package.Price} ����.";
            return RedirectToAction("Index"); 
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

