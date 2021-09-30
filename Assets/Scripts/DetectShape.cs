using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectShape: MonoBehaviour
{
    private List<Vector3> drawingPositions = new List<Vector3>();
    public Vector3 mousePos;
    public Vector3 lastmousePos;
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
    private float last_angle = 0;
    private float raise, run;
    public List<float> angles_drawing = new List<float>();
    public List<float> angles_Variaton = new List<float>();
    

    [Header("Formes")]
    public List<List<float>> listOfShapes = new List<List<float>>();
    //public int AliA;
    //public string AliB;


    private bool stopedDrawing;
    public bool correctShape = false;
    public float angleTolerance = 10;
    //public Shape quadrat;
    //public Shape triangle;

    void Start()
    {

        float[] a1 = { 120, 120};
        float[] a2 = { 90, 90, 90 };
        float[] a3 = { 140, 140, 140, 140 };
        float[] a4 = { 30, 30, 30, 30, 30 };

        foreach (float shape in a2)
            aux.Add(shape);
        //listOfShapes.Add(aux);
        //aux.Clear();

        delay = startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (drawingMenu.isDrawing)
        {
            stopedDrawing = true;
            if (Input.GetMouseButtonDown(0))
            {
                angles_drawing.Clear();
                angles_Variaton.Clear();
                correctShape = false;
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

                    //First angle
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
                    lastmousePos = mousePos;
                }
            }
        }
        else if (stopedDrawing)
        {
            CalculateShapeVariation(aux);
            angles_Variaton.Clear();
            delay = startDelay;
        }
    }

    //Shape detecting 
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
}

/*
        string arr_ref = angles_Variaton.ToString();
        string arr_aline = angles_Variaton_Shape.ToString();

        int refSeqCnt = arr_ref.Length + 1;
        int alineSeqCnt = arr_ref.Length + 1;

        int[,] scoringMatrix = new int[alineSeqCnt, refSeqCnt];

        //Initialization Step - filled with 0 for the first row and the first column of matrix
        for (int i = 0; i < alineSeqCnt; i++)
        {
            scoringMatrix[i, 0] = 0;
        }

        for (int j = 0; j < refSeqCnt; j++)
        {
            scoringMatrix[0, j] = 0;
        }

        //Matrix Fill Step
        for (int i = 1; i < alineSeqCnt; i++)
        {
            for (int j = 1; j < refSeqCnt; j++)
            {
                int scroeDiag = 0;
                if (arr_ref.Substring(j - 1, 1) == arr_aline.Substring(i - 1, 1))
                    scroeDiag = scoringMatrix[i - 1, j - 1] + 2;
                else
                    scroeDiag = scoringMatrix[i - 1, j - 1] + -1;

                int scroeLeft = scoringMatrix[i, j - 1] - 2;
                int scroeUp = scoringMatrix[i - 1, j] - 2;

                int maxScore = Math.Max(Math.Max(scroeDiag, scroeLeft), scroeUp);

                scoringMatrix[i, j] = maxScore;
                AliA = maxScore;
            }
        }
        /*
        //Traceback Step
        char[] alineSeqArray = arr_ref.ToCharArray();
        char[] refSeqArray = arr_aline.ToCharArray();

        string AlignmentA = string.Empty;
        string AlignmentB = string.Empty;
        int m = alineSeqCnt - 1;
        int n = refSeqCnt - 1;
        while (m > 0 && n > 0)
        {
            int scroeDiag = 0;

            //Remembering that the scoring scheme is +2 for a match, -1 for a mismatch and -2 for a gap
            if (alineSeqArray[m - 1] == refSeqArray[n - 1])
                scroeDiag = 2;
            else
                scroeDiag = -1;

            if (m > 0 && n > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n - 1] + scroeDiag)
            {
                AlignmentA = refSeqArray[n - 1] + AlignmentA;
                AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                m = m - 1;
                n = n - 1;
            }
            else if (n > 0 && scoringMatrix[m, n] == scoringMatrix[m, n - 1] - 2)
            {
                AlignmentA = refSeqArray[n - 1] + AlignmentA;
                AlignmentB = "-" + AlignmentB;
                n = n - 1;
            }
            else //if (m > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n] + -2)
            {
                AlignmentA = "-" + AlignmentA;
                AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                m = m - 1;
            }
        }
        AliA = AlignmentA.ToString();
        AliB = AlignmentB.ToString();
        */
