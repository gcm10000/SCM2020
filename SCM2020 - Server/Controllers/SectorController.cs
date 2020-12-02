using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorController : ControllerBase 
    {
        ControlDbContext context;
        UserManager<ApplicationUser> userManager;
        public SectorController(UserManager<ApplicationUser> userManager, ControlDbContext context)
        { this.userManager = userManager; this.context = context; }
        [HttpGet]
        public IActionResult ShowSectors()
        {
            var sectors = context.Sectors.ToList();
            var AdminSector = sectors.SingleOrDefault(x => x.NumberSector == 98);
            if (AdminSector != null)
                sectors.Remove(AdminSector);
            return Ok(sectors);
        }
        [HttpGet("Showall")]
        public IActionResult ShowAll()
        {
            return Ok(context.Sectors.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var list = context.Sectors.Find(id);
            return Ok(list);
        }
        [HttpPost("Add")]
        public async Task<ActionResult<ModelsLibraryCore.Response>> Create()
        {
            var raw = await Helper.RawFromBody(this);
            var sector = new Sector(raw);
            if (context.Sectors.Any(
                x => (x.NumberSector == sector.NumberSector)))
                return BadRequest("Número do setor já existente.");
            context.Sectors.Add(sector);
            await context.SaveChangesAsync();
            var sectoroutput = context.Sectors.Single(x => (x.NameSector == sector.NameSector) && (x.NumberSector == sector.NumberSector));
            //return Ok($"Setor {sector.NumberSector} - {sector.NameSector} adicionado com sucesso.");
            return new ModelsLibraryCore.Response() { Id = sectoroutput.Id, Message = $"Setor {sector.NumberSector} - {sector.NameSector} adicionado com sucesso." };
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var sector = new Sector(raw);
            sector.Id = id;
            //ATUALIZAR TODAS AS CLAIMS TAMBÉM
            var listUsers = await userManager.GetUsersForClaimAsync(new System.Security.Claims.Claim(ClaimTypes.Role, sector.NameSector));
            foreach (var user in listUsers)
            {
                var claims = await userManager.GetClaimsAsync(user);
                foreach (var claim in claims)
                {
                    if (claim.Type == ClaimTypes.Role)
                    {
                        await userManager.RemoveClaimAsync(user, claim);
                        Claim sectorClaim = new Claim(ClaimTypes.Role, sector.NameSector);
                        await userManager.AddClaimAsync(user, sectorClaim);
                    }
                }
            }
            context.Sectors.Update(sector);
            await context.SaveChangesAsync();
            return Ok("Setor atualizada com sucesso.");
        }
        [HttpGet("NumberSector/{numberSector}")]
        public IActionResult NumberSector(int numberSector)
        {
            var sector = context.Sectors.SingleOrDefault(x => x.NumberSector == numberSector);
            return Ok(sector);
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var sector = context.Sectors.Find(id);
            var list = await userManager.GetUsersForClaimAsync(new System.Security.Claims.Claim("Occupation", sector.NameSector));

            if (list.Count > 0)
                return BadRequest("Existe funcionários neste setor. Atualize e tente novamente.");
            var listUsers = await userManager.GetUsersForClaimAsync(new System.Security.Claims.Claim(ClaimTypes.Role, sector.NameSector));
            foreach (var user in listUsers)
            {
                var claims = await userManager.GetClaimsAsync(user);
                foreach (var claim in claims)
                {
                    if (claim.Type == ClaimTypes.Role)
                    {
                        await userManager.RemoveClaimAsync(user, claim);
                    }
                }
            }
            context.Sectors.Remove(sector);
            await context.SaveChangesAsync();
            return Ok("Setor removido com sucesso.");
        }

    }
}
