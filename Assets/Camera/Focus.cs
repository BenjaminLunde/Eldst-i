using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{
    public static Focus instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Focus!");
            return;
        }
        instance = this;
    }

    public GameObject focus = null;

    public void SetFocus()
    {

    }

    public void RemoveFocus()
    {

    }
}
