using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Infra;
using RouteMasterFrontend.Models.ViewModels.Members;
using System.Security.Claims;

namespace RouteMasterFrontend.Controllers
{
    public class MembersController : Controller
    {
        private readonly RouteMasterContext _context;
		private readonly IWebHostEnvironment _environment;

		public MembersController(RouteMasterContext context, IWebHostEnvironment environment)
        {
            _context = context;
			_environment = environment;
		}





        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Members == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Account,EncryptedPassword,Email,CellPhoneNumber,Address,Gender,Birthday,CreateDate,Image,IsConfirmed,ConfirmCode,GoogleAccessCode,FaceBookAccessCode,LineAccessCode,IsSuspended,LoginTime,IsSuscribe")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }




        [HttpGet]
		//上船系統圖片
        public IActionResult UploadSystemImages()
		{
			return View();
		}


		[HttpPost]
		public IActionResult UploadSystemImages(IFormFile[] files)
		{
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    string path = Path.Combine(_environment.WebRootPath, "SystemImages");
                    string fileName = SaveUploadFile(path, file);
					SystemImage img = new SystemImage();                    
                    img.Image = fileName;
                    _context.SystemImages.Add(img);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
		



        [HttpGet]
		public IActionResult MemberRegister()
		{
			return View();
		}

		[HttpPost]
		public IActionResult MemberRegister(MemberRegisterVM vm, IFormFile facePhoto)
		{
			if (ModelState.IsValid)
			{
				string path = Path.GetFullPath("Uploads");
				string fileName = SaveUploadFile(path, facePhoto);

				vm.Image = fileName;
			}
			else
			{
				return View(vm);
			}


			Result result = IsMemberExist(vm);
			if (result.IsSuccess)
			{
				return View("ConfirmRegister");
			}
			else
			{
				ModelState.AddModelError(string.Empty, result.ErrorMessage);
				return View(vm);
			}

		}

		private Result IsMemberExist(MemberRegisterVM vm)
		{
			if (_context.Members.Any(m => m.Account == vm.Account))
			{
				// 丟出異常,或者傳回 Result
				return Result.Failure($"帳號 {vm.Email} 已存在, 請更換後再試一次");
			}

			// 將密碼進行雜湊
			var salt = HashUtility.GetSalt();
			var hashPassword = HashUtility.ToSHA256(vm.Password, salt);
			string EncryptedPassword = hashPassword;

			// 填入 isConfirmed, ConfirmCode
			//vm.IsConfirmed = false;
			//vm.ConfirmCode = Guid.NewGuid().ToString("N");

			Member member = new Member
			{
				FirstName = vm.FirstName,
				LastName = vm.LastName,
				Account = vm.Account,
				EncryptedPassword = EncryptedPassword,
				Email = vm.Email,
				CellPhoneNumber = vm.CellPhoneNumber,
				Address = vm.Address,
				Birthday = DateTime.Now,
				Gender = vm.Gender,
				ConfirmCode = Guid.NewGuid().ToString("N"),
				IsConfirmed = false,
				IsSuspended = false,
				IsSuscribe = vm.IsSuscribe
			};

			// 將它存到db
			_context.Members.Add(member);
			_context.SaveChanges();
			return Result.Success();
		}

		[HttpGet]
		public async Task<IActionResult> MemberLogin()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> MemberLogin(MemberLoginVM vm)
		{
			if (ModelState.IsValid == false)
			{
				return View(vm);
			}



			Result result = ValidLogin(vm);

			if (result.IsSuccess != true)
			{
				ModelState.AddModelError("", result.ErrorMessage); return View(vm);
			}

			const bool rememberMe = false;


			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, vm.Account),

			};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			var authProperties = new AuthenticationProperties
			{
				IsPersistent = rememberMe,
				ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
			};

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity),
				authProperties);


			return RedirectToAction("Index", "Members");

		}










		[HttpDelete]
		public void Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}





		private Result ValidLogin(MemberLoginVM vm)
		{
			var db = new RouteMasterContext();
			//Result resultAttempt = LockAccountIfFailedAttemptsExceeded(vm.Account);
			//if (resultAttempt.IsSuccess)
			//{
			//    return resultAttempt; // 帳號已鎖定，返回錯誤結果
			//}

			var member = db.Members.FirstOrDefault(m => m.Account == vm.Account);
			int failedAttempts = 0;
			if (member == null)
			{

				return Result.Failure("帳密有錯");
			}
			else
			{
				var claims = new List<Claim>();
				{
					new Claim(ClaimTypes.Name, member.Account);
					new Claim("LastName", member.LastName);
				};
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

			}
			//if(member.IsSuspended)  --有沒有停權

			var salt = HashUtility.GetSalt();
			var hashPassword = HashUtility.ToSHA256(vm.Password, salt);

			if (string.Compare(hashPassword, member.EncryptedPassword) == 0)
			{
				return Result.Success();

			}
			else
			{

				return Result.Failure("帳密有誤");
			}
		}



		private string SaveUploadFile(string filePath, IFormFile file)
		{
			if (file == null || file.Length == 0) return string.Empty;

			string ext = Path.GetExtension(file.FileName);

			string[] allowExts = new string[] { ".jpg", ".jpeg", ".png", ".tif" };

			if (allowExts.Contains(ext.ToLower()) == false)
			{
				return string.Empty;
			}
			string newFileName = Guid.NewGuid().ToString("N") + ext;
			string fullName = Path.Combine(filePath, newFileName);
			using (var fileStream = new FileStream(fullName, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}
			return newFileName;

		}


		private bool MemberExists(int id)
        {
          return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
