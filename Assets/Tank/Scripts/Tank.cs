using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float rotationSpeed = 90.0f; // rotation in degrees per second

    [SerializeField] GameObject ammo;
    [SerializeField] GameObject muzzle;

    [SerializeField] Slider Hp;

    InputAction moveAction;
    InputAction attackAction;

    Health health;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");

        attackAction.started += ctx => OnAttack();

        health = GetComponednt<Health>();
    }

    void Update()
    {
        // direction (forward/backward movement)
        float direction = moveAction.ReadValue<Vector2>().y;

        // translate (move) the tank in the forward direction
        // moves the tank in the relative direction (direction tank is facing)
        transform.Translate(Vector3.forward * direction * speed * Time.deltaTime);

        // rotation (left/right)
        float rotation = moveAction.ReadValue<Vector2>().x;

        // rotate the tank, around the up axis (y-axis)
        transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);

        Hp.value = health.CurrentHpPrcent;
    }

    void OnAttack()
    {
        Instantiate(ammo, muzzle.transform.position, muzzle.transform.rotation);
    }
}
