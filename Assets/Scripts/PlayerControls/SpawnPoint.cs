// SpawnPoint
// version = '0.1.0'
// repo = 'MondoMegabits-TCG'
// Author(s): rttgnck
// Description: Just rotates a spawn point in space.
//
// Changelog:
//   v0.1.0  + Initial release
//
// ToDo:
//   + ...

using UnityEngine;

namespace Mondo.Players
{
    public class SpawnPoint : MonoBehaviour
    {
        float rotationsPerMinute = 5.0f;

        public Vector3 Location
        {
            get { return transform.localPosition; }
        }

        void Update()
        {
            transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
        }
    }
}

