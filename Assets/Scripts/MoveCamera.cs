
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition = null;
    [SerializeField] Transform drawingCameraPosition = null;

    void Update()
    {
        transform.position = cameraPosition.position;
        transform.position = drawingCameraPosition.position;
    }
}