using Microsoft.AspNetCore.Mvc;
using StopItEF.Models;
using System.Diagnostics;

namespace StopItEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        cloakinglebg_webserverContext _context;

        public HomeController(ILogger<HomeController> logger, cloakinglebg_webserverContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string? success = TempData["addLinkSuccess"] as string;
            if (success != null)
                ViewBag.AddMessage = success;
            List<Link> model = _context.Links.ToList();
            foreach (Link link in model)
            {
                link.Comments = _context.Comments.Where(c => c.LinkId == link.LinkId).ToList();
                link.Votes = _context.Votes.Where(v => v.LinkId == link.LinkId).ToList();
                if (link.UserId != null)
                {
                    link.User = _context.Users.Where(u => u.UserId == link.UserId).FirstOrDefault();
                }
                else
                {
                    link.User = new User { Pseudo = "[Deleted User]" };
                }
            }
            return View(model.OrderByDescending(m => m.Votes.Sum(v => v.Value)).ToList());
        }

        public IActionResult SignUp(string username, string email, string password)
        {
            bool exists = verifyUsername(username);
            if (!exists && username != null && email != null && password != null)
            {
                addUser(username, email, password);
                TempData.Add("Message", $"User {username} has been added with success! You can now sign in!");
                return RedirectToAction("SignIn");
            }
            else
            {
                if (exists)
                {
                    ViewBag.Message = $"Username {username} already exists!";
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult SignIn(string username, string password)
        {
            User? user = fetchUserForSignIn(username, password);
            string? success = TempData["Message"] as string;
            if (success != null)
                ViewBag.Success = success;

            if (user != null)
            {
                if (TempData.ContainsKey("Username"))
                    TempData.Remove("Username");
                TempData.Add("Username", user.Pseudo);
                return RedirectToAction("Index");
            }
            else
            {
                if(username != null && password != null)
                {
                    ViewBag.Message = "No user found! Try again";
                    return View();
                }
            }
            return View();

        }

        public IActionResult Disconnect()
        {
            TempData.Add("Message", "You have been disconnected successfully!");
            return RedirectToAction("SignIn");
        }

        public void addUser(string username, string email, string password)
        {
            var new_user = new User { Pseudo = username, Email = email, Password = password };
            _context.Add<User>(new_user);
            _context.SaveChanges();
        }

        public bool verifyUsername(string username)
        {
            if (_context.Users.Where(user => user.Pseudo.Equals(username)).FirstOrDefault() != null)
                return true;
            return false;
        }

        public User? fetchUserForSignIn(string username, string password) {
            return _context.Users.Where(user => user.Pseudo.Equals(username) && user.Password.Equals(password)).FirstOrDefault();
        }

        public User? fetchUserById(int id)
        {
            return _context.Users.Where(user => user.UserId == id).FirstOrDefault();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}