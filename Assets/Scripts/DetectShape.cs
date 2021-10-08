using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DetectShape: MonoBehaviour
{
    private List<Vector3> drawingPositions = new List<Vector3>();
    public Vector3 mousePos;
    public Vector3 lastmousePos;
    public Text shapeDisplay;
    private Vector3 zeroVector = new Vector3(1, 1, 0);
    private List<float> aux = new List<float>();

    [Header("Tolerancies")]
    public float minimumDistance = 10f;
    public float tolerance = 30f;
    private float distance = 0;
    public float startDelay = 4;
    private float delay = 0;

    [Header("Debugging")]
    public float pain = 0;
    public float angle;
    public float angleVariaton = 0;
    private float pendent;
    //private float last_angle = 0;
    private float raise, run;
    public List<float> angles_drawing = new List<float>();
    public List<float> angles_Variaton = new List<float>();


    [Header("Formes")]
    private bool newAngle = true;
    private float acum = 0;
    private float asum = 0;
    private float bacum = 0;
    private float basum = 0;
    private float arekt = 0;
    public float minAcum = 20;
    //public List<List<float>> listOfShapes = new List<List<float>>();
    //public int AliA;
    //public string AliB;

    [Header("Shape detecting")]
    private bool stopedDrawing;
    public static float correctShape = 0;
    public float angleTolerance = 10;

    //2 sided shapes:
    private float[] spike1 = { 60 };
    private float[] spike2 = { 120 };

    //3 sided shapes:
    private float[] triangle1 = { 120, 120 };
    private float[] capsa2 = { 90, 90 };

    //4 sided shapes:
    private float[] quadrat1 = { 90, 90, 90 };
    private float[] rellotge1 = { 120, 120, 120 };

    //5 sided shapes:
    private float[] pentagon1 = { 70, 70, 70, 70 };

    //public Shape quadrat;
    //public Shape triangle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (drawingMenu.isDrawing)
        {
            stopedDrawing = true;
            if (Input.GetMouseButtonDown(0))
            {
                correctShape = 0f;
                angles_drawing.Clear();
                angles_Variaton.Clear();
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
                    pendent = raise / run;
                    angle = Mathf.Atan(raise / run) * (180f / 3.1415f);

                    //Going from 90' to 360'
                    if (run < 0)
                    {
                        if (raise < 0)
                            angle += 180;
                        else
                            angle += 180;
                    }
                    else if (raise < 0)
                        angle = 360 + angle;

                    //Angle variation
                    angleVariaton = angle - arekt;
                    if (angleVariaton > 180)
                        angleVariaton -= 360;
                    if (angleVariaton < -180)
                        angleVariaton += 360;
                    //Angle detection
                    if (newAngle || Math.Abs(angleVariaton) < tolerance)
                    {
                        acum++;
                        asum += angle;
                        arekt = asum / acum;
                        newAngle = false;
                    }
                    else if (acum > minAcum)
                    {
                        if (Math.Abs(angleVariaton) > tolerance)
                            angles_drawing.Add(asum / acum);
                        basum = asum;
                        bacum = acum;
                        asum = angle;
                        acum = 1;
                        newAngle = true;
                    }
                    else
                    {
                        acum = 1;
                        asum = angle;
                        newAngle = true;
                    }
                    lastmousePos = mousePos;
                }
            }
        }
        else if (stopedDrawing)
        {
            stopedDrawing = false;
            angles_drawing.Add(asum / acum);
            detectVariationOnVertex(angles_drawing);
            CalculateShapeVariation(angles_Variaton);

            string Stext;
            Stext = correctShape.ToString("0.0");
            shapeDisplay.text = Stext;

            angles_Variaton.Clear();
            angles_drawing.Clear();
            delay = startDelay;
            newAngle = true;
            arekt = 0;
            asum = 0;
            acum = 0;
        }
    }

    void detectVariationOnVertex(List<float> angles)
    {
        for (int i = 0; i < angles.Count - 1; i++)
        {
            float angleVar = angles[i + 1] - angles[i];
            if (angleVar > 180)
                angleVar -= 360;
            if (angleVar < -180)
                angleVar += 360;

            angles_Variaton.Add(Math.Abs(angleVar));
        }
    }

    //Shape detecting 
    void CalculateShapeVariation(List<float> angles_Variaton_Shape)
    {
        int size = angles_Variaton_Shape.Count;
        switch (size)
        {
            case 1:
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - spike1[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 2.1f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                if (correctShape == 2.1f)
                    break;
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - spike2[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 2.2f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                break;

            case 2:
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - triangle1[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 3.1f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                if (correctShape == 3.1f)
                    break;
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - capsa2[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 3.2f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                break;

            case 3:
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - quadrat1[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 4.1f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                if (correctShape == 4.1f)
                    break;
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - rellotge1[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 4.2f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                break;

            case 4:
                for (int i1 = 0; i1 < size; i1++)
                {
                    float var = angles_Variaton_Shape[i1] - pentagon1[i1];
                    if (var > 180)
                        var -= 360;
                    if (var < -180)
                        var += 360;

                    if (Math.Abs(var) < angleTolerance)
                        correctShape = 4.1f;
                    else
                    {
                        correctShape = 0;
                        break;
                    }
                }
                break;
        }
    }

}


/*/First angle
if (angles_drawing.Count == 0)
{
delay--;
if (delay == 0)
{
    angles_drawing.Add(angle);
    last_angle = angle;
}
}

//The rest
else
{
angleVariaton = angle - last_angle;
if (angleVariaton > 180)
    angleVariaton -= 360;
if (angleVariaton < -180)
    angleVariaton += 360;

//angleVariaton = angle - last_angle;
//angleVariaton = (angleVariaton + 180) % 360 - 180;

if (Math.Abs(angleVariaton) > tolerance)
{
    angles_drawing.Add(angle);
    angles_Variaton.Add(Math.Abs(angleVariaton));
    last_angle = angle;
}
}

void CalculateShapeVariation(List<float> angles_Variaton_Shape)
{
for (int i = 0; i < angles_Variaton.Count; i++)
{
    float var = angles_Variaton[i] - angles_Variaton_Shape[i];
    if (var > 180)
        var -= 360;
    if (var < -180)
        var += 360;

    if (Math.Abs(var) < angleTolerance)
    {
        correctShape = true;
    }
    else
    {
        correctShape = false;
        break;
    }
}
}
*/
