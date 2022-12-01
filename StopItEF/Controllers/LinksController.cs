using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StopItEF.Models;

namespace StopItEF.Controllers
{
    public class LinksController : Controller
    {
        cloakinglebg_webserverContext _context;
        DateOnly _today;

        public LinksController(cloakinglebg_webserverContext context)
        {
            _context = context;
            _today = DateOnly.FromDateTime(DateTime.Now);
        }


        public ActionResult ViewLink(int linkId)
        {
            Link? linkToView = _context.Links.Where(l => l.LinkId == linkId).FirstOrDefault();
            if (linkToView.UserId != null)
            {
                linkToView.User = _context.Users.Where(u => u.UserId == linkToView.UserId).FirstOrDefault();
            }
            else
            {
                linkToView.User = new User { Pseudo = "[Deleted User]" };
            }
            linkToView.Votes = _context.Votes.Where(v => v.LinkId == linkToView.LinkId).ToList();
            linkToView.Comments = _context.Comments.Where(c => c.LinkId == linkToView.LinkId).OrderByDescending(c => c.PublicationDate).ToList();

            foreach(var comment in linkToView.Comments)
            {
                if (comment.UserId != null)
                {
                    comment.User = _context.Users.Where(u => u.UserId == comment.UserId).FirstOrDefault();
                }
                else
                {
                    comment.User = new User { Pseudo = "[Deleted User]" };
                }
            }

            return View(linkToView);
        }

        public IActionResult ViewOwnLinks(string username)
        {
            User? connected = _context.Users.Where(u => u.Pseudo.Equals(username)).FirstOrDefault();
            List<Link> model = new List<Link>();
            string? success = TempData["Message"] as string;
            if (success != null)
                ViewBag.Success = success;
            if (connected != null)
            {
                model = _context.Links.Where(l => l.UserId == connected.UserId).ToList();
            }
            if(model.Count > 0)
            {
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
            return View();
        }

        public IActionResult DeleteLink(int linkid)
        {
            Link? linkToDelete = _context.Links.Where(l => l.LinkId == linkid).FirstOrDefault();
            if(linkToDelete != null)
            {
                _context.Links.Remove(linkToDelete);
                _context.SaveChanges();
                TempData.Add("Message", $"Link ({linkToDelete.Title}) has been removed successfully");
            }
            return RedirectToAction("ViewOwnLinks", new {username = TempData.Peek("Username") as string});
        }

        public IActionResult AddLinkView()
        {
            return View();
        }

        public IActionResult AddLinkToDb(string title, string description)
        {
            User? currentUser = _context.Users.Where(u => u.Pseudo.Equals(TempData.Peek("Username") as string)).FirstOrDefault();
            if (currentUser != null)
            {
                _context.Add<Link>(new Link { Title = title, Description = description, UserId = currentUser.UserId, PublicationDate = _today });
                _context.SaveChanges();
                TempData.Add("addLinkSuccess", $"Link ({title}) created by user {currentUser.Pseudo} has been added!");
            }
            else
            {
                TempData.Add("addLinkSuccess", $"Error while trying to create link ({title})... User not found!");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult addComment(string comment, int _linkid) {
            User? currentUser = _context.Users.Where(u => u.Pseudo.Equals(TempData.Peek("Username") as string)).FirstOrDefault();
            if(currentUser != null)
            {
                _context.Add<Comment>(new Comment { Text = comment, UserId = currentUser.UserId, LinkId = _linkid, PublicationDate = _today });
                _context.SaveChanges();
            }

            return RedirectToAction("ViewLink", new { linkid = _linkid });
        }

        public IActionResult checkVote(string votetype, int idlink)
        {
            User? currentUser = _context.Users.Where(u => u.Pseudo.Equals(TempData.Peek("Username") as string)).FirstOrDefault();
            Vote? vote = _context.Votes.Where(v => v.UserId == currentUser.UserId && v.LinkId == idlink).FirstOrDefault();
            if (vote != null)
            {
                switch (vote.Value)
                {
                    case 1:
                        switch (votetype)
                        {
                            case "upvote":
                                vote.Value = 0;
                                _context.SaveChanges();
                                break;
                            case "downvote":
                                vote.Value = -1;
                                _context.SaveChanges();
                                break;
                        }
                        break;
                    case 0:
                        switch (votetype)
                        {
                            case "upvote":
                                vote.Value = 1;
                                _context.SaveChanges();
                                break;
                            case "downvote":
                                vote.Value = -1;
                                _context.SaveChanges();
                                break;
                        }
                        break;
                    case -1:
                        switch (votetype)
                        {
                            case "upvote":
                                vote.Value = 1;
                                _context.SaveChanges();
                                break;
                            case "downvote":
                                vote.Value = 0;
                                _context.SaveChanges();
                                break;
                        }
                        break;
                }
            }
            else
            {
                if (votetype.Equals("upvote"))
                {
                    _context.Add<Vote>(new Vote { LinkId = idlink, UserId = currentUser.UserId, Value = 1});
                    _context.SaveChanges();
                }
                else
                {
                    if (votetype.Equals("downvote"))
                    {
                        _context.Add<Vote>(new Vote { LinkId = idlink, UserId = currentUser.UserId, Value = -1 });
                        _context.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ViewLink", new { linkid = idlink });
        }
    }
}
