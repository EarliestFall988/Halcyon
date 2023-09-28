using Lib.Collision;
using Lib.GUI_Elements;
using Lib.PleasingTweening;
using Lib.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lib.Scenes
{
    /// <summary>
    /// The main menu scene
    /// </summary>
    public class MainMenuScene : IScene
    {
        public GraphicsDeviceManager _graphics { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public ContentManager Content { get; set; }

        public GameObjectPool GameObjectPool { get; private set; }

        public Camera Camera { get; private set; } = new();

        public DebugHelper helper;


        public static readonly string p_KenneyGenericObjects = "Kenny Generic Objects/Spritesheet/genericItems_spritesheet_colored";
        public static readonly string p_PlatformerRedux = "Kenney Platformer Redux/Spritesheets/spritesheet_complete";
        public static readonly string p_PlatformerReduxBackground = "Kenney Platformer Redux/PNG/Backgrounds/blue_grass";
        public static readonly string p_bg32x48 = "32x48button";
        public static readonly string p_font_arial = "arial";

        private Texture2D _genericObjectsAtlas;
        private Texture2D _pixelGUIAtlas;
        private Texture2D _platformerReduxAtlas;
        private Texture2D _platformerReduxBackground;

        private SpriteFont _fontArial;

        public static readonly string p_DebugBox = "Halcyon Debug Prmtvs/128_debug_box";
        public static readonly string p_DebugCircle = "Halcyon Debug Prmtvs/Debug Circle";
        public static readonly string p_4px_dot = "Halcyon Debug Prmtvs/4px_dot";

        private Texture2D _debugbox;
        private Texture2D _debugcircle;
        private Texture2D _debugDot;
        private bool _drawVisualGizmos = false; // set to true to see the collision gizmos


        public int Id => 0;

        public string Name => "Main Menu";

        #region interface contract variables

        public bool Enabled => false;

        public int UpdateOrder => 0;

        public int DrawOrder => 0;

        public bool Visible => true;



        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        #endregion

        public MainMenuScene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _graphics = graphics;
            SpriteBatch = spriteBatch;
            Content = contentManager;
        }



        public void Initialize()
        {
            GameObjectPool = new GameObjectPool(SpriteBatch, Camera);


            Camera.HandheldCameraShakeEnabled = true;
            Camera.HandheldCameraShakeAmount = 2;
            Camera.HandheldCameraShakeFrequency = 3;
            Camera.CharacterCameraOffset = new Vector2(-200, -200);
        }

        #region content init

        public void LoadButtons()
        {

            var playButton = GameObjectPool.SpawnObject(new Button("Play", new Rectangle(0, 0, 48 / 2, 32 / 2), _fontArial), new Vector2(100, 100), 0, new Vector2(16 / 16) / 2);
            playButton.Atlas = _pixelGUIAtlas;
            playButton.transform.scaleValue = 5;
            playButton.ButtonClickAction = () =>
            {
                GameManager.scenesManager.UnloadScenesByName(new List<string>() {"Main Menu"}, true);
                GameManager.scenesManager.LoadScenesByName(new List<string>() { "First Scene" }, Content);
            };

            var exitButton = GameObjectPool.SpawnObject(new Button("Exit", new Rectangle(0, 0, 48 / 2, 32 / 2), _fontArial), new Vector2(100, 200), 0, new Vector2(16 / 16) / 2);
            exitButton.Atlas = _pixelGUIAtlas;
            exitButton.transform.scaleValue = 5;
        }

        #endregion

        public void LoadContent(ContentManager contentManager)
        {
            _debugbox = Content.Load<Texture2D>(p_DebugBox);
            _debugcircle = Content.Load<Texture2D>(p_DebugCircle);
            _debugDot = Content.Load<Texture2D>(p_4px_dot);
            _pixelGUIAtlas = Content.Load<Texture2D>(p_bg32x48);

            _fontArial = Content.Load<SpriteFont>(p_font_arial);

            helper = new DebugHelper(_debugbox, _debugcircle, _debugDot, SpriteBatch, Camera);
            helper.showGizmos = _drawVisualGizmos;

            _platformerReduxAtlas = Content.Load<Texture2D>(p_PlatformerRedux);
            _platformerReduxBackground = Content.Load<Texture2D>(p_PlatformerReduxBackground);

            LoadButtons();

        }

        public void UnloadContent()
        {
            GameObjectPool.Clear();
            DebugHelper.Clear();
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Draw(gameTime);
            }

            DebugHelper.Main.Draw(gameTime);

            SpriteBatch.End();
        }


        public void Update(GameTime gameTime)
        {
            Tweening.Update(gameTime);

            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Update(gameTime);
            }

            var gameObjects = GameObjectPool.AllObjects.FindAll(x => x.colliders.Count > 0 && x.Enabled);
            List<IGameObjectCollision> cols = new List<IGameObjectCollision>();

            Camera.UpdateCamera(gameTime);
        }
    }
}
