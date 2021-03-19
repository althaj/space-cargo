using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using TMPro;
using PSG.SpaceCargo;
using UnityEngine.SceneManagement;

namespace PSG.SpaceCargo.Networking
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        #region Serialized Variables

        [Tooltip("Container with the players names.")]
        [SerializeField]
        private Transform playerListContainer;

        [Tooltip("Text of the ready button.\rThis is used for switching the text from 'Ready' to 'Not ready'.")]
        [SerializeField]
        private TMP_Text readyButtonText;

        [Tooltip("Container with the start button and it's tooltip.")]
        [SerializeField]
        private GameObject startGameContainer;

        #endregion

        #region Private Variables

        private GameObject playerNameTemplate;

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            playerNameTemplate = playerListContainer.GetChild(0).gameObject;
            playerNameTemplate.SetActive(false);

            startGameContainer.SetActive(false);

            NetworkHelpers.SetCustomProperty(Constants.PLAYER_READY_STATE, false);
        }

        #endregion

        #region Photon CallBacks

        /// <summary>
        /// Called when a custom property was changed.
        /// </summary>
        /// <param name="targetPlayer"></param>
        /// <param name="changedProps"></param>
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (changedProps.ContainsKey(Constants.PLAYER_READY_STATE))
                ReloadPlayersListUI();
        }

        /// <summary>
        /// Called when new player joins a room.
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (newPlayer.CustomProperties.ContainsKey(Constants.PLAYER_READY_STATE))
                ReloadPlayersListUI();
        }

        /// <summary>
        /// Called when a player leaves a room.
        /// </summary>
        /// <param name="otherPlayer"></param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            ReloadPlayersListUI();

            if (otherPlayer.IsLocal)
                SceneManager.LoadScene(0);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reload the UI with the list of players and their ready state.
        /// </summary>
        private void ReloadPlayersListUI()
        {
            for (int i = 1; i < playerListContainer.childCount; i++)
                Destroy(playerListContainer.GetChild(i).gameObject);

            bool allPlayersReady = true;

            foreach (var roomPlayer in PhotonNetwork.CurrentRoom.Players)
            {
                // Get the player.
                Player player = roomPlayer.Value;

                // Copy the template object.
                GameObject playerNameObject = Instantiate(playerNameTemplate);
                playerNameObject.transform.SetParent(playerListContainer);
                playerNameObject.SetActive(true);

                // Set the name of the object, as well as the text.
                string playerName = player.NickName;
                if (player.IsLocal)
                    playerName += " (you)";
                playerNameObject.name = playerName;
                playerNameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = playerName;

                // Set the visibility of the ready check box.
                bool isReady = (bool)player.CustomProperties[Constants.PLAYER_READY_STATE];
                playerNameObject.transform.GetChild(1).gameObject.SetActive((bool)player.CustomProperties[Constants.PLAYER_READY_STATE]);
                if (!isReady)
                    allPlayersReady = false;
            }

            startGameContainer.SetActive(allPlayersReady);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Switch the local player's ready state. The UI is reloaded once the property is changed on the current player.
        /// </summary>
        public void SwitchReadyState()
        {
            bool isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties[Constants.PLAYER_READY_STATE];
            NetworkHelpers.SetCustomProperty(Constants.PLAYER_READY_STATE, !isReady);

            readyButtonText.text = isReady ? "Ready" : "Not ready";
        }

        #endregion
    }
}
