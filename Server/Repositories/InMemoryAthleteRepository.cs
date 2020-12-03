using System;
using System.Collections.Generic;
using System.Linq;
using YoYoTest.Shared.Models;

namespace YoYoTest.Server.Repositories
{
    public interface IAthleteRepository
    {
        IEnumerable<Athlete> GetAllAthletes();
        Athlete GetAthleteById(int athleteId);
        Athlete UpdateAthlete(Athlete athlete);
    }

    public class InMemoryAthleteRepository : IAthleteRepository
    {
        private readonly IEnumerable<Athlete> _athletes;

        public InMemoryAthleteRepository()
        {
            _athletes = new List<Athlete>
            {
                new Athlete {Id = 1, Name = "Praveen Gandhi"},
                new Athlete {Id = 2, Name = "Dan Mano"},
                new Athlete {Id = 3, Name = "Krishna Kumar"},
                new Athlete {Id = 4, Name = "Maa Sri"},
                new Athlete {Id = 5, Name = "M Pal"},
                new Athlete {Id = 6, Name = "Ram Ki"},
                new Athlete {Id = 8, Name = "K Prem"}
            };
        }

        public IEnumerable<Athlete> GetAllAthletes()
        {
            return _athletes;
        }

        public Athlete GetAthleteById(int athleteId)
        {
            var athlete = _athletes.FirstOrDefault(a => a.Id == athleteId);
            if (athlete == null) throw new Exception("Athlete not found");

            return athlete;
        }

        public Athlete UpdateAthlete(Athlete athlete)
        {
            var ath = GetAthleteById(athlete.Id);
            ath.Warn = athlete.Warn;
            ath.Stop = athlete.Stop;
            ath.Level = athlete.Level;
            ath.Shuttle = athlete.Shuttle;
            return athlete;
        }
    }
}