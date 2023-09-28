using Lib.Collision;
using Lib.PleasingTweening;
using Lib.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Pleasing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Scenes
{
    public class Game1Scene : IScene
    {

        public GraphicsDeviceManager _graphics { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }
        public ContentManager Content { get; private set; }

        List<SoundEffect> SFX = new List<SoundEffect>();


        public static readonly string p_KenneyGenericObjects = "Kenny Generic Objects/Spritesheet/genericItems_spritesheet_colored";
        public static readonly string p_PlatformerRedux = "Kenney Platformer Redux/Spritesheets/spritesheet_complete";
        public static readonly string p_PlatformerReduxBackground = "Kenney Platformer Redux/PNG/Backgrounds/blue_grass";


        private Texture2D _genericObjectsAtlas;
        private Texture2D _platformerReduxAtlas;
        private Texture2D _platformerReduxBackground;

        public static readonly string p_DebugBox = "Halcyon Debug Prmtvs/128_debug_box";
        public static readonly string p_DebugCircle = "Halcyon Debug Prmtvs/Debug Circle";
        public static readonly string p_4px_dot = "Halcyon Debug Prmtvs/4px_dot";

        private Texture2D _debugbox;
        private Texture2D _debugcircle;
        private Texture2D _debugDot;
        private bool _drawVisualGizmos = false; // set to true to see the collision gizmos

        public GameObjectPool GameObjectPool { get; private set; }
        public Camera Camera { get; private set; } = new();

        public DebugHelper helper;

        public static GameTime Time;

        public AtlasSprite CoinsCollectedSprite;

        public bool Loaded { get; set; } = false;

        public Game1Scene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _graphics = graphics;
            SpriteBatch = spriteBatch;
            Content = contentManager;
        }


        public int Id => 1;

        public string Name => "First Scene";

        public bool Enabled => false;

        public int UpdateOrder => 0;

        public int DrawOrder => 0;

        public bool Visible => true;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Initialize()
        {
            GameObjectPool = new GameObjectPool(SpriteBatch, Camera);


            Camera.HandheldCameraShakeEnabled = true;
            Camera.HandheldCameraShakeAmount = 2;
            Camera.HandheldCameraShakeFrequency = 3;
            Camera.CharacterCameraOffset = new Vector2(-200, -200);
        }

        #region content initialization

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


        private GameObject SetupCharacter()
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

            character.name = "character";
            character.tag = new Tag("player");
            character.transform.scaleValue = 0.5f;
            character.WalkingSoundEffects = new List<SoundEffect>()
            {
                SFX[2],
                SFX[3],
                SFX[4],
                SFX[5],
            };

            character.GroundedSoundEffect = SFX[6];
            character.RunningBreathe = SFX[7];

            character.colliders.AddRange(new List<IGameObjectCollision>()
            {
                new BoundingCircle(character.transform.position, character.transform.scaleValue * 128 / 2, character),
                new BoundingCircle(character.transform.position + new Vector2(1, 40), character.transform.scaleValue * 128 / 2, character)
            });

            return character;
        }


        void BuildWalkPlane()
        {
            for (int k = 0; k < 4; k++)
            {

                for (int i = 0; i < 15; i++)
                {
                    var atlasSpriteTest = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2((128 * i / 2) - 128, 355 + 128 * k / 2), 0, Vector2.Zero);
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

        void BuildMass(Vector2 startingPoint, int width, int height, Texture2D textureAtlas, Rectangle spriteLocation, float scale = 0.5f)
        {
            for (int k = 0; k < height; k++)
            {
                for (int i = 0; i < width; i++)
                {
                    var atlasSpriteTest = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(startingPoint.X + (spriteLocation.Width * i * scale), startingPoint.Y + (spriteLocation.Height * k * scale)), 0, Vector2.Zero);
                    atlasSpriteTest.Atlas = textureAtlas;
                    atlasSpriteTest.SpriteLocation = spriteLocation;
                    atlasSpriteTest.transform.scaleValue = scale;
                }
            }
        }

        void CreateGround()
        {
            BuildWalkPlane();
            BuildMass(new Vector2(-128 * 4, -128 / 2), 8, 12, _platformerReduxAtlas, new Rectangle(1040, 780, 128, 128));
        }


        private void CreateBackground(int amt)
        {
            for (int i = 0; i < amt; i++)
            {
                var backgroundSprite = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2((1920 / 2 * i) - 50, -60), 0, Vector2.Zero);
                backgroundSprite.Atlas = _platformerReduxBackground;
                backgroundSprite.SpriteLocation = new Rectangle(0, 0, 1920, 1080);
                backgroundSprite.transform.scaleValue = 0.5f;
                backgroundSprite.WorldType = SpriteType.Background;
            }
        }

        private void CreateDetails()
        {

            CreateBackground(5);

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

            var coin = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(620, 355 - 128 / 2), 0, new Vector2(128, 128) / 2);
            coin.Atlas = _platformerReduxAtlas;
            coin.SpriteLocation = new Rectangle(2730, 0, 128, 128);
            coin.transform.scaleValue = 0.5f;
            coin.tag = new Tag("coin");
            coin.colliders.AddRange(new List<IGameObjectCollision>() { new BoundingCircle(coin.transform.position, coin.transform.scaleValue * 128 / 2, coin) });

            TweenTimeline timeline = Tweening.NewTimeline();
            var positionProperty = timeline.AddVector2(coin.transform, nameof(coin.transform.position));
            var rotationProperty = timeline.AddFloat(coin.transform, nameof(coin.transform.rotation));

            positionProperty.AddFrame(0, new Vector2(620, 200 - 128 / 3), Easing.Cubic.InOut);
            positionProperty.AddFrame(1000, new Vector2(620, 200 - 128 / 2), Easing.Cubic.InOut);
            positionProperty.AddFrame(2000, new Vector2(620, 200 - 128 / 3), Easing.Cubic.InOut);

            rotationProperty.AddFrame(0, 0, Easing.Back.InOut);
            rotationProperty.AddFrame(700, MathHelper.ToRadians(270), Easing.Back.InOut);
            rotationProperty.AddFrame(1200 * 2, 0, Easing.Back.InOut);

            timeline.Loop = true;

            var coin1 = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(250, 355 - 128 / 2), 0, new Vector2(128, 128) / 2);
            coin1.Atlas = _platformerReduxAtlas;
            coin1.SpriteLocation = new Rectangle(2730, 0, 128, 128);
            coin1.transform.scaleValue = 0.5f;
            coin1.tag = new Tag("coin");
            coin1.colliders.AddRange(new List<IGameObjectCollision>() { new BoundingCircle(coin1.transform.position, coin1.transform.scaleValue * 128 / 2, coin1) });

            TweenTimeline timeline1 = Tweening.NewTimeline();
            var positionProperty1 = timeline1.AddVector2(coin1.transform, nameof(coin1.transform.position));
            var rotationProperty1 = timeline1.AddFloat(coin1.transform, nameof(coin1.transform.rotation));

            positionProperty1.AddFrame(0, new Vector2(250, 310 - 128 / 3), Easing.Cubic.InOut);
            positionProperty1.AddFrame(1000, new Vector2(250, 310 - 128 / 2), Easing.Cubic.InOut);
            positionProperty1.AddFrame(2000, new Vector2(250, 310 - 128 / 3), Easing.Cubic.InOut);

            rotationProperty1.AddFrame(0, 0, Easing.Back.InOut);
            rotationProperty1.AddFrame(700, MathHelper.ToRadians(270), Easing.Back.InOut);
            rotationProperty1.AddFrame(1200 * 2, 0, Easing.Back.InOut);

            timeline1.Loop = true;
        }

        void CreateUI()
        {
            var playerFaceHUD = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(-128 / 2 + 30, -128 / 2 + 30), 0, Vector2.One * 128 / 2);
            playerFaceHUD.Atlas = _platformerReduxAtlas;
            playerFaceHUD.SpriteLocation = new Rectangle(2730, 780, 128, 128);
            playerFaceHUD.transform.scaleValue = 0.5f;
            playerFaceHUD.WorldType = SpriteType.UI;

            TweenTimeline timeline = Tweening.NewTimeline();
            var positionProperty = timeline.AddFloat(playerFaceHUD.transform, nameof(playerFaceHUD.transform.rotation));
            positionProperty.AddFrame(0, 0, Easing.Exponential.InOut);
            positionProperty.AddFrame(500, MathHelper.ToRadians(4), Easing.Exponential.InOut);
            positionProperty.AddFrame(1000, 0, Easing.Exponential.InOut);
            timeline.Loop = true;


            var coinsCollectedUINumber = GameObjectPool.SpawnObject(new AtlasSprite(), new Vector2(-128 / 2 + 90, -128 / 2 + 30), 0, Vector2.One * 128 / 2);
            coinsCollectedUINumber.Atlas = _platformerReduxAtlas;
            coinsCollectedUINumber.SpriteLocation = new Rectangle(2990, 910, 128, 128);
            coinsCollectedUINumber.transform.scaleValue = 0.5f;
            coinsCollectedUINumber.WorldType = SpriteType.UI;
            CoinsCollectedSprite = coinsCollectedUINumber;
            coinsCollectedUINumber.transform.SetParent(playerFaceHUD.transform);

            PlayerProgression_OnCurrentCollectedCoinsChanged(PlayerProgression.CurrentCollectedCoins);
            PlayerProgression.OnCurrentCollectedCoinsChanged += PlayerProgression_OnCurrentCollectedCoinsChanged;
        }

        private void PlayerProgression_OnCurrentCollectedCoinsChanged(int level)
        {
            switch (level)
            {
                case 1:
                    CoinsCollectedSprite.SpriteLocation = new Rectangle(2990, 650, 128, 128);
                    break;

                case 2:
                    CoinsCollectedSprite.SpriteLocation = new Rectangle(2470, 910, 128, 128);
                    break;

                case 3:
                    CoinsCollectedSprite.SpriteLocation = new Rectangle(2990, 520, 128, 128);
                    break;

                case 4:
                    CoinsCollectedSprite.SpriteLocation = new Rectangle(2990, 390, 128, 128);
                    break;

                case 5:
                    CoinsCollectedSprite.SpriteLocation = new Rectangle(2990, 260, 128, 128);
                    break;

                default:
                    CoinsCollectedSprite.SpriteLocation = new Rectangle(2990, 910, 128, 128);
                    break;
            }
        }


        #endregion

        public void LoadContent(ContentManager contentManager)
        {
            _debugbox = Content.Load<Texture2D>(p_DebugBox);
            _debugcircle = Content.Load<Texture2D>(p_DebugCircle);
            _debugDot = Content.Load<Texture2D>(p_4px_dot);

            helper = new DebugHelper(_debugbox, _debugcircle, _debugDot, SpriteBatch, Camera);
            helper.showGizmos = _drawVisualGizmos;

            SFX.AddRange(new List<SoundEffect>()
            {
                Content.Load<SoundEffect>("Audio/651515__1bob__grab-item"), //grab coin
                Content.Load<SoundEffect>("Audio/blizzard-by-tim-kulig-from-filmmusic-io"), //wind bckgnd
                Content.Load<SoundEffect>("Audio/23937__dkiller2204__foundation-sounds-for-in-search-of-the-fallen-star/422990__dkiller2204__sfxrunground1"), //locomotion
                Content.Load<SoundEffect>("Audio/23937__dkiller2204__foundation-sounds-for-in-search-of-the-fallen-star/422989__dkiller2204__sfxrunground2"), //locomotion
                Content.Load<SoundEffect>("Audio/23937__dkiller2204__foundation-sounds-for-in-search-of-the-fallen-star/422994__dkiller2204__sfxrunground3"), //locomotion
                Content.Load<SoundEffect>("Audio/23937__dkiller2204__foundation-sounds-for-in-search-of-the-fallen-star/422993__dkiller2204__sfxrunground4"), //locomotion
                Content.Load<SoundEffect>("Audio/23937__dkiller2204__foundation-sounds-for-in-search-of-the-fallen-star/422968__dkiller2204__sfxjumplandingground"), //landing
                Content.Load<SoundEffect>("Audio/23937__dkiller2204__foundation-sounds-for-in-search-of-the-fallen-star/422987__dkiller2204__sfxpantingafterrun"), //running breathe hard
            });



            var wind = SFX[1].CreateInstance();
            wind.IsLooped = true;
            wind.Play();
            wind.Volume = 0.125f;


            _platformerReduxAtlas = Content.Load<Texture2D>(p_PlatformerRedux);
            _platformerReduxBackground = Content.Load<Texture2D>(p_PlatformerReduxBackground);

            //z order applies here

            //ExampleCodeToLoadAndRun();

            CreateDetails();
            var character = SetupCharacter();
            CreateGround();
            CreateUI();


            Camera.TargetCharacter = character;
        }


        public void UnloadContent()
        {
            GameObjectPool.Clear();
            DebugHelper.Clear();
        }


        public void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();


            Tweening.Update(gameTime);


            // TODO: Add your update logic here

            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Update(gameTime);
            }

            var gameObjects = GameObjectPool.AllObjects.FindAll(x => x.colliders.Count > 0 && x.Enabled);
            List<IGameObjectCollision> cols = new List<IGameObjectCollision>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                cols.AddRange(gameObjects[i].colliders);
            }

            for (int i = 0; i < cols.Count; i++)
            {
                for (int k = 0; k < cols.Count; k++)
                {
                    if (cols[i] == cols[k])
                    {
                        continue;
                    }

                    var result = CollisionHelper.Collides(cols[i], cols[k]);

                    if (result.collision)
                    {
                        Debug.WriteLine(result.a.gameObject.tag.name + " collided with " + result.b.gameObject.tag.name);

                        if (result.a.gameObject.tag.name == "player" && result.b.gameObject.tag.name == "coin")
                        {
                            result.b.gameObject.SetActive(false);
                            PlayerProgression.IncrementCoinsCollected();
                            SFX[0].Play();
                        }
                    }
                }
            }

            Camera.UpdateCamera(gameTime); // camera shake and movement with the character
        }


        public void Draw(GameTime gameTime)
        {

            //_graphics.Clear(Color.CornflowerBlue);


            SpriteBatch.Begin();

            for (int i = 0; i < GameObjectPool.GameObjectsToUpdate.Count; i++)
            {
                GameObjectPool.GameObjectsToUpdate[i].Draw(gameTime);
            }

            DebugHelper.Main.Draw(gameTime);

            SpriteBatch.End();

        }
    }
}
