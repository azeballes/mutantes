using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mutants.Api.Request;
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
            if (_mutant.IsMutant(dna.Dna))
                return StatusCode(200);            
            return StatusCode(403);
        }
    }
}
