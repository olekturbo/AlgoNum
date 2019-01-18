namespace AlgNum_Projekt3
{
    public class Protocol
    {
        public int TotalVoters { get; }
        public int YesVoters { get; }
        public int NoVoters { get; }
        public int UndecidedVoters { get; }

        public Protocol(int totalVoters, int yesVoters, int noVoters)
        {
            TotalVoters = totalVoters;
            YesVoters = yesVoters;
            NoVoters = noVoters;
            UndecidedVoters = TotalVoters - YesVoters - NoVoters;
        }
    }
}
