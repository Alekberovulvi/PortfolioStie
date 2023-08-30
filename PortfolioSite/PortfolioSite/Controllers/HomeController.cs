using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PortfolioSite.DAL;
using PortfolioSite.Models;
using PortfolioSite.ViewsModel;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace PortfolioSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {

            var home1 = new HomeVM
            {
                About = await _context.Abouts.FirstOrDefaultAsync(),
                AboutsSites = await _context.AboutsSites.ToListAsync(),
                Favorite = await _context.Favorites.FirstOrDefaultAsync(),
                Percents = await _context.Percents.ToListAsync(),
                SIcons = await _context.SIcons.ToListAsync(),
                Skill = await _context.Skills.FirstOrDefaultAsync(),
                Testimonials = await _context.Testimonials.FirstOrDefaultAsync(),
                TestimonialsEdits = await _context.TestimonialsEdits.ToListAsync(),
                Contact = await _context.Contacts.FirstOrDefaultAsync(),
                ContactLocations = await _context.ContactLocations.ToListAsync(),
                Cvs = await _context.Cvs.FirstOrDefaultAsync(),
                CVmodel = await _context.CVmodels.FirstOrDefaultAsync(),
                ContactFormModel = await _context.contactFormModels.FirstOrDefaultAsync(),
            };
            return View(home1);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }




            var message = new MailMessage();
            message.From = new MailAddress("aze24932@gmail.com");
            message.To.Add("elekberovulvi520@gmail.com");
            message.Subject = model.Subject;
            message.Body = $"Ad: {model.Name}\nEmail: {model.Email}\nBaşlıq: {model.Subject}\nMesaj: {model.Message}";

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("ulvi.alekberov449@gmail.com", "jioeaxsctssowfhg");
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);

            var captchaImage = HttpContext.Request.Form["g-recaptcha-response"];

            if (string.IsNullOrEmpty(captchaImage))
            {
                return Content("Doldur");
            }

            var verified = await CheckCaptcha();

            if (!verified)
            {
                TempData["Message"] = "Cəhdiniz ugursuz oldu!";
            }
            if (verified)
            {
                TempData["Message"] = "Mesaj ugurla gonderildi!";
            }

            return RedirectToAction("Index");
        }

        public async Task<bool> CheckCaptcha()
        {
            var postData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("secret", "6LefW2YmAAAAAL7Ufn0sl3mx3IilhQqdUj_aDIsW"),
                new KeyValuePair<string, string>("response", HttpContext.Request.Form["g-recaptcha-response"])
            };

            var client = new HttpClient();

            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(postData));

            var o = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return (bool)o["success"];
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("{*url}", Order = 999)]
        public IActionResult CatchAll()
        {
            return RedirectToAction("Error");
        }
    }
}