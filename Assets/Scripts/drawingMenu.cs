using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawingMenu : MonoBehaviour
{
    public static bool isDrawing = false;
    public GameObject drawingMenuUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            isDrawing = true;
            drawingMenuUI.SetActive(true);
            Time.timeScale = 0.5f;
        }
        else
        {
            isDrawing = false;
            drawingMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
