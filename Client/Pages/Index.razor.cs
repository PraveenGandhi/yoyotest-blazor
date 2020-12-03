using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using YoYoTest.Client.Utils;
using YoYoTest.Shared.Models;

namespace YoYoTest.Client.Pages
{
    public partial class Index
    {
        public Index()
        {
            Timer = new Timer(1000);
            Timer.Elapsed += (s, e) => RunShuttles();
        }

        [Inject] private HttpClient HttpClient { get; set; }

        #region AppState

        private Timer Timer { get; }
        private Rating CurrentShuttle { get; set; } = new Rating();
        private Rating PreviousShuttle { get; set; }
        private bool IsStarted { get; set; }
        private bool IsCompleted { get; set; }
        private float Percent { get; set; }
        private string TimeLapsed { get; set; } = "00:00";
        private string NextShuttleText { get; set; } = "Next <br>Shuttle";
        private string NextShuttleIn { get; set; } = "00:00";
        private string TotalDistance { get; set; } = "0";
        private int Counter { get; set; }
        private int CounterDown { get; set; }
        private Athlete[] Athletes { get; set; }
        private Rating[] RatingData { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            Athletes = await HttpClient.GetFromJsonAsync<Athlete[]>("Athlete");
            RatingData = await HttpClient.GetFromJsonAsync<Rating[]>("fitnessrating_beeptest.json");
        }

        private void Start()
        {
            IsStarted = true;
            RunShuttles();
            Timer.Start();
        }

        private async void Warn(Athlete athlete)
        {
            athlete.Warn = true;
            await HttpClient.PostAsync($"/athlete/Warn/{athlete.Id}", null);
        }

        private void Stop(Athlete athlete)
        {
            StopAthlete(athlete, true);
        }
        
        private async void OnResultChange(string value, Athlete athlete)
        {
            var results = value.Split('-');
            athlete.Level = int.Parse(results[0]);
            athlete.Shuttle = int.Parse(results[1]);
            athlete.Result = value;
            await HttpClient.PostAsJsonAsync("/athlete/SaveResult", athlete);
        }

        private async void StopAthlete(Athlete athlete, bool byCoach = false)
        {
            athlete.Stop = true;
            var rating = byCoach ? PreviousShuttle : CurrentShuttle;
            if (rating != null) {
                athlete.Level = int.Parse(rating.SpeedLevel);
                athlete.Shuttle = int.Parse(rating.ShuttleNo);
                athlete.Result = athlete.Level + "-" + athlete.Shuttle;
            }
            else {
                athlete.Result = "";
            }
            await HttpClient.PostAsJsonAsync("/athlete/SaveResult", athlete);
            if (Athletes.Any(a => !a.Stop)) {
                StateHasChanged();
                return;
            }
            Percent = 100;
            Timer.Stop();
            IsCompleted = true;
            StateHasChanged();
        }

        private void RunShuttles()
        {
            TimeLapsed = Counter.ToTime();
            foreach (var (item, index) in RatingData.ToTuple()) {
                if (item.StartTime.ToSeconds() == Counter) {
                    CurrentShuttle = item;
                    if (index != 0) {
                        PreviousShuttle = RatingData[index - 1];
                        TotalDistance = PreviousShuttle.AccumulatedShuttleDistance;
                    }
                    Percent = (float) index * 100 / RatingData.Length;
                    SetNextShuttleCountDown(index);
                }
                else if (RatingData.Last().CommulativeTime == TimeLapsed) {
                    Athletes.ToList().FindAll(a => !a.Stop).ForEach(a => StopAthlete(a));
                    TotalDistance = CurrentShuttle.AccumulatedShuttleDistance;
                    StateHasChanged();
                    break;
                }
            }
            CountDown();
            Counter++;
            StateHasChanged();
        }

        private void SetNextShuttleCountDown(int index)
        {
            CounterDown = CalculateCountDownTime(RatingData[index]);
            if (index != RatingData.Length - 1) return;
            CounterDown--;
            NextShuttleText = "Test <br>ends in";
        }

        private void CountDown()
        {
            NextShuttleIn = CounterDown.ToTime();
            if (CounterDown <= 0) {
                CounterDown = 0;
                NextShuttleIn = "00:00";
                return;
            }
            CounterDown--;
        }

        private static int CalculateCountDownTime(Rating rating)
        {
            return rating.CommulativeTime.ToSeconds() - rating.StartTime.ToSeconds() + 1;
        }

        private static bool IsSelected(Rating rating, Athlete athlete)
        {
            return rating.SpeedLevel.Equals(athlete.Shuttle.ToString())
                   && rating.ShuttleNo.Equals(athlete.Level.ToString());
        }
    }
}