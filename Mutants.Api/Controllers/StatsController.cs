using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mutants.Api.Dtos;

namespace Mutants.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        [HttpGet]
        public async Task<StatsResponse> DoGet()
        {
            return new StatsResponse();
        }
    }
}