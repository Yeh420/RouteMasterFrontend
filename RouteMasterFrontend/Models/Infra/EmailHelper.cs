using System.Net.Mail;
using System.Net;
using System.Text;

namespace RouteMasterFrontend.Models.Infra
{
	//直接貼，版本有差異可能需要修正
	public class EmailHelper 
	{
		private readonly string senderEmail = "routemaster888@gmail.com"; // 寄件者

		public void SendForgetPasswordEmail(string url, string name, string email)
		{
			var subject = "[重設密碼通知]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>我要重設密碼</a>], 以進行重設密碼, 如果您沒有提出申請, 請忽略本信, 謝謝";

			var from = senderEmail;
			var to = email;

			SendFromGmail(from, to, subject, body);
		}

		public void SendConfirmRegisterEmail(string url, string name, string email, int Id, string Confirmcode)
		{
			var subject = "[新會員確認信]";

			var urlWithParams = $"{url}?memberId={Id}&Confirmcode={Confirmcode}";

			//var body = $@"Hi {name},
			//<br />
			//請點擊此連結 <a href='{url}?Id={Id}&Confirmcode={Confirmcode}'>的確是我申請會員</a>，如果您沒有提出申請，請忽略本信，謝謝";

			var body = $@"Hi {name},
<br />
請點擊此連結 <a href='{urlWithParams}'>的確是我申請會員</a>，如果您沒有提出申請，請忽略本信，謝謝";

			var from = senderEmail;
			var to = email;

			SendFromGmail(from, to, subject, body);
		}

		public void SendUppaidNotification(string name, string email)
		{
			var subject = "[Route Master - 付款提醒通知]";
			var body = $@"Hi {name},
<br/>
感謝您的訂單！
<br/>
我們很高興能為您提供服務。根據我們的記錄，您的訂單尚未付款。為了確保您能順利獲得所訂購的商品/服務，我們請您盡快完成付款程序。
";
			var from = senderEmail;
			var to = email;

			SendFromGmail(from, to, subject, body);


		}
		public virtual void SendFromGmail(string from, string to, string subject, string body)
		{
			// todo 以下是開發時,測試之用, 只是建立text file, 不真的寄出信
			//google帳號只能用第一段帳號認證的帳號
			//var path = HttpContext.Current.Server.MapPath("~/Uploads/");
			//CreateTextFile(path, from, to, subject, body);


			// 以下是實作程式, 可以視需要真的寄出信, 或者只是單純建立text file,供開發時使用
			// ref https://dotblogs.com.tw/chichiblog/2018/04/20/122816
			var smtpAccount = from;

			// TODO 請在這裡填入密碼,或從web.config裡讀取
			var smtpPassword = "jjkdoisrtceagkdt";

			var smtpServer = "smtp.gmail.com";
			var SmtpPort = 587;

			var mms = new MailMessage
			{
				From = new MailAddress(smtpAccount),
				Subject = subject,
				Body = body,
				IsBodyHtml = true,
				SubjectEncoding = Encoding.UTF8
			};
			mms.To.Add(new MailAddress(to));

			using (var client = new SmtpClient(smtpServer, SmtpPort))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential(smtpAccount, smtpPassword);//寄信帳密 
				client.Send(mms); //寄出信件
			}
		}

		private void CreateTextFile(string path, string from, string to, string subject, string body)
		{
			var fileName = $"{to.Replace("@", "_")} {DateTime.Now:yyyyMMdd_HHmmss}.txt";
			var fullPath = Path.Combine(path, fileName);

			var contents = $@"from:{from}
to:{to}
subject:{subject}

{body}";
			File.WriteAllText(fullPath, contents, Encoding.UTF8);
		}
	}
}
