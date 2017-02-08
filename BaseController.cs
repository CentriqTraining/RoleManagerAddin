using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RecordCollection;
using RecordCollection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RecordCollection.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationUserManager _UserManager = null;
        private RoleManager<IdentityRole> _RoleManager = null;
        private ApplicationSignInManager _signInManager = null;
        private ApplicationDbContext _ctx = null;
        private IAuthenticationManager _AuthManger = null;
        protected IAuthenticationManager AppAuthenticationManager
        {
            get
            {
                if (_AuthManger == null)
                {
                    _AuthManger = HttpContext.GetOwinContext().Authentication;
                }
                return _AuthManger;
            }
        }

        protected ApplicationDbContext AppSecurityContext
        {
            get
            {
                if (_ctx == null)
                {
                    _ctx = new ApplicationDbContext();
                }
                return _ctx;
            }
        }
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                if (_UserManager == null)
                {
                    _UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _UserManager;
            }
        }
        protected RoleManager<IdentityRole> AppRoleManager
        {
            get
            {
                if (_RoleManager == null)
                {
                    _RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(AppSecurityContext));
                }
                return _RoleManager;
            }
        }
        protected ApplicationSignInManager AppSignInManager
        {
            get
            {
                if (_signInManager == null)
                {
                    _signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                }
                return _signInManager;
            }
            private set
            {
                _signInManager = value;
            }
        }
        protected override void Dispose(bool disposing)
        {
            _UserManager?.Dispose();
            _RoleManager?.Dispose();
            _signInManager?.Dispose();
            _ctx?.Dispose();
            base.Dispose(disposing);
        }
    }
}