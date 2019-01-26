using Microsoft.Xna.Framework.Input;
using VAC;

namespace GraveyardShift
{
    internal class CharacterGenerationMenu : State
    {
        VirtualConsole screen;
        public CharacterGenerationMenu(StateManager manager, Virtual_root_Console root) : base(manager, root)
        {
            screen = root.AddConsole(60, 40);
            consoles.Add(screen);
            root.Print(screen, 5, 10, "You are a randomly generated test person...");
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            root.Clear(screen);
            root.Flush();
            base.OnExit();
        }

        public override void Update()
        {
            if (root.input.wasKeyPressed(Keys.Enter))
            { manager.PopState(); }
                base.Update();
        }
    }
}