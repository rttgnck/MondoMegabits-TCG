// Player UI Overlay
// version = '0.1.3'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Controls for the UI of the player 
//
// Changelog:
//   v0.1.3  + Initial release
//
// ToDo:
//   + Add control of the UI card slide-in/out
//   + Add card info to overlay

using Mondo.MLogging;
using Mondo.MondoCards;
using UnityEngine;

namespace Mondo.Players
{
    public class PlayerUiOverlay : MonoBehaviour
    {
        [Header("Player UI Overlay Settings")]
        [Tooltip("The MondoCard in the UI")]
        [SerializeField]
        private MondoCard_UI uiMondoCard;

        [Tooltip("This overlay that is also camera")]
        [SerializeField]
        private Camera uiCamera;

        private MLogger _logger = new MLogger("PlayerUiOverlay");

        public bool Enabled
        {
            get { return uiCamera.enabled; }
            set { uiCamera.enabled = value; }
        }

        public void TurnOnUI(MondoCard hoveredCard)
        {
            _logger.Log("TurnOnUI");

            Renderer renderer = uiMondoCard.GetComponent<Renderer>();
            renderer.materials[1].mainTexture = hoveredCard.GetComponent<Renderer>().materials[1].mainTexture;

            uiCamera.enabled = true;
            //uiMondoCard.SlideIn();
        }

        public void TurnOffUI()
        {
            _logger.Log("TurnOffUI");

            //uiMondoCard.SlideOut();

            uiCamera.enabled = false;

            Renderer renderer = uiMondoCard.GetComponent<Renderer>();
            renderer.materials[1].mainTexture = null;
        }
    }
}

