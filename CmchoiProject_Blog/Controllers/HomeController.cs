using CmchoiProject_Blog.Models;
using CmchoiProject_Blog.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace CmchoiProject_Blog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: BlogPosts
         public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr); //Add an search object to call IndexSearch()Method down

            int pageSize = 5;   //the number of posts you want to display per page
            int pageNumber = (page ?? 1); // if th epage number is null show page 1 and if there's page nubmer go to that page
            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        //IndexSearch(searchStr)
        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if (searchStr != null)
            {
                result = db.BlogPosts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                p.Body.Contains(searchStr) ||
                p.Comments.Any(c => c.Body.Contains(searchStr) ||
                c.Author.FirstName.Contains(searchStr) ||
                c.Author.LastName.Contains(searchStr) ||
                c.Author.DisplayName.Contains(searchStr) ||
                c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.BlogPosts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);




        }
        //public ActionResult Index()
        //{
        //    var PublishedPosts = db.BlogPosts.Where(b => b.Published).ToList();
        //    return View(PublishedPosts);
        //}
       


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold> ({1})</p><p> Subject:</p><p>{2}</p><p> Message:</p><p>{3}</p>";

                    var email = new IdentityMessage()
                    {
                        Subject = "Portfolio Contact Email",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Subject,
                                             model.Body),
                        Destination = ConfigurationManager.AppSettings["emailto"]
                    };

                    var svc = new EmailService();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

    }
}