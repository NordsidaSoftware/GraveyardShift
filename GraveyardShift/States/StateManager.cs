using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    public class State
    {
       
        internal StateManager manager;

        internal List<VirtualConsole> consoles;
        internal Virtual_root_Console root;

        public State(StateManager manager, Virtual_root_Console root)
        {
            this.manager = manager;
            this.root = root;
            consoles = new List<VirtualConsole>();
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() {  }
        public virtual void Update() { }
        public virtual void Draw()
        {
            foreach (VirtualConsole con in consoles)
            {
                root.Blit(con, 0, 0, con.screen_width, con.screen_height, root, con.X_Offset, con.Y_Offset);
            }
        }

    }

    public class StateManager
    {

        private Stack<State> stack;
        private Game game;
        Virtual_root_Console root;

        public StateManager(Game game, Virtual_root_Console root)
        { 
            this.game = game;
            this.root = root;
            stack = new Stack<State>();
          

            InitializeStates();

        }

        private void InitializeStates()
        {
            
            PushState(new MainMenuState(this, root, 60, 40));
           
        }

        public bool Update()
        {
            root.Update();

            if (stack.Count > 0)
            {
                stack.Peek().Update();
                return true;
            }
            return false;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (stack.Count > 0)
            stack.Peek().Draw();

            root.Draw(spriteBatch);
        }

        public void PushState(State newState)
        {
                stack.Push(newState);
                stack.Peek().OnEnter();
        }

        public State PopState()
        {
            if (stack.Count > 0)
            {
                stack.Peek().OnExit();
            }
            return stack.Pop();
        }
    }
}
