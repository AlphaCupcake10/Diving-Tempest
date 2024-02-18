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
    public bool restartKey = false;
    public bool isInputBlocked = false;
    public bool pauseKey = false;

    public static PlayerInput Instance;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        restartKey = Input.GetKeyDown(KeyCode.R);
        if(isInputBlocked)
        {
            MovementAxis = Vector2.zero;
            Jump = false;
            Crouch = false;
            grabKey = false;
            throwKey = false;
            return;
        }
        Jump = Input.GetButtonDown("Jump");
        Crouch = Input.GetButton("Crouch");
        MovementAxis = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        pauseKey = Input.GetKeyDown(KeyCode.Escape);
        grabKey = Input.GetKeyDown(KeyCode.Mouse1);
        throwKey = Input.GetKey(KeyCode.Mouse0);
    }
    public void SetBlockedState(bool val)
    {
        isInputBlocked = val;
    }
}
