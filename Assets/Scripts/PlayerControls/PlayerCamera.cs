// Player Camera
// version = '0.1.3'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Controls for the camera of the player 
//
// Changelog:
//   v0.1.3  + Initial release
//
// ToDo:
//   + ...

using Mondo.MLogging;
using UnityEngine;

namespace Mondo.Players
{
    public class PlayerCamera : MonoBehaviour
    {
        private MLogger _logger = new MLogger("PlayerCamera", false);
        private Camera playerCamera;
        private bool cameraEnabled = true;

        public bool Enabled
        {
            get { return cameraEnabled; }
            set { cameraEnabled = value; }
        }

        public void TurnOnCamera()
        {
            playerCamera.enabled = true;
            cameraEnabled = true;
        }

        public void TurnOffCamera()
        {
            playerCamera.enabled = false;
            cameraEnabled = false;
        }

        public Vector3 ScreenToWorldPoint(Vector3 position) {
            Vector3 screenPoint;

            screenPoint = playerCamera.ScreenToWorldPoint(position);
            _logger.Log($"screenPoint: {screenPoint}");

            return screenPoint;
        }

        public Vector3 WorldToScreenPoint(Vector3 position)
        {
            Vector3 worldPoint;

            worldPoint = playerCamera.WorldToScreenPoint(position);
            _logger.Log($"worldPoint: {worldPoint}");

            return worldPoint;
        }

        public Ray ScreenPointToRay(Vector3 position)
        {
            Ray ray;

            ray = playerCamera.ScreenPointToRay(position);
            _logger.Log($"ray: {ray}");

            return ray;
        }

        void Start()
        {
            playerCamera = gameObject.GetComponent<Camera>();
        }

        private void Update()
        {
            if (cameraEnabled && (playerCamera.enabled == false))
            {
                TurnOnCamera();
            }

            if (!cameraEnabled && (playerCamera.enabled == true))
            {
                TurnOffCamera();
            }
        }
    }
}
