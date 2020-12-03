using System.Collections.Generic;
using YoYoTest.Server.Repositories;
using YoYoTest.Shared.Models;

namespace YoYoTest.Server.Services
{
    public interface IAthleteService
    {
        IEnumerable<Athlete> GetAllAthletes();
        Athlete WarnAthlete(int athleteId);
        Athlete SaveResult(in int athleteId, int level, int shuttle);
    }

    public class AthleteService : IAthleteService
    {
        private readonly IAthleteRepository _athleteRepository;

        public AthleteService(IAthleteRepository athleteRepository)
        {
            _athleteRepository = athleteRepository;
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return _athleteRepository.GetAllAthletes();
        }

        public Athlete WarnAthlete(int athleteId)
        {
            var athlete = _athleteRepository.GetAthleteById(athleteId);
            athlete.Warn = true;
            _athleteRepository.UpdateAthlete(athlete);
            return athlete;
        }

        public Athlete SaveResult(in int athleteId, int level, int shuttle)
        {
            var athlete = _athleteRepository.GetAthleteById(athleteId);
            athlete.Stop = true;
            athlete.Level = level;
            athlete.Shuttle = shuttle;
            return _athleteRepository.UpdateAthlete(athlete);
        }
    }
}