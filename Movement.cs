using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f;
    private float gravity = 9.81f;
    private CharacterController controller;
    private Vector3 moveDirection;
    private Transform cameraTransform;
    private Vector3 cameraOffset = new Vector3(0, 20, -20);
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Mathf.Abs(moveX) < 0.2f) moveX = 0;
        if (Mathf.Abs(moveZ) < 0.2f) moveZ = 0;

        Vector3 move = new Vector3(moveX, 0, moveZ);

        if (move.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }

        moveDirection = new Vector3(move.x, moveDirection.y, move.z);
        moveDirection.y -= gravity * Time.deltaTime;

        
        Vector3 proposedMove = moveDirection * speed * Time.deltaTime;
        Vector3 proposedPosition = transform.position + proposedMove;

        
        Vector3 center = Vector3.zero;
        float radius = 140f;

        Vector3 flatProposed = new Vector3(proposedPosition.x, 0, proposedPosition.z);
        Vector3 flatCenter = new Vector3(center.x, 0, center.z);
        Vector3 toCenter = flatProposed - flatCenter;

        if (toCenter.magnitude > radius)
        {
            
            Vector3 flatCurrent = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 currentToCenter = flatCurrent - flatCenter;

            Vector3 allowedDirection = (flatProposed - flatCurrent).normalized;
            float maxMove = radius - currentToCenter.magnitude;

            
            proposedMove = allowedDirection * Mathf.Max(0, maxMove);
            proposedMove.y = moveDirection.y * speed * Time.deltaTime; 
        }

        controller.Move(proposedMove);

        
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }

        bool isWalking = move.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }

}