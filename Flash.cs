using UnityEngine;
using UnityEngine.InputSystem;

public class Flash : MonoBehaviour
{
    public float teleportDistance = 15f;
    private CharacterController characterController;

    public float flashCooldown = 5f; 
    private float lastFlashTime = -Mathf.Infinity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            if (Time.time - lastFlashTime >= flashCooldown)
            {
                TeleportForward();
                lastFlashTime = Time.time;
            }
            else
            {
                Debug.Log("Flash je na cooldownu!");
            }
        }
    }

    void TeleportForward()
    {
        Vector3 teleportPosition = transform.position + transform.forward * teleportDistance;
        characterController.enabled = false; 
        transform.position = teleportPosition;
        characterController.enabled = true;
        Debug.Log("Flash použit!");
    }
}
