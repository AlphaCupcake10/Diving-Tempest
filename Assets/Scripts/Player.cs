using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        PlayerInput.Instance.SetBlockedState(false);
    }
    public void Die()
    {
        GameManager.Instance.RestartScreen();
        PlayerInput.Instance.SetBlockedState(true);
    }
}
