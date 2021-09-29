using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectShape: MonoBehaviour
{
    public List<Vector3> drawingPositions = new List<Vector3>();
    public Vector3 mousePos;
    public Vector3 lastmousePos;
    private Vector3 zeroVector = new Vector3(1, 1, 0);

    [Header("Tolerancies")]
    public float minimumDistance = 100f;
    public float tolerance = 0.5f;
    private float distance = 0;

    [Header("Debugging")]
    public float pain = 0;
    public float angle;
    private float raise, run;
    public List<float> angles_drawing = new List<float>();
    
    

    // Update is called once per frame
    void Update()
    {
        if (drawingMenu.isDrawing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                angles_drawing.Clear();
                mousePos = Input.mousePosition;
                lastmousePos = mousePos;
            }
            else if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                distance = Vector3.Distance(mousePos, lastmousePos);
                if (distance >= minimumDistance)
                {
                    run = mousePos.x - lastmousePos.x;
                    raise = mousePos.y - lastmousePos.y;
                    angle = Mathf.Atan(raise / run)*(180f/3.1415f);
                    angles_drawing.Add(angle);
                    lastmousePos = mousePos;
                }
            }
        }
    }
}
