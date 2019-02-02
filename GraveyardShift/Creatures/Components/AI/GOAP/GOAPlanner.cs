using System;
using System.Collections.Generic;

namespace GraveyardShift
{
    [Serializable]
    public class Node
    {
        public Node Parent { get; set; }
        public int Cost { get; set; }
        public Dictionary<string, object> State { get; set; }
        public GOAP_action Action { get; set; }

        public Node (Node Parent, int Cost, Dictionary<string, object> State, GOAP_action Action)
        {
            this.Parent = Parent;
            this.Cost = Cost;
            this.State = State;
            this.Action = Action;
        }
        
    }

    [Serializable]
    public class GOAPlanner
    {
        private FSM fsm;

        public GOAPlanner(FSM fsm)
        {
            this.fsm = fsm;
        }


        internal Queue<GOAP_action> MakePlan(Dictionary<string, object> goals, Dictionary<string, object> world_state, List<GOAP_action> allActions)
        {
            Queue<GOAP_action> plan = new Queue<GOAP_action>();

            // 1. Reset all actions
            foreach ( GOAP_action action in allActions )
            { action.Reset(); }

            // 2. Check Procedural Preconditions - Initialize target etc.
            //    Add all available actions to list
            List<GOAP_action> availableActions = new List<GOAP_action>();
            foreach (GOAP_action action in allActions)
            {
                if (action.CheckProceduralPrecondition(fsm.owner)) { availableActions.Add(action); }
            }

            // 3. Make a start node
            Node start = new Node(null, 0, world_state, null);

            // 4. Make a node list
            List<Node> planningTreeLeaves = new List<Node>();

            // 5. Build a Graph of nodes from start node root to goal state leafs
            bool success = BuildGraph(start, planningTreeLeaves, availableActions, goals);

            if (!success)
            {
                return plan;  // returning empty plan...
            }

            ProcessTree();

            plan.Enqueue(planningTreeLeaves[0].Action);
            return plan;
        }

        private bool BuildGraph(Node start, List<Node> planningTreeLeaves,List<GOAP_action> availableActions, Dictionary<string, object> goals )
        {
            bool foundPlan = false;
            
            // 1. Check if any available actions preconditions are fulfilled and therfore can be tried
            foreach ( GOAP_action action in availableActions )
            {
                if ( InState(action.Preconditions, start.State))
                {
                    // 2. Apply actions effect on current state, and create new node with new values
                    Dictionary<string, object> currentState = PopulateState(start.State, action.Effects);
                    Node next = new Node(start, start.Cost + action.Cost, currentState, action);
                    // 3. Check if in goal state
                    if (InState(currentState, goals))
                    {
                        planningTreeLeaves.Add(next);
                        foundPlan = true;
                    }

                    else // Not yet in goal state, try another action
                    {
                        List<GOAP_action> subsetOfActions = new List<GOAP_action>();
                        foreach ( GOAP_action a in availableActions )
                        {
                            if (a.Equals(action)) continue;
                            subsetOfActions.Add(a);
                        }

                        bool foundNext = BuildGraph(next, planningTreeLeaves, subsetOfActions, goals); // recursive madness...

                        if ( foundNext)
                        {
                            foundPlan = true;
                        }
                    }

                }
            }


            return foundPlan;
        }

        private Dictionary<string, object> PopulateState(Dictionary<string, object> state, Dictionary<string, object> effects)
        {
            // 1. Create new state, and copy values from current state
            Dictionary<string, object> newState = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> KVP in state)
            {
                newState.Add(KVP.Key, KVP.Value);
            }

           // 2. Update and apply the effects of an action
            foreach (KeyValuePair<string, object> KVP in effects)
            {
                if ( state.ContainsKey(KVP.Key))
                {
                    newState[KVP.Key] = KVP.Value;
                }
                else
                {
                    newState.Add(KVP.Key, KVP.Value);
                }
            }

            return newState;                
        }

        private bool InState(Dictionary<string, object> test, Dictionary<string, object> state)
        {
            // returns true if all conditions in 'test' equals conditions in 'state'
            bool allMatch = true;
            foreach ( KeyValuePair<string, object> KVP in test)
            {
              //  if ( !state.ContainsKey(KVP.Key)) { allMatch = false; } // Key not in state

                if ( state.ContainsKey(KVP.Key))
                {
                    if ( ! state[KVP.Key].Equals(test[KVP.Key])) { allMatch = false; } // key in state, different values
                }
            }

            return allMatch;
        }

        private void ProcessTree()
        {
           
        }

       

      
    }
}