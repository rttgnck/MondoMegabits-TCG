// PlayerMovement
// version = '0.1.3'
// user = 'rttgnck'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Controls the look movement of the player camera and player body, as well as rotation.
//
// Changelog:
//   v0.1.3  + Initial release
//
// ToDo:
//   + Camera does not seem correct for keeping movement aligned, and allowing clicking on in gameobjects nicely.
//   + Camera can sometimes snap out to orientation, realigning mouse and view is broken.

using UnityEngine;
using Mondo.MLogging;

namespace Mondo.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Movement Settings")]
        [Tooltip("The body of the player")]
        [SerializeField]
        private CharacterController playerBody;

        [Tooltip("The camera of the player")]
        [SerializeField]
        private PlayerCamera playerCamera;

        [Tooltip("Mouse look around 'speed' (1-100)")]
        [SerializeField]
        private float mouseSensitivity = 50f;

        [Tooltip("Start angle of the camera, looks at table, down 45d from horizon")]
        [SerializeField]
        private float xRotation = 45f;

        [Tooltip("Player movement speed, controlls with the arrow keys or WASD")]
        [SerializeField]
        private float speed = 6f;

        //Custom Logger with [ClassName] log entry prefix
        private MLogger _logger = new MLogger("PlayerMovement");

        private void Start()
        {
            Cursor.visible = true;
        }

        void Update()
        {
            float mMoveX = Input.GetAxis("Horizontal");
            float mMoveZ = Input.GetAxis("Vertical");

            float mRotX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mRotY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            Vector3 move = playerBody.transform.right * mMoveX + playerBody.transform.forward * mMoveZ;

            xRotation -= mRotY;
            xRotation = Mathf.Clamp(xRotation, -30f, 70f);

            Quaternion rotate = Quaternion.Euler(xRotation, 0f, 0f);

            playerCamera.transform.localRotation = rotate;

            playerBody.transform.Rotate(Vector3.up * mRotX);
            playerBody.Move(move * speed * Time.deltaTime);
        }
    }
}

