using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pleasing;
using Lib.PleasingTweening;
using Lib.Utilities;
using Lib.Collision;
using Lib.Scenes;
using Microsoft.Xna.Framework.Media;

namespace Lib
{
    /// <summary>
    /// the Game Manager
    /// </summary>
    public class GameManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static ContentManager RootContent;

        public static GameTime Time;
        public static ScenesManager scenesManager;

        public Song song;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            RootContent = Content;
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            scenesManager = new ScenesManager();

            // add the scenes here...
            MainMenuScene mainMenuScene = new MainMenuScene(_graphics, _spriteBatch, Content);
            Game1Scene firstScene = new Game1Scene(_graphics, _spriteBatch, Content);



            // add the scenes to the manager
            scenesManager.AddScene(mainMenuScene);
            scenesManager.AddScene(firstScene);


            // load the first scene(s)
            scenesManager.LoadScenesByName(new List<string>() { "Main Menu" }, Content);


            song = Content.Load<Song>("Audio/angelic-interlude-by-tim-kulig-from-filmmusic-io");
            MediaPlayer.Play(song);
            //  Uncomment the following line will also loop the song
            MediaPlayer.IsRepeating = true;


            base.Initialize();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            scenesManager.UpdateLoadedScenes(gameTime);

            foreach(var x in scenesManager.Scenes)
            {
                if(x.Name != "Main Menu" && x.Loaded)
                {
                    MediaPlayer.Volume = 0.1f;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            scenesManager.DrawLoadedScenes(gameTime);
            base.Draw(gameTime);
        }
    }
}