namespace YoYoTest.Shared.Models
{
    public class Athlete
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Warn { get; set; }
        public bool Stop { get; set; }
        public int Level { get; set; }
        public int Shuttle { get; set; }

        public string Result { get; set; }
    }
}