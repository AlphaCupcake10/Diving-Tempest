using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

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
