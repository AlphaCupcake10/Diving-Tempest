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
    }
    private void OnDestroy()
    {
        // Make sure to unsubscribe from the event when the script is destroyed to avoid memory leaks.
        if (joystickRight != null)
        {
            joystickRight.OnJoystickDown -= RightStickDown;
            joystickRight.OnJoystickUp -= RightStickUp;
        }
    }

    private void Update()
    {
        restartKey = Input.GetKeyDown(KeyCode.R);
        if(Input.touchCount > 0)
        {
            inputType = InputType.Touch;
        }
        if(p_inputType != inputType)
        {
            if(inputType == InputType.Touch)
            {
                ShadowCaster2D[] shadows = FindObjectsOfType<ShadowCaster2D>();
                foreach(ShadowCaster2D shadow in shadows)
                {
                    shadow.enabled = false;
                }
            }
            if(inputType == InputType.Keyboard)
            {
                ShadowCaster2D[] shadows = FindObjectsOfType<ShadowCaster2D>();
                foreach(ShadowCaster2D shadow in shadows)
                {
                    shadow.enabled = true;
                }
            }
            p_inputType = inputType;
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
            grabKey = Input.GetKeyDown(KeyCode.Mouse1);
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

    void ResetCrouch()
    {
        Crouch = false;
    }
    public void JumpButtonDown()
    {
        Jump = true;
    }
    
    public void RightStickDown()
    {
        StartCoroutine(GrabKeyNextFrame());
    }
    public void RightStickUp()
    {
        StartCoroutine(ThrowKeyNextFrame());
    }
    IEnumerator ThrowKeyNextFrame()
    {
        yield return new WaitForEndOfFrame();
        throwKey = true;
        yield return new WaitForEndOfFrame();
        throwKey = false;
    }
    IEnumerator GrabKeyNextFrame()
    {
        yield return new WaitForEndOfFrame();
        grabKey = true;
        yield return new WaitForEndOfFrame();
        grabKey = false;
    }

    public void SetBlockedState(bool val)
    {
        isInputBlocked = val;
    }
}
