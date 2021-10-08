using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public List<float> angles_Shape = new List<float>();
    public List<float> angles_Variaton_Shape = new List<float>();

    public void Fill(float[] angles, float[] angles_Variaton)
    {
        foreach (float angle in angles)
        {
            angles_Shape.Add(angle);
        }
        foreach (float angle_Variaton in angles_Variaton)
        {
            angles_Variaton_Shape.Add(angle_Variaton);
        }
    }
}
