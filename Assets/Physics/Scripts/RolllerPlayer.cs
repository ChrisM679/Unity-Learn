using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RolllerPlayer : MonoBehaviour
{
    [SerializeField, Range(0,50)] float moveForce = 3;
    [SerializeField, Range(0,50)] float jumpForce = 3;
    [SerializeField] Transform view;

    [Header("Ground Collision")]
    [SerializeField, Range(0, 5)] float rayLength = 1;
    [SerializeField] LayerMask groundLayerMask = Physics.AllLayers;

    Rigidbody rb;
    Vector2 inputMovement;

    InputAction moveAction;
    InputAction jumpAction;

    void Awake()
    {
        view ??= Camera.main.transform;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        jumpAction.started += OnJump;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        jumpAction.started -= OnJump;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.yellow);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(inputMovement.x, 0, inputMovement.y);
        movement = Quaternion.AngleAxis(view.rotation.eulerAngles.y, Vector3.up) * movement;
        rb.AddForce(movement * moveForce, ForceMode.Force);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        inputMovement = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if ( OnGround())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    bool OnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayerMask);
    }
}
