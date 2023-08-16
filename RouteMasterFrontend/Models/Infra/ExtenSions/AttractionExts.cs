using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.ViewModels.AttractionVMs;

namespace RouteMasterFrontend.Models.Infra.ExtenSions
{
    public static class AttractionExts
    {
        public static AttractionIndexVM ToIndexVM(this AttractionIndexDto dto)
        {
            return new AttractionIndexVM
            {
                Id = dto.Id,
                AttractionCategory = dto.AttractionCategory,
                Name = dto.Name,
                Region = dto.Region,
                Town = dto.Town,
                Image = dto.Image,
                DescriptionText = dto.DescriptionText,
                Tags = dto.Tags,
                Score = dto.Score,
                ScoreCount = dto.ScoreCount,
                Hours = dto.Hours,
                HoursCount = dto.HoursCount,
                Price = dto.Price,
                PriceCount = dto.PriceCount,
                Clicks = dto.Clicks,
                ClicksInThirty = dto.ClicksInThirty,
            };
        }

        public static AttractionTopTenVM ToTopTenVM(this AttractionIndexDto dto)
        {
            return new AttractionTopTenVM
            {
                Id = dto.Id,
                AttractionCategory = dto.AttractionCategory,
                Name = dto.Name,
                Region = dto.Region,
                Town = dto.Town,
                Image = dto.Image,
                DescriptionText = dto.DescriptionText,
                Tags = dto.Tags,
                Score = dto.Score,
                ScoreCount = dto.ScoreCount,
                Hours = dto.Hours,
                HoursCount = dto.HoursCount,
                Price = dto.Price,
                PriceCount = dto.PriceCount,
                Clicks = dto.Clicks,
            };
        }

        public static AttractionDetailVM ToDetailVM(this AttractionDetailDto dto)
        {
            return new AttractionDetailVM
            {
                Id = dto.Id,
                AttractionCategory = dto.AttractionCategory,
                Name = dto.Name,
                Region = dto.Region,
                Town = dto.Town,
                Images = dto.Images,
                Description = dto.Description,
                Tags = dto.Tags,
                Score = dto.Score,
                ScoreCount = dto.ScoreCount,
                Hours = dto.Hours,
                HoursCount = dto.HoursCount,
                Price = dto.Price,
                PriceCount = dto.PriceCount,
                Clicks = dto.Clicks,
                Website = dto.Website,
            };
        }
    }
}
