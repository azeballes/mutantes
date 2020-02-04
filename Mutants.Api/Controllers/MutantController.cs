using Microsoft.AspNetCore.Mvc;
using Mutants.Api.Dtos;
using Mutants.Model;

namespace Mutants.Api.Controllers
{
    [ApiController]
    public class MutantController : ControllerBase
    {
        private readonly Mutant _mutant;
        public MutantController(Mutant mutant)
        {
            _mutant = mutant;
        }
        
        [HttpPost]
        [Route("[controller]")]
        public ActionResult IsMutant(DnaRequest dna)
        {
            return StatusCode(_mutant.IsMutant(dna.Dna) ? 200 : 403);
        }

        [HttpGet]
        [Route("stats")]
        public StatsResponse DoGet()
        {
            return new StatsResponse(_mutant.Stats());
        }
    }
}
