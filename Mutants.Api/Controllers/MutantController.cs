using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mutants.Api.Dtos;
using Mutants.Model;

namespace Mutants.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MutantController : ControllerBase
    {
        private readonly Mutant _mutant;
        public MutantController(Mutant mutant)
        {
            _mutant = mutant;
        }
        
        [HttpPost]
        public async Task<ActionResult> IsMutant(DnaRequest dna)
        {
            return StatusCode(_mutant.IsMutant(dna.Dna) ? 200 : 403);
        }
    }
}
