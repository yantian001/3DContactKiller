using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour
{

    public float smoothly = 20f;
    Vector2 oldPos1, oldPos2;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Camera.main.fieldOfView += Input.mouseScrollDelta.magnitude / smoothly;
        //Debug.Log(Input.mouseScrollDelta.magnitude);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * smoothly;
        }
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                oldPos1 = Input.GetTouch(0).position;
                oldPos2 = Input.GetTouch(1).position;
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    var newPos1 = Input.GetTouch(0).position;
                    var newPos2 = Input.GetTouch(1).position;
                    float delta = Vector2.Distance(oldPos1, oldPos2) - Vector2.Distance(newPos1, newPos2);
                    Camera.main.fieldOfView += delta / smoothly;
                    oldPos1 = newPos1;
                    oldPos2 = newPos2;
                }
            }
        }
    }


}
