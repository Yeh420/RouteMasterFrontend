using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.ViewModels.Comments_Accommodations;

namespace RouteMasterFrontend.Models.Infra.ExtenSions
{
	public static class Comments_AccommodationExts
	{
		public static Comments_AccommodationIndexImgVM ToImgList(this Comments_AccommodationImage entity)
		{
			return new Comments_AccommodationIndexImgVM
			{
				Id = entity.Id,
				CommentId = entity.Comments_AccommodationId,
				Image = entity.Image,

			};	
		}

		public static Comments_AccommodationIndexVM ToIndexVM(this Comments_Accommodation entity)
		{
			
			return new Comments_AccommodationIndexVM
			{
				Id = entity.Id,
				Account = entity.Member.Account,
				HotelName = entity.Accommodation.Name,
				Score = entity.Score,
				Pros = entity.Pros,
				Cons = entity.Cons,
				Title = entity.Title,
				CreateDate = entity.CreateDate,
				Status=entity.CommentStatus.Name,
				ReplyMesssage=entity.Reply,
				ReplyDate=entity.ReplyAt,
				ImageList = entity.Comments_AccommodationImages.ToList()
			};
		}
	}
}
