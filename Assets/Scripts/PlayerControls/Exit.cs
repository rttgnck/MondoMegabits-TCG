// ExitButton
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Simple exit the player position and return to spectator 
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

using Mondo.MLogging;
using Mondo.Players;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Mondo.Controls {
    public class Exit : MonoBehaviour
    {
        private MLogger _logger = new MLogger("ExitButton", true);

        private Player _player;

        private PlayerGameArea _playerGameArea;

        private PlayerCamera _playerCamera;

        private GameObject quitPanel;
        private Button yesBtn;
        private Button noBtn;

        private MondoHand _hand;

        private string _playerName;

        private void OnMouseDown()
        {
            _logger.Log($"{Input.mousePosition} TRIGGERED {gameObject.name}");
            ExitGameConfirmation();
        }

        public bool SetupExit(Player player, PlayerGameArea playerGameArea, PlayerCamera playerCamera, MondoHand hand, string playerName)
        {
            _player = player;
            _playerGameArea = playerGameArea;
            _playerCamera = playerCamera;
            foreach (Transform _cam in _playerCamera.transform) {
                if (_cam.name == "Player UI Overlay") {
                    GameObject playerUiOverlay = _cam.gameObject;
                    foreach (Transform _ui in playerUiOverlay.transform)
                    {
                        if (_ui.name == "UI")
                        {
                            GameObject playerUI = _ui.gameObject;
                            foreach (Transform panel in playerUI.transform)
                            {
                                if (panel.name == "QuitPlaying")
                                {
                                    quitPanel = panel.gameObject;
                                    foreach (Transform trans in quitPanel.transform)
                                    {
                                        if (trans.name == "Yes")
                                        {
                                            yesBtn = trans.gameObject.GetComponent<Button>();
                                            yesBtn.onClick.AddListener(delegate { QuitPlaying(); });
                                        }

                                        if (trans.name == "No")
                                        {
                                            noBtn = trans.gameObject.GetComponent<Button>();
                                            noBtn.onClick.AddListener(delegate { ContinuePlaying(); });

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            _hand = hand;
            _playerName = playerName;

            return true;
        }

        private void QuitPlaying() {
            _logger.Log("Return to spectator");
            quitPanel.SetActive(false);

            _logger.Log($"{transform.name}", true);
            Vector3 _pointPos = transform.localPosition;
            Vector3 _playerPos = _player.transform.localPosition;

            _player.transform.localPosition = new Vector3(_playerPos.x + 0.25f, _playerPos.y, _pointPos.z);

            _player.GetComponent<Spectator>().enabled = true;
            _player.GetComponent<Player>().enabled = false;

            _player.transform.eulerAngles = new Vector3(0, 0, 0);

            GameObject playerCardsGO = GameObject.Find("PlayerCards");
            PlayerCards _playerCards = playerCardsGO.GetComponent<PlayerCards>();

            _playerCards.DestroyPlayerCards(_playerName);
            _hand.DestroyHandCards();
            Destroy(_playerGameArea.gameObject);
        }

        private void ContinuePlaying()
        {
            _logger.Log("Continue playing");
        }

        private void ExitGameConfirmation()
        {
            quitPanel.SetActive(true);

            //bool decision = EditorUtility.DisplayDialog(
            //  "Quit playing?", // title
            //  "Want to quit being a MondoMan, and let someone else try?", // description
            //  "Yes", // OK button
            //  "No" // Cancel button
            //);

            //if (decision)
            //{
            //    QuitPlaying();
            //}
            //else
            //{
            //    ContinuePlaying();
            //}
        }
    }
}

