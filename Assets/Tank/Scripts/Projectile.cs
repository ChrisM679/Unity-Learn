using UnityEngine;
using UnityEngine.InputSystem;

public class Rocket : Ammo
{
    [SerializeField] GameObject effect;
    [SerializeField] float speed = 1.0f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    void Uodate()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null) 
        { 
            health.OnDamage(dmg);
        }
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
