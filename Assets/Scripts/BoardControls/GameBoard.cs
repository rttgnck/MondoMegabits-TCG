using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private Plane plane;

    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Vector3.up, new Vector3(0, 0.8f, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        //if (plane.Raycast(ray, out enter))
        //{
        //    //Get the point that is clicked
        //    Vector3 hitPoint = ray.GetPoint(enter);

        //    //Move your cube GameObject to the point where you clicked
        //    m_Cube.transform.position = hitPoint;
        //}
    }
}
