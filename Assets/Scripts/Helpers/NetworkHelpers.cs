using Photon.Pun;
using Photon.Realtime;
using PSG.SpaceCargo.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.SpaceCargo
{
    public static class NetworkHelpers
    {
        #region Extension methods

        /// <summary>
        /// Set a player custom property.
        /// </summary>
        /// <param name="key">Key of the property in the CustomProperties hashtable.</param>
        /// <param name="value">Value of the custom property.</param>
        public static void SetCustomProperty(this Player player, string key, object value)
        {
            Debug.Log($"Setting custom property {key} to value {value}");

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(key, value);

            player.SetCustomProperties(properties);
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Set a local player custom property.
        /// </summary>
        /// <param name="key">Key of the property in the CustomProperties hashtable.</param>
        /// <param name="value">Value of the custom property.</param>
        public static void SetCustomProperty(string key, object value)
        {
            Debug.Log($"Setting local player custom property {key} to value {value}");

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(key, value);

            PhotonNetwork.SetPlayerCustomProperties(properties);
        }

        /// <summary>
        /// Set a current room custom property.
        /// </summary>
        /// <param name="key">Key of the property in the CustomProperties hashtable.</param>
        /// <param name="value">Value of the custom property.</param>
        public static void SetRoomProperty(string key, object value)
        {
            Debug.Log($"Setting room custom property {key} to value {value}");

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(key, value);

            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }

        /// <summary>
        /// Save hexes to room properties as array of their titles.
        /// </summary>
        /// <param name="hexes">List of hexes used in this game.</param>
        public static void SaveHexesToRoom(List<HexData> hexes)
        {
            SetRoomProperty(Constants.USED_HEXES, hexes.Select(x => x.Title).ToArray());
        }
        #endregion
    }
}
