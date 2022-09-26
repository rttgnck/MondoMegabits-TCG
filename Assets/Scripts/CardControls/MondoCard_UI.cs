// MondoCard UI Element
// version = '0.1.3'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Controls for the UI card element 
//
// Changelog:
//   v0.1.3  + Initial release
//
// ToDo:
//   + Animated slide card in and out when the uiCamera is enabled, and when disabled

using UnityEngine;
using Mondo.Players;
using Mondo.MLogging;
using System.Collections;

namespace Mondo.MondoCards
{
    public class MondoCard_UI : MonoBehaviour
    {
        public PlayerUiOverlay uiOverlay;

        private GameObject uiMondoCard;
        //private bool uiShowing = true;

        //public bool mondoHover = false;

        private MLogger _logger = new MLogger("MondoCard_UI");

        void Start()
        {
            _logger.Log($"[MondoCard_UI] Start");
            uiMondoCard = gameObject;
            //SlideOut();
        }

        public void SlideIn()
        {
            _logger.Log($"[MondoCard_UI] SLIDE_IN");
            //if (!uiShowing)
            //{
            Vector3 tempPos = uiMondoCard.transform.localPosition;
            tempPos.x = -0.2f;
            uiMondoCard.transform.localPosition = tempPos;
            //uiShowing = true;
            //}
        }

        public void SlideOut()
        {
            _logger.Log($"[MondoCard_UI] SLIDE_OUT");
            //if (uiShowing)
            //{
            Vector3 tempPos = uiMondoCard.transform.localPosition;
            tempPos.x = -0.5f;
            uiMondoCard.transform.localPosition = tempPos;
            //uiShowing = false;
            //}
        }

        //private IEnumerator UiTimer() {
        //    yield return new WaitForSeconds(5f);
        //    SlideOut();
        //    yield return new WaitForSeconds(2f);
        //    uiOverlay.Enabled = false;
        //}

        //void Update()
        //{
        //    if (mondoHover) {
        //        StartCoroutine(UiTimer());
        //    }
        //}
    }
}

