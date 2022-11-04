using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 

public class LocomotionController : MonoBehaviour
{

    [Header("Controllers")]
    public XRController leftTeleportRay;
    [Header("Buttons")]
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftTeleportRay)
        {
            leftTeleportRay.gameObject.SetActive(checkIfActivated(leftTeleportRay));
        }
    }

    public bool checkIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
