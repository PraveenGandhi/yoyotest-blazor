using Microsoft.AspNetCore.Mvc;
using YoYoTest.Server.Services;
using YoYoTest.Shared.Models;

namespace YoYoTest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AthleteController : ControllerBase
    {
        private readonly IAthleteService _athleteService;

        public AthleteController(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_athleteService.GetAllAthletes());
        }

        [HttpPost]
        [Route("Warn/{id}")]
        public IActionResult Warn(int id)
        {
            var athlete = _athleteService.WarnAthlete(id);
            return Ok(athlete);
        }

        [HttpPost]
        [Route("SaveResult")]
        public IActionResult SaveResult([FromBody] Athlete athlete)
        {
            athlete = _athleteService.SaveResult(athlete.Id, athlete.Level, athlete.Shuttle);
            return Ok(athlete);
        }
    }
}