using Lib.Collision;
using Lib.GameObjectComponents;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Collision.Shapes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.WorldItems
{

    /// <summary>
    /// Handles details that pertain to the character throughout the game
    /// </summary>
    public class Character
    {

        private static readonly string p_PlatformerRedux = "Kenney Platformer Redux/Spritesheets/spritesheet_complete";

        /// <summary>
        /// Spawn the player
        /// </summary>
        /// <param name="Content">the content manager</param>
        /// <returns>returns the game character</returns>
        public static GameCharacterController CreatePlayer(ContentManager Content)
        {

            var playerTextures = Content.Load<Texture2D>(p_PlatformerRedux);

            List<SoundEffect> SFX = new List<SoundEffect>();


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

            GameCharacterController character = new GameCharacterController(new Vector2(128, 256) / 2)
            {
                Atlas = playerTextures,
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

            //adding physics component to the character
            character.AddComponent<RigidBodyComponent>();
            var rbody = character.GetComponent<RigidBodyComponent>();
            rbody.body = rbody.world.CreateCapsule(1, 1, 1);

            rbody.Add(new nkast.Aether.Physics2D.Dynamics.Fixture(new CircleShape(character.transform.scaleValue * 128 / 2, 1)));


            return character;
        }
    }
}
