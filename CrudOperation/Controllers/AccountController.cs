using CrudOperation.Models;
using CrudOperation.Reprositry;
using CrudOperation.VM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;
using System.Drawing;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace CrudOperation.Controllers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AccountController : Controller
    {
        private readonly IToastNotification Toasnoty;

        private long _maxAllowedPosterSize = 1048576;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private readonly UserManager<AppUser> _userManager;
        private readonly AppUser _appuser;
        private readonly SignInManager<AppUser> _signManger;
        IAccountReprositry AccRepo;
        public AccountController(IAccountReprositry accountReprositry, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppUser appuser, IToastNotification _Toasnoty)
        {
            _signManger = signInManager;
            _userManager = userManager;
            AccRepo = accountReprositry;
            _appuser = appuser;
            Toasnoty = _Toasnoty;
        }
        //AppDbContext Context= new AppDbContext();
        //
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
            

            if (ModelState.IsValid == true)
            {
                AppUser appUser = new AppUser()
                {
                    PhoneNumber=account.PhoneNumber,
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
                   
                   await _signManger.SignInAsync(appUser, false);// Id and UserName
                    Toasnoty.AddSuccessToastMessage("Succesfully Register");
                    // the custom crate Cookie 
                    //ClaimsIdentity claims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    //claims.AddClaim(new Claim("", account.Id.ToString()));
                    //claims.AddClaim(new Claim("UserName", account.UserName));

                    //ClaimsPrincipal principal = new ClaimsPrincipal(claims);
                    //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Movies");
                }
                else
                {
                    //send to view 
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }
                // الشغل القديم 
                //Account account1 = new Account();
                //account1.UserName = account.UserName;
                //account1.Password = account.Password;

                //AccRepo.save();

                //return RedirectToAction("Index", "Deparetment");
                return RedirectToAction("Index", "Home");
            }
            return View(account);

        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM Vm)
        {
            if (ModelState.IsValid == true)
            {
                var applicationUser = await _userManager.FindByNameAsync(Vm.UserName);
                
                var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(applicationUser, Vm.Password);
                if(isOldPasswordCorrect!=true)
                {
                    Toasnoty.AddErrorToastMessage("Error Login");
                    ModelState.AddModelError("", "Username Or Password is not corrent");
                }
                else { 
                if (applicationUser != null)
                {
                    await _signManger.PasswordSignInAsync(applicationUser, Vm.Password, Vm.RememberMe, false);
                    Toasnoty.AddSuccessToastMessage("Succesfully LogIn");
                    return RedirectToAction("Index", "Movies");
                }
                else
                {
                    Toasnoty.AddErrorToastMessage("Error Login");
                    ModelState.AddModelError("", "Username Or Password is not corrent");
                }
                }
            }
            return View(Vm);
        }
        #region The authontcation Login Custom 
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(Account account)
        //{
        //    var account1 = AccRepo.Find(account.UserName, account.Password);
        //    if (ModelState.IsValid && account1 == true)
        //    {
        //        //AppUser account2 = AccRepo.Get(account.UserName, account.Password);
        //        //_signManger.SignInAsync(appUser, false);
        //        //ClaimsIdentity claims = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        //        //claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, account2.Id.ToString()));
        //        //claims.AddClaim(new Claim(ClaimTypes.Name, account2.UserName));
        //        //// claims.AddClaim(new Claim(ClaimTypes.Role, account2.Fun to The Roe in database(IDataSerializer To Acc)));

        //        //ClaimsPrincipal principle = new ClaimsPrincipal(claims);
        //        //HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        //        return RedirectToAction("Index","Movies");
        //    }
        //    if (account1 == null)
        //    {
        //        return View(account1);
        //    }
        //    else
        //        return RedirectToAction("Index", "Movies");
        //}
        #endregion
        public async Task<IActionResult> signout()
        {
            await _signManger.SignOutAsync();
            Toasnoty.AddErrorToastMessage("Logged out");
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> AccountDetails(string Name)
        {
            // Check if the user ID is provided
            if (string.IsNullOrEmpty(Name))
            {
                return NotFound(); // Handle the scenario when user ID is not provided
            }

            // Find the user by ID
            var user = await _userManager.FindByNameAsync(Name);
            if (user == null)
            {
                return NotFound(); // Handle the scenario when user is not found by the ID
            }
            return View(user);

        }

        public async Task<IActionResult> Edit(string Id)
        {
            if (Id==null)
            {
                return NotFound();
            }
            var edit = await _userManager.FindByIdAsync(Id);
            if (edit == null)
            {
                return BadRequest();
            }
            var Edit = new RegisterdataViewModel
            {
                FirstName = edit.FirstName,
                LastName = edit.LastName,
                PhoneNumber = edit.PhoneNumber,
                UserName = edit.UserName,
                Email = edit.Email,
                Address = edit.Address,
                Image = edit.Image,
                
                
               
            };
            return View(Edit);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit( RegisterdataViewModel Acc)
        {
            if (Acc == null )
            {
                return BadRequest();
            }
            
            var edit = await _userManager.FindByNameAsync(User.Identity.Name);
            if (edit == null)
                return BadRequest();
            var files = Request.Form.Files;

            if (files.Any())
            {
                var Profile = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await Profile.CopyToAsync(dataStream);

                Acc.Image = dataStream.ToArray();

                if (!_allowedExtenstions.Contains(Path.GetExtension(Profile.FileName).ToLower()))
                {

                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                    return View( Acc);
                }

                if (Profile.Length > _maxAllowedPosterSize)
                {

                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return View(Acc);
                }
                edit.Image = dataStream.ToArray();


            }
            if (ModelState.IsValid == true)
            {
                edit.FirstName = Acc.FirstName;
                edit.LastName = Acc.LastName;
                edit.Email = Acc.Email;
                edit.Address = Acc.Address;
                edit.PhoneNumber = Acc.PhoneNumber;
                var result = await _userManager.UpdateAsync(edit);
                Toasnoty.AddSuccessToastMessage("Succesfully Edited");
                if (result.Succeeded)
                {
                // Update successful, redirect to a success page or the profile page
                

                return RedirectToAction("AccountDetails", "Account", new {User.Identity.Name});
                // Redirect to the profile page
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(Acc);

            // If there are errors, add them to ModelState and display them in the view


        }
        public async Task<IActionResult> ChangePassword(string Id)
        {
            var User = await _userManager.FindByIdAsync(Id);
            var Edit = new ChangePassword
            {
                OldPassword = User.PasswordHash,

            };
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(string Id,ChangePassword changePassword)
        {
            if (changePassword == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if(user == null)
            {
                return View();
            }
            if (ModelState.IsValid == true)
            {
                var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, changePassword.OldPassword);
                if (!isOldPasswordCorrect)
                {
                    ModelState.AddModelError("", "The old password provided is incorrect.");
                    return View();
                }

                // Change the user's password
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    // Password updated successfully
                    return RedirectToAction("AccountDetails", new { User.Identity.Name });
                }
                else
                {
                    // Handle errors
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();

                }
               
            }
            return View();

        }


    }
}
