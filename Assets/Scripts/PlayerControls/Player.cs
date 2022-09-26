// Player
// version = '0.1.0'
// user = 'rttgnck'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Simple settings for the player 
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...


using UnityEngine;
using Mondo.Controls;
using Mondo.MLogging;

namespace Mondo.Players
{
    public class Player : MonoBehaviour
    {
        [Header("Player Settings")]
        [Tooltip("The camera of the player")]
        [SerializeField]
        private PlayerCamera playerCamera;

        [Tooltip("Set the start look angle of the camera")]
        [SerializeField]
        private float playerCameraAngle;

        private Player player;

        private MondoHand hand;

        private MLogger _logger;

        public bool SetupPlayer(string playerName) {
            _logger = new MLogger(playerName, false);
            _logger.Log($"SETTING UP {playerName}");

            player = gameObject.GetComponent<Player>();

            playerCamera.transform.localEulerAngles = new Vector3(playerCameraAngle, 0, 0);

            GameObject playerGameAreaGO = Instantiate(Resources.Load("Prefabs/PlayerGameArea", typeof(GameObject))) as GameObject;
            playerGameAreaGO.name = "PlayerGameArea";

            playerGameAreaGO.transform.parent = player.transform;

            PlayerGameArea playerGameArea = playerGameAreaGO.GetComponent<PlayerGameArea>();

            foreach (Transform goTrans in playerCamera.transform) {
                if (goTrans.name == "MondoHand") {
                    hand = goTrans.GetComponent<MondoHand>();
                    hand.handOwner = playerName;
                    _logger.Log($"{hand.name}");
                }
            }

            bool completeGameAreaSetup = playerGameArea.SetupPlayerGameArea(player, playerCamera, hand, playerName);

            _logger.Log($"COMPLETED SETTING UP {playerName}");

            if (completeGameAreaSetup) {
                return true;
            } else {
                return false;
            }
        }
    }
}
