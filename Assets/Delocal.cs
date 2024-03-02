using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delocal : MonoBehaviour
{
    Delocal Instance;

    public GameObject TouchTutorial;
    public GameObject KeyboardTutorial;

    public GameObject TouchTutorialButtons;
    public GameObject TouchTutorialJoysticks;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        if (PlayerInput.Instance.inputType == PlayerInput.InputType.Keyboard)
        {
            TouchTutorial.SetActive(false);
            KeyboardTutorial.SetActive(true);
        }
        else
        {
            TouchTutorial.SetActive(true);
            KeyboardTutorial.SetActive(false);
            
            if(PlayerInput.Instance.touchScheme == PlayerInput.TouchScheme.Buttons)
            {
                TouchTutorialButtons.SetActive(true);
                TouchTutorialJoysticks.SetActive(false);
            }
            else
            {
                TouchTutorialButtons.SetActive(false);
                TouchTutorialJoysticks.SetActive(true);
            }
        }
    }

}
