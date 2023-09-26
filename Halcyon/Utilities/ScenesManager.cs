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
        private List<IScene> _activeScenes = new List<IScene>();

        public IEnumerable<IScene> ActiveScenes => _activeScenes;

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
        public void LoadScenes(List<string> listOfScenestoLoad)
        {
            foreach (var x in listOfScenestoLoad)
            {
                if (_scenes.ContainsKey(x) && !_activeScenes.Contains(_scenes[x]))
                {
                    _scenes[x].LoadContent(null);
                    _activeScenes.Add(_scenes[x]);
                }
            }
        }


        /// <summary>
        /// the list of scenes to unload
        /// </summary>
        /// <param name="listOfScenesToUnload">the list of scenes to unload</param>
        public void UnloadScenes(List<string> listOfScenesToUnload)
        {
            foreach (var x in listOfScenesToUnload)
            {
                if (_scenes.ContainsKey(x) && _activeScenes.Contains(_scenes[x]))
                {
                    //unload the content??
                    _activeScenes.Remove(_scenes[x]);
                }
            }
        }
    }
}
