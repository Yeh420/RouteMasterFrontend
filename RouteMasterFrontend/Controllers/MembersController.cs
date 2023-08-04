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
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using System.Runtime.Intrinsics.X86;

namespace RouteMasterFrontend.Controllers
{
    public class MembersController : Controller
    {
        private readonly RouteMasterContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly HashUtility _hashUtility;

        public MembersController(RouteMasterContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _hashUtility = new HashUtility(configuration);
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

        //會員資料頁
        [HttpGet]
        public IActionResult MyMemberIndex()
        {
            //抓會員登入資訊
            ClaimsPrincipal user = HttpContext.User;

            //列出與登入符合資料
            if (user.Identity.IsAuthenticated)
            {
                string userName = user.Identity.Name;
                IEnumerable<MemberIndexVM> members;
                members = (IEnumerable<MemberIndexVM>)_context.Members
                    .Where(m => m.Account == userName)
                    .Select(m => new MemberIndexVM
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Account = m.Account,
                        Email = m.Email,
                        CellPhoneNumber = m.CellPhoneNumber,
                        Address = m.Address,
                        Gender = m.Gender,
                        Birthday = m.Birthday,
                        CreateDate = m.CreateDate,
                        Image = m.Image,
                        LoginTime = m.LoginTime,
                        IsConfirmed = m.IsConfirmed,
                        IsSuscribe = m.IsSuscribe,

                    }).ToList();
                return View(members);
            }
            return RedirectToAction("MemberLogin", "Members");
        }

        [HttpGet]
        //上傳系統圖片 -- 有空時應該改去後台管理系統
        public IActionResult UploadSystemImages()
        {
            return View();
        }

        //上傳系統內建大頭貼
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

        //註冊會員
        [HttpGet]
        public IActionResult MemberRegister()
        {
            return View();
        }

        //註冊會員
        [HttpPost]
        public IActionResult MemberRegister(MemberRegisterVM vm, IFormFile? facePhoto, int value)
        {
            MemberImage img = new MemberImage();
            if (ModelState.IsValid)
            {
                if (facePhoto != null && facePhoto.Length > 0)
                {
                    string path = Path.Combine(_environment.WebRootPath, "MemberUploads");
                    string fileName = SaveUploadFile(path, facePhoto);
                    img.Image = fileName;
                    img.Name = "未命名";
                    vm.Image = fileName;
                }
            }
            else
            {
                return View(vm);
            }


            Result result = RegisterMember(vm, img);

            if (result.IsSuccess)
            {
                return View("MyMemberIndex");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }

        }

        //會員普通登入
        [HttpGet]
        public async Task<IActionResult> MemberLogin()
        {
            return View();
        }

        //會員普通登入
        public async Task<IActionResult> MemberLogin(MemberLoginVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);

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

            var memberid = _context.Members.Where(m => m.Account == vm.Account).FirstOrDefault()?.Id;
            if (memberid != null)
            {
                var cart = _context.Carts.FirstOrDefault(x => x.MemberId == memberid);
                if (cart != null)
                {
                    int cartId = cart.Id;

                   
                    Response.Cookies.Append("CartId", cartId.ToString(), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddHours(2)
                    });
                }
                else
                {
                    var newCart = new Cart
                    {
                        MemberId = memberid.Value
                    };

                    _context.Carts.Add(newCart);
                    _context.SaveChanges();

                    int cartId = newCart.Id;

                  
                    Response.Cookies.Append("CartId", cartId.ToString(), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddHours(2)
                    });
                }
                
            }

            return RedirectToAction("MyMemberIndex", "Members");
        }


        //Google登入的會員頁面
        public async Task<IActionResult> GoogleMember()
        {
            return View();
        }

        //會員登出
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Logout", "Members");
        }

        //忘記密碼並寄信
        public IActionResult MemberForgetPassword()
        {
            return View();
        }

        //忘記密碼並寄信
        [HttpPost]
        public IActionResult MemberForgetPassword(MemberForgetPasswordVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            //var myAccount = _context.Members.FirstOrDefault(m=>m.Account == vm.Account);

            //// 生成email裡的連結
            //var urlTemplate = $"{Request.Scheme}://{Request.Host.Value}/Members/MemberResetPassword?Account={myAccount}&confirmCode={0}";
            

            Result result = ProcessResetPassword(vm.Account, vm.Email);

            if (result.IsFalse)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }

            return View("MemberLogin");
        }

        //更改密碼       
        public IActionResult MemberResetPassword()
        {
            return View();
        }
        
        //更改密碼 
        [HttpPost]
        public IActionResult MemberResetPassword(MemberResetPasswordVM vm, string account, string confirmCode)
        {
            if (ModelState.IsValid == false) return View(vm);
            
            Result result = ProcessChangePassword(account, confirmCode, vm.Password);

            //if (result.IsSuccess == false) { }
            //if (!result.IsSuccess) { }
            if (result.IsFalse)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }

            return View("MemberLogin");
        }

        public IActionResult EditPassword()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult EditPassword(MemberEditPasswordVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            var currentUserAccount = User.Identity.Name;
            Result result = ChangePassword(currentUserAccount, vm);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }
            return RedirectToAction("MyMemberIndex");
        }

        private Result ChangePassword(string account, MemberEditPasswordVM vm)
        {
            var salt = _hashUtility.GetSalt();
            var hashOrigPassword = HashUtility.ToSHA256(vm.OldPassword, salt);

            var memberInDb = _context.Members.FirstOrDefault(m => m.Account == account && m.EncryptedPassword == hashOrigPassword);
            if (memberInDb == null) return Result.Failure("找不到要修改的會員記錄");

            var hashPassword = HashUtility.ToSHA256(vm.NewPassword, salt);


            // 更新密碼
            memberInDb.EncryptedPassword = hashPassword;
            _context.SaveChanges();

            return Result.Success();
        }

        private Result ProcessChangePassword(string account, string confirmCode, string password)
        {

            // 驗證 memberId, confirmCode是否正確
            var memberInDb = _context.Members.FirstOrDefault(m => m.Account == account && m.ConfirmCode == confirmCode);
            if (memberInDb == null) return Result.Failure("找不到對應的會員記錄");

            // 更新密碼,並將 confirmCode清空
            var salt = _hashUtility.GetSalt();
            var encryptedPassword = HashUtility.ToSHA256(password, salt);

            memberInDb.EncryptedPassword = encryptedPassword;
            memberInDb.ConfirmCode = null;

            _context.SaveChanges();

            return Result.Success();
        }

        //會員是否存在,密碼雜湊,存進資料庫
        private Result IsMemberExist(MemberRegisterVM vm)
        {
            if (_context.Members.Any(m => m.Account == vm.Account))
            {
                // 丟出異常,或者傳回 Result
                return Result.Failure($"帳號 {vm.Email} 已存在, 請更換後再試一次");
            }

            // 將密碼進行雜湊
            var salt = _hashUtility.GetSalt();
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

        //有效登入
        private Result ValidLogin(MemberLoginVM vm)
        {
            var db = new RouteMasterContext();

            //Result resultAttempt = LockAccountIfFailedAttemptsExceeded(vm.Account);
            //if (resultAttempt.IsSuccess)
            //{
            //    return resultAttempt; // 帳號已鎖定，返回錯誤結果m
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
                    new Claim("Id", member.Id.ToString());
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            }
            //if(member.IsSuspended)  --有沒有停權

            var salt = _hashUtility.GetSalt();
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

        //上傳圖片
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

        //會員是否存在
        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //會員存進DB
        private Result RegisterMember(MemberRegisterVM vm, MemberImage img)
        {
            if (_context.Members.Any(m => m.Account == vm.Account))
            {
                // 丟出異常,或者傳回 Result
                return Result.Failure($"帳號 {vm.Email} 已存在, 請更換後再試一次");
            }

            Regex PasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$");
            string[] CommonPasswords = new string[]
            {
        "password",
        "123456",
        "qwerty",
                // 添加其他常見密碼
            };

            // 將密碼進行雜湊

            var salt = _hashUtility.GetSalt();
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
                Image = vm.Image,
                ConfirmCode = Guid.NewGuid().ToString("N"),
                IsConfirmed = false,
                IsSuspended = false,
                IsSuscribe = vm.IsSuscribe
            };

            // 將它存到db
            _context.Members.Add(member);
            _context.SaveChanges();
            img.MemberId = member.Id;
            _context.MemberImages.Add(img);
            _context.SaveChanges();

            return Result.Success();
        }

        //確認帳號發送更改密碼信件
        private Result ProcessResetPassword(string account, string email)
        {

            // 檢查account,email正確性
            var memberInDb = _context.Members.FirstOrDefault(m => m.Account == account);
            var myAccount = memberInDb.Account;

            if (memberInDb == null) return Result.Failure("帳號或 Email 錯誤"); // 故意不告知確切錯誤原因

            if (string.Compare(email, memberInDb.Email, StringComparison.CurrentCultureIgnoreCase) != 0) return Result.Failure("帳號或 Email 錯誤");

            // 檢查 IsConfirmed必需是true, 因為只有已啟用的帳號才能重設密碼
            if (memberInDb.IsConfirmed == false) return Result.Failure("您還沒有啟用本帳號, 請先完成才能重設密碼");

            // 更新記錄, 填入 confirmCode
            var confirmCode = Guid.NewGuid().ToString("N");
            memberInDb.ConfirmCode = confirmCode;
            _context.SaveChanges();

            var urlTemplate = $"{Request.Scheme}://{Request.Host.Value}/Members/MemberResetPassword?Account={myAccount}&confirmCode={confirmCode}";

            // 發email
            var url = string.Format(urlTemplate, memberInDb.Account, confirmCode);
            new EmailHelper().SendForgetPasswordEmail(url, memberInDb.FirstName, email);

            return Result.Success();
        }

    }
}
