using CrudOperation.Models;
using CrudOperation.VM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CrudOperation.Controllers
{

    public class RoleController : Controller
    {
        private long _maxAllowedPosterSize = 1048576;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppUser _appuser;
        private readonly SignInManager<AppUser> _signManger;
        public RoleController(RoleManager<IdentityRole> _Role, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppUser appuser)
        {
            Role = _Role;
            _userManager = userManager;
            _signInManager = signInManager;
            _appuser = appuser;
        }

        public RoleManager<IdentityRole> Role { get; }

        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> New(RoleVM VM)
        {
            if (ModelState.IsValid == true)
            {
                IdentityRole role = new IdentityRole();
                role.Name = VM.RoleName;
              IdentityResult result=  await Role.CreateAsync(role);
                if (result.Succeeded )
                {
                    return RedirectToAction("Login","Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                        return View(item.Description); 

                    }
                }
                    
            }
            return View(VM);
        }
        public IActionResult register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> register(RegisterdataViewModel account)

        {
            var Files = Request.Form.Files;
            if (!Files.Any())
            {
                ModelState.AddModelError("Image", "Select Profile");
                return View(account);
            }
            var poster = Files.FirstOrDefault();
            var allowedE = new List<string> { ".jpg", ".png" };
            if (!allowedE.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                ModelState.AddModelError("Image", "Select allowed Extension");
                return View(account);
            }
            if (poster.Length > 1048576)
            {
                ModelState.AddModelError("Image", "can't be more than 1 mg");
                return View(account);
            }
            using var datastream = new MemoryStream();
            await poster.CopyToAsync(datastream);
            ViewBag.Image = datastream.ToArray();

            if (ModelState.IsValid == true)
            {
                AppUser appUser = new AppUser()
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    UserName = account.UserName,
                    Email = account.Email,
                    Address = account.Address,
                    PasswordHash = account.Password,
                    Image = datastream.ToArray(),

                };

                IdentityResult identityResult = await _userManager.CreateAsync(appUser, account.Password);
                if (identityResult.Succeeded)
                {
                    //create cookie
                    await _userManager.AddToRoleAsync(appUser, "Admin");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    //send to view 
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }
                return RedirectToAction("Index", "Home");
            }
            return View(account);

        }

    }
}
