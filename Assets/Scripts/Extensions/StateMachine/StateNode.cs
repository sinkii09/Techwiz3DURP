using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState state, IPredicate condition)
        {
            Transitions.Add(new Transition(state, condition));
        }
    }

