namespace RouteMasterFrontend.Models.Infra
{

	//直接貼，版本有差異可能需要修正
	public class Result
	{
		public bool IsSuccess { get; private set; }

		public bool IsFalse => !IsSuccess;

		public string? ErrorMessage { get; private set; }

		public static Result Success() => new Result { IsSuccess = true, ErrorMessage = null };

		public static Result Failure(string errormessage) => new Result { IsSuccess = false, ErrorMessage = errormessage };
	}
}
