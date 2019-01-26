using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using VAC;

namespace GraveyardShift
{
    internal class MainMenuState : State
    {
        private StateManager stateManager;
        private int width;
        private int height;
        VirtualConsole TopScreen;

        int hilite;
        
        VirtualConsole MainMenu;
        List<string> Options;

        bool generated;
        public WorldManager world { get; set; }
        public CreatureManager creatureManager { get; set; }

        public MainMenuState(StateManager manager, Virtual_root_Console root, int width, int height) : base(manager, root)
        {
            this.width = width;
            this.height = height;



            Options = new List<string>();
            Options.Add("Quit");
            Options.Add("Play");
            Options.Add("Generate World");
            generated = false;

            TopScreen =  root.AddConsole(width, 5);
            consoles.Add(TopScreen);

            root.SetForegroundColor(TopScreen, VAColor.LawnGreen);
            root.Print(TopScreen, width/2, 0, "MAIN MENU");

            MainMenu = root.AddConsole(width, 15);
            MainMenu.Y_Offset = TopScreen.screen_height+1;
            MainMenu.X_Offset = width / 2;
            consoles.Add(MainMenu);

            root.SetForegroundColor(MainMenu, VAColor.AntiqueWhite);
            hilite = 0;

        } 

        public override void Draw()
        {
            int line = 0;
            foreach (string key in Options)
            {
                if (line == hilite) { root.SetForegroundColor(MainMenu, VAColor.Red); }
                else { root.SetForegroundColor(MainMenu, VAColor.AntiqueWhite); }
                root.Print(MainMenu, 1, line, key);
                line++;
            }
            base.Draw();
        }

        public override void OnEnter()
        {
            hilite = 0;
            base.OnEnter();
        }

        public override void OnExit()
        {
            root.Clear(TopScreen);
            root.Flush();
            base.OnExit();
        }

        public override void Update()
        {
            
            if ( root.input.wasKeyPressed(Keys.Down)) { hilite++; if ( hilite > Options.Count-1) { hilite = 0; } }
            if ( root.input.wasKeyPressed(Keys.Up)) { hilite--; if (hilite < 0 ) { hilite = Options.Count - 1; } }

            if (root.input.wasKeyPressed(Keys.Enter))
            {
                string selected_option = Options[hilite];

                switch (selected_option)
                {
                    case "Quit": { manager.PopState(); break; }
                    case "Play":
                        {
                            if (generated)
                            {
                                manager.PushState(new PlayState(manager, world, creatureManager, root, 60, 40));
                            }
                            break;
                        }
                    case "Generate World":
                        {
                            manager.PushState(new GenerateWorldState(manager, this, root, 60, 40));
                            generated = true;                         
                            break;
                        }
                }
            }
        }

    }
}