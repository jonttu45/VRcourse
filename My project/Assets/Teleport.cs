using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour
{
    public Transform startposition;
    public Transform externalviewposition;
    public InputActionReference switchposition;

    private bool isatstart = true;
    // Start is called before the first frame update
    void Start()
    {
        switchposition.action.Enable();
        switchposition.action.performed += context => SwitchPosition(context);
        
    }

    private void SwitchPosition(InputAction.CallbackContext context)
    {
        if(isatstart)
        {
            transform.position = externalviewposition.position;
            transform.rotation = externalviewposition.rotation;
        }
        else
        {
            transform.position = startposition.position;
            transform.rotation = startposition.rotation;
        }
        isatstart = !isatstart;
    }
}
