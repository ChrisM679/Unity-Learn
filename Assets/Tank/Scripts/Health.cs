using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeFeild] float maxHp = 10;
    [SerializeFeild] GameObject hitEffect;
    [SerializeFeild] GameObject destroyEffect;

    public float HpCurrent 
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, maxHp); } 
    }

    public float CurrentHpPrecent
    {

    }

    float health = 0;
    bool destroyed = false;

    void Start()
    {
        HpCurrent = maxHp;
    }

    public void OnDmg(float dmg)
    {
        if (destroyed) return;

        HpCurrent -= dmg;

        if (HpCurrent <= 0 ) destroyed = true;

        if (!destroyed && hitEffect != null)
        { 
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (destroyed)
        {
            TankGameManager.Instance.Score += 100;

            if (destroyed != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void OnHeal(float amount)
    {
        HpCurrent += amount;
    }
}
