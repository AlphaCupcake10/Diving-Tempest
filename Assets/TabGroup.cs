using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{

    void Start()
    {
        SetTab(0);
    }

    public void SetTab(int selectedTab)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == selectedTab);
        }
    }
}
