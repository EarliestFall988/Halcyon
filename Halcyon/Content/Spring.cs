using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Lib.Content
{
    public class Spring
    {

        public GameCharacter Character { get; set; }
        public AtlasSprite SpringTexture { get; set; }
        public AtlasSprite Sprung { get; set; }
        public string Name { get; set; }


        public Spring(GameCharacter character, Vector2 position, AtlasSprite spring, AtlasSprite sprung, string name)
        {
            Character = character;
            Name = name;
            SpringTexture = spring;
            Sprung = sprung;

            spring.transform.position = position;
            sprung.transform.position = position;

            GameObjectPool.Main.SpawnObject(spring);
            GameObjectPool.Main.SpawnObject(sprung);
            sprung.Enabled = false;
        }

        public void Update(float dt)
        {
            if (Character.IsTouching(SpringTexture))
            {
                SpringTexture.Enabled = false;
                Sprung.Enabled = true;
            }

            if (Character.IsTouching(Sprung))
            {
                Sprung.Enabled = false;
                SpringTexture.Enabled = true;
            }

            Character.Bounce(5);
        }
    }
}
