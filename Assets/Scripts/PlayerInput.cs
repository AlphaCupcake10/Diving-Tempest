using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class PlayerInput : MonoBehaviour
{
    public bool Jump = false;
    public bool Crouch = false;
    public Vector2 MovementAxis;
    public Vector2 AimAxis;
    public bool grabKey = false;
    public bool throwKey = false;
    public bool restartKey = false;
    public bool isInputBlocked = false;
    public bool pauseKey = false;

    public bool interactKey = false;

    public bool[] debugKeys = new bool[10];

    private static PlayerInput _instance;

    public enum InputType
    {
        Touch,
        Keyboard,
        Controller
    }

    public enum TouchScheme
    {
        Buttons,
        JoysticksOnly
    }

    InputType p_inputType;
    public InputType inputType;
    public TouchScheme touchScheme;
    


    public GameObject touchControls;
    public FloatingJoystick joystickLeft;
    public FloatingJoystick joystickRight;
    public TouchButton jumpButton;
    public TouchButton crouchButton;

    public TouchButton interactButton;

    public static PlayerInput Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerInput>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerInput");
                    _instance = singletonObject.AddComponent<PlayerInput>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (joystickRight != null)
        {
            joystickRight.OnJoystickDown += RightStickDown;
            joystickRight.OnJoystickUp += RightStickUp;
        }
        if(interactButton != null)
        {
            interactButton.OnJoystickDown += InteractKeyDown;
        }
        interactButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Make sure to unsubscribe from the event when the script is destroyed to avoid memory leaks.
        if (joystickRight != null)
        {
            joystickRight.OnJoystickDown -= RightStickDown;
            joystickRight.OnJoystickUp -= RightStickUp;
        }
        if(interactButton != null)
        {
            interactButton.OnJoystickDown -= InteractKeyDown;
        }
    }

    public void ShowInteractButton()
    {
        interactButton.gameObject.SetActive(true);
    }
    public void HideInteractButton()
    {
        interactButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        debugKeys[0] = Input.GetKeyDown(KeyCode.F1);
        debugKeys[1] = Input.GetKeyDown(KeyCode.F2);
        debugKeys[2] = Input.GetKeyDown(KeyCode.F3);
        debugKeys[3] = Input.GetKeyDown(KeyCode.F4);
        debugKeys[4] = Input.GetKeyDown(KeyCode.F5);
        debugKeys[5] = Input.GetKeyDown(KeyCode.F6);
        debugKeys[6] = Input.GetKeyDown(KeyCode.F7);
        debugKeys[7] = Input.GetKeyDown(KeyCode.F8);
        debugKeys[8] = Input.GetKeyDown(KeyCode.F9);
        debugKeys[9] = Input.GetKeyDown(KeyCode.F10);

        if(Input.touchCount > 0)
        {
            inputType = InputType.Touch;
        }

        if (inputType == InputType.Keyboard)
        {
            interactKey = Input.GetKeyDown(KeyCode.E);
            restartKey = Input.GetKeyDown(KeyCode.R);
        }
        touchControls.SetActive(inputType == InputType.Touch && SceneManager.GetActiveScene().buildIndex != 0);
        if (isInputBlocked)
        {
            MovementAxis = Vector2.zero;
            AimAxis = Vector2.zero;
            Jump = false;
            Crouch = false;
            grabKey = false;
            throwKey = false;
            return;
        }
        if (inputType == InputType.Keyboard)
        {
            Jump = Input.GetButtonDown("Jump");
            Crouch = Input.GetButton("Crouch");
            MovementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            pauseKey = Input.GetKeyDown(KeyCode.Escape);
            grabKey = Input.GetKey(KeyCode.Mouse1);
            throwKey = Input.GetKeyDown(KeyCode.Mouse0);
        }
        else if(inputType == InputType.Touch)
        {
            if(joystickLeft.Horizontal > 0.5f)
            {
                MovementAxis = new Vector2(1, 0);
            }
            else if(joystickLeft.Horizontal < -0.5f)
            {
                MovementAxis = new Vector2(-1, 0);
            }
            else
            {
                MovementAxis = new Vector2(0, 0);
            }
            
            if(touchScheme == TouchScheme.Buttons)
            {
                if(!jumpButton.gameObject.activeSelf)
                {
                    jumpButton.gameObject.SetActive(true);
                }
                if(!crouchButton.gameObject.activeSelf)
                {
                    crouchButton.gameObject.SetActive(true);
                }
                Jump = jumpButton.isTouching;
                if(crouchButton.isTouching)
                {
                    Crouch = true;
                    CancelInvoke("ResetCrouch");
                    Invoke("ResetCrouch", 0.1f);
                }
            }
            else if(touchScheme == TouchScheme.JoysticksOnly)
            {
                if(jumpButton.gameObject.activeSelf)
                {
                    jumpButton.gameObject.SetActive(false);
                }
                if(crouchButton.gameObject.activeSelf)
                {
                    crouchButton.gameObject.SetActive(false);
                }
                Jump = joystickLeft.Vertical > 0.5f;
                if(joystickLeft.Vertical < -0.5f)
                {
                    Crouch = true;
                    CancelInvoke("ResetCrouch");
                    Invoke("ResetCrouch", 0.1f);
                }
            }

            if(joystickRight.Direction.magnitude > 0.1f)
                AimAxis = joystickRight.Direction;
        }
        else if(inputType == InputType.Controller)
        {
            MovementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
        }
    }

    public void PauseKeyDown()
    {
        PauseMenu.Instance?.TogglePauseMenu();
    }
    
    public void RightStickDown()
    {
        grabKey = true;
    }
    public void RightStickUp()
    {
        grabKey = false;
        StartCoroutine(ThrowKeyNextFrame());
    }

    public void ResetCrouch()
    {
        Crouch = false;
    }

    void InteractKeyDown()
    {
        StartCoroutine(ResetInteractKey());
    }

    IEnumerator ThrowKeyNextFrame()
    {
        yield return new WaitForEndOfFrame();
        throwKey = true;
        yield return new WaitForEndOfFrame();
        throwKey = false;
    }

    IEnumerator ResetInteractKey()
    {
        yield return new WaitForEndOfFrame();
        interactKey = true;
        yield return new WaitForEndOfFrame();
        interactKey = false;
    }
    public void SetBlockedState(bool val)
    {
        isInputBlocked = val;
    }
}
