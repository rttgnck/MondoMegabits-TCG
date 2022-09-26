// PlayerPoint
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Controls what happens when a player interacts with the player point 
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

using Mondo.MLogging;
using UnityEngine;

namespace Mondo.Players {
    public class PlayerPoint : MonoBehaviour
    {
        private MLogger _logger;

        private bool SetupNewPlayer(Player player, string playerName, float yRot) {
            _logger.Log($"SETTING UP {playerName} FROM {gameObject.name}");

            Vector3 _pointPos = transform.localPosition;
            Vector3 _playerPos = player.transform.localPosition;

            player.transform.localPosition = new Vector3(_pointPos.x, _playerPos.y, _pointPos.z);

            player.GetComponent<Spectator>().enabled = false;
            player.GetComponent<Player>().enabled = true;

            player.transform.eulerAngles = new Vector3(0, yRot, 0);

            bool completePlayerSetup = player.SetupPlayer(playerName);

            if (completePlayerSetup)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Start()
        {
            _logger = new MLogger(gameObject.name, false);
        }

        void OnTriggerEnter(Collider other)
        {
            _logger.Log($"{other.name} TRIGGERED {gameObject.name}");

            if (other.gameObject.CompareTag("Player"))
            {
                Player player = other.gameObject.GetComponent<Player>();

                float yRot = 0f;

                string playerName = "";

                if (gameObject.name == "PlayerOnePoint")
                {
                    yRot = 180f;
                    playerName = "PlayerOne";
                }

                if (gameObject.name == "PlayerTwoPoint")
                {
                    yRot = 0;
                    playerName = "PlayerTwo";
                }

                bool completeNewPlayerSetup = SetupNewPlayer(player, playerName, yRot);

                if (completeNewPlayerSetup)
                {
                    //the player is setup and ready to play
                    return;
                }
                else {
                    //the player did not get setup properly
                    return;
                }
            }
        }
    }
}

