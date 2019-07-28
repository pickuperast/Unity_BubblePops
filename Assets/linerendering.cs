using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class linerendering : MonoBehaviour
{

    UILineRenderer UILR;
    // Start is called before the first frame update
    void Start()
    {
        UILR = gameObject.GetComponent<UILineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.GetTouch(0);
        UILR.Points[1] = touch.position;
        UILR.Points[2] = touch.position;
        UILR.Points[3] = touch.position;
        if (Input.GetMouseButtonDown(0))
        {
            UILR.Points[1] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}
