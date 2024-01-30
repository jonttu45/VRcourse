using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light light;
    public InputActionReference action;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            light.color = new Color(0.3f, 0.4f, 0.6f, 0.3f);
        };

    }

}
