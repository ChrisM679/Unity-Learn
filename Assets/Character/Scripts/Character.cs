using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : Actor, IMovable, IJumpable, IAttackable, ISprintable
{
    [SerializeField] Animator animator;

    [SerializeField] float force = 5;
    [SerializeField] float speed = 2;
    [SerializeField] float sprintSpeed = 5;
    [SerializeField] float dragSpeed = 20;
    [SerializeField] float airDragSpeed = 20;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 2.0f;
    [SerializeField] float turnRate = 5.0f;

    CharacterController controller;

    Vector3 velocity = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    bool isSprinting = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // check if the player is on ground
        bool onGround = controller.isGrounded;

        // reset vertical velocity when grounded to prevent accumulating downward force
        if (onGround && velocity.y < 0)
        {
            velocity.y = -0.2f; // small downward force to keep player grounded
        }

        // initialize acceleration vector for movement calculations
        Vector3 acceleration = Vector3.zero;
        acceleration.x = moveDirection.x * force;
        acceleration.z = moveDirection.z * force;

        // reduce acceleration while in the air for smoother movement control
        if (!onGround) acceleration *= 0.5f;

        // extract horizontal velocity (ignoring vertical movement)
        Vector3 vXZ = new Vector3(velocity.x, 0, velocity.z);

        // apply acceleration to velocity while limiting max speed
        vXZ += acceleration * Time.deltaTime;
        vXZ = Vector3.ClampMagnitude(vXZ, (isSprinting) ? sprintSpeed : speed);

        // assign updated velocity values
        if (moveDirection.sqrMagnitude <= 0 || !onGround)
        {
            float drag = (onGround) ? dragSpeed : airDragSpeed;
            vXZ.x = Mathf.MoveTowards(vXZ.x, 0, drag * Time.deltaTime);
            vXZ.z = Mathf.MoveTowards(vXZ.z, 0, drag * Time.deltaTime);
        }

        velocity.x = vXZ.x;
        velocity.z = vXZ.z;

        // smoothly rotate the player towards the movement direction
        if (moveDirection.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * turnRate);
        }

        // apply gravity
        velocity.y += gravity * Time.deltaTime;

        // move the character using the CharacterController component
        controller.Move(velocity * Time.deltaTime);

        // animator
        animator?.SetFloat("Speed", vXZ.magnitude);

        //animator?.SetFloat("AirSpeed", controller.velocity.y);
        //animator?.SetBool("OnGround", onGround);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(-2.0f * gravity * jumpHeight);
            animator?.SetTrigger("Jump");
        }
    }

    public void Attack()
    {
        animator?.SetTrigger("Attack");
    }

    public void Move(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void StartSprint()
    {
        isSprinting = true;
    }

    public void StopSprint()
    {
        isSprinting = false;
    }
}