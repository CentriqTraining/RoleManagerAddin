using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RecordCollection.Models;

namespace RecordCollection.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SecurityController : BaseController 
    {
        // GET: Security
        public ActionResult Index()
        {
            var roles = AppRoleManager.Roles
                .Select(r => r.Name)
                .ToList();
            var systemusers = AppUserManager.Users.ToList();
            var users = new List<UserManagement_DTO>();
            foreach (var item in systemusers)
            {
                var user = new UserManagement_DTO()
                {
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Roles = AppUserManager.GetRoles(item.Id),
                    UserName = item.UserName,
                };
                users.Add(user);
            }
            ViewBag.Roles = roles;
            return View(users);
        }

        // GET: Security/Details/5
        public ActionResult ViewRoles()
        {
            var roles = AppRoleManager.Roles.ToList().Select(r => new RoleManagement_DTO()
            {
                Id = r.Id,
                Name = r.Name,
                Users = r.Users.Select(u => GetUserById(u.UserId)).ToList()
            });
            if (roles.Count() > 0)
            {
                return View(roles.ToList());
            }
            return View();
        }

        private string GetUserById(string Id)
        {
            var user = AppUserManager.FindById(Id);
            if (user != null)
            {
                return user.UserName;
            }
            else
                return string.Empty;

        }

        // GET: Security/Create
        public ActionResult CreateRole()
        {
            return View();
        }

        // POST: Security/Create
        [HttpPost]
        public ActionResult CreateRole(string name)
        {
            try
            {
                var newrole = new IdentityRole() { Name = name };
                AppRoleManager.Create(newrole);
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RemoveUserFromRole(string Id, string role)
        {
            var user = AppUserManager.FindByName(Id);

            AppUserManager.RemoveFromRole(user.Id, role);
            return RedirectToAction("Index");
        }

        public ActionResult AddUserToRole(string Id, string role)
        {
            var user = AppUserManager.FindByName(Id);
            
            AppUserManager.AddToRoles(user.Id, role);
            return RedirectToAction("Index");
        }
    }
}
