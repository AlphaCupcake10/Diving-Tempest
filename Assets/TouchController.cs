using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public bool Jump { get; private set; }
    public bool Crouch { get; private set; }
    public Vector2 MovementAxis { get; private set; }

    void Start()
    {
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        // Reset input values
        Jump = false;
        Crouch = false;
        MovementAxis = Vector2.zero;

        // Process touch inputs
        foreach (Touch touch in Input.touches)
        {
            // Check if the touch is on the left half of the screen
            if (touch.position.x < Screen.width / 2)
            {
                // Movement control
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    MovementAxis = new Vector2(1f, 0f); // Move right
                }
            }
            // Check if the touch is on the right half of the screen
            else
            {
                // Jump or crouch control
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.y < Screen.height / 2)
                    {
                        // Crouch
                        Crouch = true;
                    }
                    else
                    {
                        // Jump
                        Jump = true;
                    }
                }
            }
        }
    }
}
