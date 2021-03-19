using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace PSG.SpaceCargo.Networking
{
    public class LoginWindow : MonoBehaviourPunCallbacks
    {
        #region Serialized Variables

        [Tooltip("Input field for player name.")]
        [SerializeField]
        private TMP_InputField nameInputField;

        [Tooltip("Panel containing login fields and buttons.")]
        [SerializeField]
        private GameObject loginPanel;

        #endregion

        #region Private Variables

        RoomOptions defaultRoomOptions = new RoomOptions
        {
            IsOpen = true,
            IsVisible  = true,
            MaxPlayers = 4
        };

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            loginPanel.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && EventSystem.current.currentSelectedGameObject == nameInputField.gameObject)
                Login();
        }
        #endregion

        #region Photon CallBacks

        /// <summary>
        /// Called when we succesfully connect to master.
        /// </summary>
        public override void OnConnectedToMaster()
        {
            loginPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(nameInputField.gameObject);
        }

        /// <summary>
        /// Called when we fail to join a random room (no free room for us?).
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom($"{PhotonNetwork.NickName}'s room", defaultRoomOptions);
        }

        /// <summary>
        /// Called once we join a room.
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log($"Joined room {PhotonNetwork.CurrentRoom.Name}.");

            SceneManager.LoadScene(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to log in and join a room.
        /// </summary>
        public void Login()
        {
            PhotonNetwork.NickName = nameInputField.text;
            loginPanel.SetActive(false);

            PhotonNetwork.JoinRandomRoom();
        }

        #endregion
    }
}
