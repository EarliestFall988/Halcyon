using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{
    /// <summary>
    /// The scene of the game.
    /// </summary>
    public interface IScene : IGameComponent, IUpdateable, IDrawable
    {
        GraphicsDeviceManager _graphics { get; }
        SpriteBatch SpriteBatch { get; }
        ContentManager Content { get; }
        GameObjectPool GameObjectPool { get; }
        Camera Camera { get; }

        public int Id { get; }
        public string Name { get; }

        /// <summary>
        /// Load the content
        /// </summary>
        /// <param name="contentManager"></param>
        void LoadContent(ContentManager contentManager);

        /// <summary>
        /// Unload the content
        /// </summary>
        void UnloadContent();
    }
}
