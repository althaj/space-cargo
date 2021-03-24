using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo
{
    public static class Constants
    {
        #region Player custom properties

        public static readonly string PLAYER_READY_STATE = "Ready";

        #endregion

        #region Editor

        public static readonly string DATABASE_PATH_KEY = "DatabasePath";
        public static readonly string DATABASE_PATH = "Assets/Database";
        public static readonly string HEX_PATH = "Assets/Database/Hexes";
        public static readonly string CARD_PATH = "Assets/Database/Cards";

        #endregion

        #region Scene management

        public static readonly int LOGIN_SCENE_NUMBER = 0;
        public static readonly int LOBBY_SCENE_NUMBER = 1;
        public static readonly int GAME_SCENE_NUMBER = 2;

        #endregion
    }
}
