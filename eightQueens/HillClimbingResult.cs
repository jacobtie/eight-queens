namespace hill_climbing_eight_queens
{
    public class HillClimbingResult
    {
        public string Type { get; set; }
        public bool Succeeded { get; set; }
        public int NumSteps { get; set; }
        public bool SidewaysMoves { get; set; }
        public int? NumRestarts { get; set; }
    }
}