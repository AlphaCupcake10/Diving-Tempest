using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerInput : MonoBehaviour
{
    public bool Jump;
    public bool Crouch;
    public Vector2 MovementAxis;
    public bool grabKey;
    public bool throwKey;

    void Update()
    {
        Jump = Input.GetButtonDown("Jump");
        Crouch = Input.GetButton("Crouch");
        MovementAxis = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        grabKey = Input.GetKeyDown(KeyCode.E);
        throwKey = Input.GetKey(KeyCode.Mouse0);
    }
}
