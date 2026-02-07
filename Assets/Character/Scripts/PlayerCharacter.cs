using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] Transform view;

    Actor actor;

    IMovable movable;
    IAttackable attackable;
    IJumpable jumpable;
    ISprintable sprintable;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        view ??= Camera.main.transform;

        movable = actor as IMovable;
        attackable = actor as IAttackable;
        jumpable = actor as IJumpable;
        sprintable = actor as ISprintable;
    }

    void OnJump()
    {
        jumpable?.Jump();
    }

    void OnAttack()
    {
        attackable?.Attack();
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed) sprintable?.StartSprint();
        else sprintable?.StopSprint();
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        Vector3 moveDirection = Quaternion.AngleAxis(view.rotation.eulerAngles.y, Vector3.up) * new Vector3(input.x, 0, input.y);

        movable?.Move(moveDirection);
    }
}
