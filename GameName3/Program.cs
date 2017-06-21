using System;

namespace GameName3
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game.Game1 game = new Game.Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

