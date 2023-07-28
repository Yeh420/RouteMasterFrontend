using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteMasterBackend.Models;
using System.Diagnostics;

namespace RouteMasterBackend.Controllers
{
	//允許前台專案叫用
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class ActivitiesController : ControllerBase
	{
		private readonly RouteMasterContext _context;
        public ActivitiesController(RouteMasterContext context)
        {
            _context = context;
        }


		[HttpGet("{attractionId}")]
		public async Task<IEnumerable<Models.Activity>> GetActivities(int attractionId)
		{
			//todo無法完全非同步?
			return  _context.Activities.Where(a=>a.AttractionId==attractionId);
		}





    }
}
