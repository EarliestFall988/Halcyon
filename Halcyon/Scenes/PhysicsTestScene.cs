using Lib.Collision;
using Lib.GameObjectComponents;
using Lib.PleasingTweening;
using Lib.Utilities;
using Lib.WorldItems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Dynamics;

using Pleasing;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lib.Scenes
{
    /// <summary>
    /// A test scene to test the physics engine (aether physics 2d)
    /// </summary>
    public class PhysicsTestScene : IScene
    {
        public GraphicsDeviceManager _graphics { get; set; }

        public static readonly string p_KenneyGenericObjects = "Kenny Generic Objects/Spritesheet/genericItems_spritesheet_colored";
        public static readonly string p_PlatformerReduxBackground = "Kenney Platformer Redux/PNG/Backgrounds/blue_grass";
        public static readonly string p_PlatformerRedux = "Kenney Platformer Redux/Spritesheets/spritesheet_complete";

        private Texture2D _genericObjectsAtlas;
        private Texture2D _platformerReduxBackground;
        private Texture2D _platformerReduxAtlas;

        public SpriteBatch SpriteBatch { get; set; }

        public ContentManager Content { get; set; }

        public GameObjectPool GameObjectPool { get; set; }

        public Camera Camera { get; set; }

        public bool Loaded { get; set; }

        public int Id => 2;

        public string Name => "Physics Test Scene";

        public World World { get; set; } = new();

        public bool Enabled { get; set; }

        public int UpdateOrder => 0;

        public int DrawOrder => 0;

        public bool Visible => true;

        PhysicsCharacterController character;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        RigidBodyComponent rbodyTest;

        public PhysicsTestScene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _graphics = graphics;
            SpriteBatch = spriteBatch;
            Content = contentManager;
        }

        /// <summary>
        /// This is the example code to load and run
        /// </summary>
        private void ExampleCodeToLoadAndRun()
        {
            _genericObjectsAtlas = Content.Load<Texture2D>(p_KenneyGenericObjects);
            Debug.WriteLineIf(_genericObjectsAtlas == null, "Could not create the atlas");

            var atlasSpriteTest = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(100, 100), 0, new Vector2(52, 68) / 2);
            atlasSpriteTest.Atlas = _genericObjectsAtlas;
            atlasSpriteTest.SpriteLocation = new Rectangle(688, 393, 52, 68);

            var atlasSpriteTest1 = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(150, 105), 0, new Vector2(52, 68) / 2);
            atlasSpriteTest1.Atlas = _genericObjectsAtlas;
            atlasSpriteTest1.SpriteLocation = new Rectangle(688, 393, 52, 68);
            atlasSpriteTest1.transform.SetParent(atlasSpriteTest.transform);


            TweenTimeline timeline = Tweening.NewTimeline();
            var positionProperty = timeline.AddVector2(GameObjectPool.GameObjectsToUpdate[0].transform, "position");
            var rotationProperty = timeline.AddFloat(GameObjectPool.GameObjectsToUpdate[0].transform, "rotation");

            positionProperty.AddFrame(1000, new Vector2(100, 200), Easing.Back.InOut);
            positionProperty.AddFrame(2000, new Vector2(200, 200), Easing.Back.InOut);
            positionProperty.AddFrame(3000, new Vector2(100, 100), Easing.Back.InOut);

            rotationProperty.AddFrame(500, 0, Easing.Cubic.InOut);
            rotationProperty.AddFrame(1000, 3 * MathF.PI / 4, Easing.Cubic.InOut);
            rotationProperty.AddFrame(1500, MathF.PI / 2, Easing.Cubic.InOut);
            rotationProperty.AddFrame(2000, 0, Easing.Cubic.InOut);
            timeline.Loop = true;

            var physicsObjectTest = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(200, 200), 0, new Vector2(52, 68) / 2);
            physicsObjectTest.Atlas = _genericObjectsAtlas;
            physicsObjectTest.SpriteLocation = new Rectangle(688, 393, 52, 68);

            var rbody = physicsObjectTest.AddComponent<RigidBodyComponent>();
            rbody.body = rbody.world.CreateRectangle(52, 68, 1, physicsObjectTest.transform.position.ToAetherVector2(), bodyType: BodyType.Dynamic);
            rbody.body.LinearVelocity = new nkast.Aether.Physics2D.Common.Vector2(-20f * 100, 0);

            rbodyTest = rbody;
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Draw(gameTime);
            }

            if (DebugHelper.Main != null)
                DebugHelper.Main.Draw(gameTime);

            SpriteBatch.End();

            ///items that have their own sprite batch
            for (int i = 0; i < GameObjectPool.GameObjectsToDrawIndependently.Count; i++)
            {
                if (GameObjectPool.GameObjectsToDrawIndependently[i].Visible && GameObjectPool.GameObjectsToDrawIndependently[i].Enabled)
                    GameObjectPool.GameObjectsToDrawIndependently[i].Draw(gameTime);
            }
        }

        public void Initialize()
        {
            Camera = new();
            Camera.HandheldCameraShakeEnabled = true;

            Camera.HandheldCameraShakeEnabled = true;
            Camera.HandheldCameraShakeAmount = 2;
            Camera.HandheldCameraShakeFrequency = 3;
            Camera.CharacterCameraOffset = new Vector2(-200, -200);

            GameObjectPool = new GameObjectPool(SpriteBatch, Camera);
        }

        private void CreateBackground(int amt)
        {

            _platformerReduxBackground = Content.Load<Texture2D>(p_PlatformerReduxBackground);

            for (int i = 0; i < amt; i++)
            {
                var backgroundSprite = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2((1920 * i) - 50, -60), 0, Vector2.Zero);
                backgroundSprite.Atlas = _platformerReduxBackground;
                backgroundSprite.SpriteLocation = new Rectangle(0, 0, 1920, 1080);
                backgroundSprite.transform.scaleValue = 1f;
                backgroundSprite.WorldType = SpriteType.Background;
            }
        }

        public void CreateWalkPlane()
        {

            _platformerReduxAtlas = Content.Load<Texture2D>(p_PlatformerRedux);

            //var bdy = World.CreateEdge(new nkast.Aether.Physics2D.Common.Vector2(0, 355), new nkast.Aether.Physics2D.Common.Vector2(1920 * 5, 355));

            for (int k = 0; k < 4; k++)
            {

                for (int i = 0; i < 15; i++)
                {

                    var atlasSprite = new AtlasSprite();
                    atlasSprite.AddComponent<RigidBodyComponent>();
                    var rbody = atlasSprite.GetComponent<RigidBodyComponent>();
                    rbody.body = rbody.world.CreateRectangle(128, 128, 1, new nkast.Aether.Physics2D.Common.Vector2((128 * i / 2) - 128, 355 + 128 * k / 2), bodyType: BodyType.Static);

                    //var vec = new Vector2((128 * i / 2) - 128, 355 + 128 * k / 2);

                    //var vectorRes = new nkast.Aether.Physics2D.Common.Vector2(vec.X, vec.Y);




                    var atlasSpriteTest = GameObjectPool.SpawnObject(atlasSprite, new Vector2((128 * i / 2) - 128, 355 + 128 * k / 2), 0, Vector2.Zero);
                    atlasSpriteTest.Atlas = _platformerReduxAtlas;
                    if (k == 0)
                    {
                        atlasSpriteTest.SpriteLocation = new Rectangle(1560, 390, 128, 128);
                    }
                    else
                    {
                        atlasSpriteTest.SpriteLocation = new Rectangle(1690, 390, 128, 128);
                    }
                    atlasSpriteTest.transform.scaleValue = 0.5f;
                }
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            ExampleCodeToLoadAndRun();
            CreateBackground(5);
            CreateWalkPlane();

            var bdy = World.CreateEdge(new nkast.Aether.Physics2D.Common.Vector2(0, 500), new nkast.Aether.Physics2D.Common.Vector2(1920 * 5, 500));


            character = Character.CreatePhysicsPlayer(Content);
            Camera.TargetCharacter = character;
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            Tweening.Update(gameTime);



            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Update(gameTime);
            }

            for (int i = 0; i < GameObjectPool.GameObjectsToDrawIndependently.Count; i++)
            {
                if (GameObjectPool.GameObjectsToDrawIndependently[i].Visible && GameObjectPool.GameObjectsToDrawIndependently[i].Enabled)
                    GameObjectPool.GameObjectsToDrawIndependently[i].Update(gameTime);
            }

            Camera.UpdateCamera(gameTime); // camera shake and movement with the character
        }
    }
}
