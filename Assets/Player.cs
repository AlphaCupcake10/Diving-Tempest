using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Die()
    {
        GameManager.Instance.RestartScreen();
        GetComponent<PlayerInput>()?.SetBlockedState(true);
    }
}
