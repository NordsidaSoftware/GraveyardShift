using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VAC;

namespace GraveyardShift
{
   
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager;
        Virtual_root_Console root;
        Randomizer randomizer;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            randomizer = new Randomizer(1976);
        }

      
        protected override void Initialize()
        {
            root = new Virtual_root_Console(this, 100, 60);
            stateManager = new StateManager(this, root);
           
  
            base.Initialize();
        }

     
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            // TODO: use this.Content to load your game content here
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

      
        protected override void Update(GameTime gameTime)
        {
            if ( ! stateManager.Update() )
                Exit();

            base.Update(gameTime);
        }

      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            stateManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    //TODO :Make console screens know their center
}
