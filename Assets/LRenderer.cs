using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRenderer : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    public Camera camera;
    LineRenderer lr;
    Vector3 camOffset = new Vector3(0, 0, 10);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
            }
            lr.positionCount = 2;
            startPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lr.SetPosition(0, startPos);
            lr.useWorldSpace = true;
        }
        if (Input.GetMouseButton(0))
        {
            endPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lr.SetPosition(0, endPos);
        }
        if (Input.GetMouseButtonUp(0))
        {

        }
    }
}
