using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.ViewModels.Comments_Accommodations;

namespace RouteMasterFrontend.Models.Infra.ExtenSions
{
	public static class Comments_AccommodationExts
	{

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
				ReplyMessage=entity.Reply,
				ReplyDate=entity.ReplyAt,
				ImageList = entity.Comments_AccommodationImages.ToList()
			};
		}

		public static Comments_LikesFilterDTO ToFilterDto(this Comments_Accommodation entity)
		{
			return new Comments_LikesFilterDTO
			{
				MemberId = entity.Member.Id,
				CommentTitle = entity.Title
			};

        }
    }
}
