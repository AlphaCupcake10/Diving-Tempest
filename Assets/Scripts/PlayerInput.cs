using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerInput : MonoBehaviour
{
    public bool Jump = false;
    public bool Crouch = false;
    public Vector2 MovementAxis;
    public bool grabKey = false;
    public bool throwKey = false;
    public bool AimKeyUp = false;
    public bool AimKeyDown = false;

    public bool isInputBlocked = false;

    void Update()
    {
        if(isInputBlocked)
        {
            MovementAxis = Vector2.zero;
            Jump = false;
            Crouch = false;
            grabKey = false;
            throwKey = false;
            AimKeyDown = false;
            AimKeyUp = false;
            return;
        }
        Jump = Input.GetButtonDown("Jump");
        Crouch = Input.GetButton("Crouch");
        MovementAxis = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        grabKey = Input.GetKeyDown(KeyCode.E);
        throwKey = Input.GetKey(KeyCode.Mouse0);

        AimKeyDown = Input.GetKeyDown(KeyCode.Mouse1);
        AimKeyUp = Input.GetKeyUp(KeyCode.Mouse1);
    }
    public void SetBlockedState(bool val)
    {
        isInputBlocked = val;
    }
}
