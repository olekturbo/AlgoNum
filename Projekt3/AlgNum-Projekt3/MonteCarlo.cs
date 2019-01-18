using System;
using System.Linq;

namespace AlgNum_Projekt3
{
    public class MonteCarlo
    {
        public int Iterations { get; set; }

        public int TotalAgents { get; set; }

        public Agent[] Agents { get; set; }

        public double[] FinalVector { get; set; }

        public MonteCarlo(int iterations, int totalAgents)
        {
            Iterations = iterations;
            TotalAgents = totalAgents;
            Agents = new Agent[TotalAgents];

            int size = 0;
            for (int i = 1; i <= TotalAgents + 1; i++)
                size += i;
            FinalVector = new double[size];

            int yesCount = 0, noCount = 0;
            for (int i = 0; i < size; i++)
            {
                FinalVector[i] = RunSimulation(yesCount, noCount);
                if (yesCount == TotalAgents)
                    continue;
                if (yesCount + noCount == TotalAgents)
                {
                    yesCount++;
                    noCount = 0;
                }
                else
                    noCount++;
            }
        }

        private Agent[] SetAgents(int yesCount, int noCount, int undecidedCount)
        {
            Agent[] agents = new Agent[TotalAgents];
            int index;
            bool isSet;
            Random randomizer = new Random();

            for (var i = 0; i < yesCount; i++)
            {
                isSet = false;
                while (!isSet)
                {
                    index = randomizer.Next(agents.Length);
                    if (agents[index] != null)
                        continue;
                    agents[index] = new Agent(State.Yes);
                    isSet = true;
                }
            }

            for (var i = 0; i < noCount; i++)
            {
                isSet = false;
                while (!isSet)
                {
                    index = randomizer.Next(agents.Length);
                    if (agents[index] != null)
                        continue;
                    agents[index] = new Agent(State.No);
                    isSet = true;
                }
            }

            if (undecidedCount > 0)
            {
                for (var i = 0; i < undecidedCount; i++)
                {
                    isSet = false;
                    while (!isSet)
                    {
                        index = randomizer.Next(agents.Length);
                        if (agents[index] != null)
                            continue;
                        agents[index] = new Agent(State.Undecided);
                        isSet = true;
                    }
                }
            }

            return agents;
        }

        private double RunSimulation(int yesCount, int noCount)
        {
            double yesVotes = 0;

            for (int i = 0; i < Iterations; i++)
            {
                Agent[] agents = SetAgents(yesCount, noCount, TotalAgents - yesCount - noCount);

                if (SimulateVoting(agents) == State.Yes)
                    yesVotes++;
            }

            return yesVotes / Iterations;
        }

        private State SimulateVoting(Agent[] agents)
        {
            bool simulationFinished = false;
            
            //Random randomizer = new Random();
            while (!simulationFinished)
            {
                //Interesting case.
                //Agent firstAgent = agents.ElementAt(randomizer.Next(0, Agents.Length));
                //Agent secondAgent = agents.ElementAt(randomizer.Next(0, Agents.Length));

                Agent firstAgent = agents.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                Agent secondAgent = agents.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

                firstAgent.ChangeStates(secondAgent);

                var yesCount = CountStates(agents, State.Yes);
                var noCount = CountStates(agents, State.No);
                var undecidedCount = CountStates(agents, State.Undecided);

                if (yesCount == TotalAgents || noCount == TotalAgents || undecidedCount == TotalAgents)
                    simulationFinished = true;
            }

            return agents.First().State;
        }

        private static int CountStates(Agent[] agents, State state)
        {
            return agents.Count(agent => agent.State == state);
        }
    }
}
