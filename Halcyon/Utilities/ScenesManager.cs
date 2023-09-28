using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Lib.Utilities
{
    /// <summary>
    /// A class that manages the scenes in the game
    /// </summary>
    public class ScenesManager
    {

        /// <summary>
        /// the list of active scenes
        /// </summary>
        private List<IScene> _loadedScenes = new List<IScene>();

        public IEnumerable<IScene> LoadedScenes => _loadedScenes;

        /// <summary>
        /// the dictionary of scenes
        /// </summary>
        private static Dictionary<string, IScene> _scenes = new Dictionary<string, IScene>();

        /// <summary>
        /// enumerate through the list of scenes
        /// </summary>
        public IEnumerable<IScene> Scenes => _scenes.Values;

        /// <summary>
        /// add a scene to the manager
        /// </summary>
        /// <param name="scene">the scene</param>
        public void AddScene(IScene scene)
        {
            _scenes.Add(scene.Name, scene);
        }

        /// <summary>
        /// remove the scene from the dictionary - this does not unload the content
        /// </summary>
        /// <param name="scene">the scene</param>
        /// <remarks>this does not unload the content</remarks>
        public void RemoveScene(IScene scene)
        {
            _scenes.Remove(scene.Name);
        }

        /// <summary>
        /// Does the scene manager contain a reference to the scene?
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsScene(string name)
        {
            return _scenes.ContainsKey(name);
        }

        /// <summary>
        /// Load the list of scenes
        /// </summary>
        /// <param name="listOfScenestoLoad">the list of scenes to unload</param>
        public void LoadScenesByName(List<string> listOfScenestoLoad, ContentManager contentManager)
        {
            foreach (var x in listOfScenestoLoad)
            {
                if (_scenes.ContainsKey(x) && !_loadedScenes.Contains(_scenes[x]))
                {
                    _scenes[x].Initialize();
                    _scenes[x].LoadContent(contentManager);
                    _loadedScenes.Add(_scenes[x]);
                }
            }
        }


        /// <summary>
        /// the list of scenes to unload
        /// </summary>
        /// <param name="hardUnload">unload all the content</param>
        /// <param name="listOfScenesToUnload">the list of scenes to unload</param>
        /// <remarks>
        /// The scenes can be unloaded, but the content will still be in memory if hardUnload is false.
        /// Because the scene system does not tag which content is used by which scene, it is up to the
        /// developer to manage the content.
        /// </remarks>
        public void UnloadScenesByName(List<string> listOfScenesToUnload, bool hardUnload)
        {
            foreach (var x in listOfScenesToUnload)
            {
                if (_scenes.ContainsKey(x) && _loadedScenes.Contains(_scenes[x]))
                {

                    if (hardUnload)
                    {
                        _scenes[x].UnloadContent();
                    }

                    _loadedScenes.Remove(_scenes[x]);
                }
            }
        }


        public void UpdateLoadedScenes(GameTime time)
        {
            for (int i = 0; i < _loadedScenes.Count; i++)
            {
                var scene = _loadedScenes[i];

                scene.Update(time);
            }
        }

        public void DrawLoadedScenes(GameTime time)
        {
            for (int i = 0; i < _loadedScenes.Count; i++)
            {
                var scene = _loadedScenes[i];

                scene.Draw(time);
            }
        }
    }
}
