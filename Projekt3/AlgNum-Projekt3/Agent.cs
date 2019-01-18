using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgNum_Projekt3
{
    public class Agent
    {
        public State State { get; set; }

        public Agent(State state)
        {
            State = state;
        }

        public void ChangeStates(Agent secondAgent)
        {
            if (this.State == State.Yes && secondAgent.State == State.Undecided || this.State == State.Undecided && secondAgent.State == State.Yes)
            {
                if (this.State == State.Undecided)
                    this.State = State.Yes;
                else
                    secondAgent.State = State.Yes;
                return;
            }

            if (this.State == State.Yes && secondAgent.State == State.No || this.State == State.No && secondAgent.State == State.Yes)
            {
                this.State = State.Undecided;
                secondAgent.State = State.Undecided;
                return;
            }

            if (this.State == State.No && secondAgent.State == State.Undecided || this.State == State.Undecided && secondAgent.State == State.No)
            {
                if (secondAgent.State == State.Undecided)
                    secondAgent.State = State.No;
                else
                    this.State = State.No;
                return;
            }
        }
    }

    public enum State
    {
        Yes,
        No,
        Undecided
    }
}
