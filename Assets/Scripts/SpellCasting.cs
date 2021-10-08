using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public PlayerMovement playerMove;

    void Start()
    {
        
    }

    void Update()
    {
        if (DetectShape.correctShape != 0.0)
        {
            switch (DetectShape.correctShape)
            {
                case 2.1f:
                    playerMove.Leap();
                    break;
                case 2.2f:
                    playerMove.Leap();
                    break;
                case 3.1f:
                    break;
                case 3.2f:
                    break;
                case 4.1f:
                    break;
            }
            DetectShape.correctShape = 0.0f;
        }
    }
}
