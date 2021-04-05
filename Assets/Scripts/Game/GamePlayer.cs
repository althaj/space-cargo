using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.Game
{
    public class GamePlayer
    {
        #region Public fields
        public Player Player { get; set; }
        #endregion

        #region Constructors
        public GamePlayer(Player player)
        {
            Player = player;
        }
        #endregion
    }
}
