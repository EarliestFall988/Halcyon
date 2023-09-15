using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pleasing;
using Lib.PleasingTweening;

namespace Lib
{
    public class GameManager : Game
    {
        public static readonly string p_KenneyGenericObjects = "Kenny Generic Objects/Spritesheet/genericItems_spritesheet_colored";
        public static readonly string p_PlatformerRedux = "Kenney Platformer Redux/Spritesheets/spritesheet_complete";
        public static readonly string p_PlatformerReduxBackground = "Kenney Platformer Redux/PNG/Backgrounds/blue_grass";

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _genericObjectsAtlas;
        private Texture2D _platformerReduxAtlas;
        private Texture2D _platformerReduxBackground;

        public static ContentManager RootContent;
        public GameObjectPool GameObjectPool;

        public static GameTime Time;

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
            GameObjectPool = new GameObjectPool(_spriteBatch);


            base.Initialize();
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
        }


        void SetupCharacter()
        {
            GameCharacter character = new GameCharacter(new Vector2(128, 256) / 2)
            {
                Atlas = _platformerReduxAtlas,
                States = new Dictionary<string, CharacterState>()
                {
                    {
                        "idle", new CharacterState()
                        {
                            frames = new List<Rectangle>()
                            {
                                new Rectangle(650,516,128,256),
                            }
                        }
                    },
                    {
                        "duck", new CharacterState()
                        {
                            frames = new List<Rectangle>()
                            {
                                new Rectangle(650,1548,128,256),
                            }
                        }
                    },
                    {
                        "hit", new CharacterState()
                        {
                            frames = new List<Rectangle>()
                            {
                                new Rectangle(650,1032,128,256),
                            }
                        }
                    },
                    {
                        "jump", new CharacterState()
                        {
                            frames = new List<Rectangle>()
                            {
                                new Rectangle(650,774,128,256),
                            }
                        }
                    },
                    {
                        "walk", new CharacterState()
                        {
                            frames = new List<Rectangle>()
                            {
                                new Rectangle(520,1548,128,256),
                                new Rectangle(520,1290,128,256)
                            }
                        }
                    }
                }
            };
            character.transform.position = new Vector2(100, 0);
            character.transform.scaleValue = 1f;
            character.name = "character";
            character.transform.scaleValue = 0.5f;
        }


        void CreateGround()
        {
            for (int k = 0; k < 4; k++)
            {

                for (int i = 0; i < 15; i++)
                {
                    var atlasSpriteTest = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(128 * i / 2, 355 + 128 * k / 2), 0, Vector2.Zero);
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

        protected override void LoadContent()
        {
            //ExampleCodeToLoadAndRun();

            _platformerReduxAtlas = Content.Load<Texture2D>(p_PlatformerRedux);
            _platformerReduxBackground = Content.Load<Texture2D>(p_PlatformerReduxBackground);

            var backgroundSprite = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(0, 0), 0, Vector2.Zero);
            backgroundSprite.Atlas = _platformerReduxBackground;
            backgroundSprite.SpriteLocation = new Rectangle(0, 0, 1920, 1080);
            backgroundSprite.transform.scaleValue = 0.5f;

            var fence = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(128, 355 - 128 / 2), 0, Vector2.Zero);
            fence.Atlas = _platformerReduxAtlas;
            fence.SpriteLocation = new Rectangle(2210, 780, 128, 128);
            fence.transform.scaleValue = 0.5f;

            var fence2 = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(128 + 128 / 2, 355 - 128 / 2), 0, Vector2.Zero);
            fence2.Atlas = _platformerReduxAtlas;
            fence2.SpriteLocation = new Rectangle(2210, 780, 128, 128);
            fence2.transform.scaleValue = 0.5f;

            var fence1 = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(434, 355 - 128 / 2), 0, Vector2.Zero);
            fence1.Atlas = _platformerReduxAtlas;
            fence1.SpriteLocation = new Rectangle(2210, 650, 128, 128);
            fence1.transform.scaleValue = 0.5f;

            var bush = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(-5, 355 - 128 / 2), 0, Vector2.Zero);
            bush.Atlas = _platformerReduxAtlas;
            bush.SpriteLocation = new Rectangle(2210, 1690, 128, 128);
            bush.transform.scaleValue = 0.5f;

            var rock = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(569, 355 - 128 / 2), 0, Vector2.Zero);
            rock.Atlas = _platformerReduxAtlas;
            rock.SpriteLocation = new Rectangle(2080, 390, 128, 128);
            rock.transform.scaleValue = 0.5f;

            SetupCharacter();
            CreateGround();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            Tweening.Update(gameTime);

            // TODO: Add your update logic here

            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();



            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Draw(gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}