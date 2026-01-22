using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] float rotaionSpeed = 45.0f;
    [SerializeField] float fireRate = 1.0f;

    [SerializeField] Ammo ammo;
    [SerializeField] Transform muzzle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0 )
        {
            fireTimer += fireRate;
            Instantiate(ammo, muzzle.position, muzzle.rotation);
        }

        transform.Roatate(Vector3.up * rotaionSpeed * Time.deltaTime);
    }
}
