// PlayerGameArea
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Holds all interactive elements of the game board for each player to interact with
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

using Mondo.MLogging;
using Mondo.Controls;
using UnityEngine;

namespace Mondo.Players
{
    public class PlayerGameArea : MonoBehaviour
    {
        private MLogger _logger;

        private string playerOwner = "";

        private PlayerGameArea playerGameArea;

        public bool SetupPlayerGameArea(Player player, PlayerCamera playerCamera, MondoHand hand, string playerName)
        {
            _logger = new MLogger(playerName, false);
            _logger.Log("SETTING UP PLAYER GAME AREA");
            
            GameObject playerGameAreaGO = gameObject;
            playerGameArea = playerGameAreaGO.GetComponent<PlayerGameArea>();

            playerOwner = playerName;

            if (playerOwner == "PlayerTwo")
            {
                playerGameArea.transform.eulerAngles = new Vector3(0, 180f, 0);
            }

            MondoDeck mondoDeck = new MondoDeck();
            Exit exitBtn = new Exit();

            foreach (Transform go in playerGameArea.transform) {
                if (go.name == "MondoDeck") {
                    mondoDeck = go.gameObject.GetComponent<MondoDeck>();
                }

                if (go.name == "Exit") {
                    exitBtn = go.gameObject.GetComponent<Exit>();
                }
            }

            bool completExitSetup = exitBtn.SetupExit(player, playerGameArea, playerCamera, hand, playerName);
            bool completeDeckSetup = mondoDeck.SetupDeck(player, playerCamera, hand, playerName);

            if (completeDeckSetup) {
                GameObject gameBoard = GameObject.Find("GameBoard");
                playerGameArea.transform.parent = gameBoard.transform;
                playerGameArea.transform.localPosition = new Vector3(0, 0, 0);
                playerGameArea.transform.localScale = new Vector3(1, 1, 1);

                _logger.Log("COMPLETE SETTING UP PLAYER GAME AREA");

                return true;
            } else {
                return false;
            }
        }
    }
}
