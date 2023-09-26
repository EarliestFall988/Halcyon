using Lib.Utilities;


namespace Lib.Dynamic_Game_Objects
{
    /// <summary>
    /// Door class
    /// </summary>
    public class Door
    {

        public GameEvent gameEvent { get; private set; }
        public string lastArgs { get; private set; } = "";


        public Door(string name, string description)
        {
            gameEvent = new GameEvent()
            {
                Name = name,
                Descripion = description,
                Event = (s) => DoorAction(s)
            };

            GameEventBus.Main.NewGameEvent(name, gameEvent);
        }

        public void DoorAction(string args)
        {
            lastArgs = args;
            DoorSetOpen(args);
        }


        protected virtual void DoorSetOpen(string args)
        {
            
        }
    }
}
