using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] Ammo ammo;
    [SerializeField] Transform muzzle;

    [SerializeField] float fireRate;
    [SerializeField] int maxAmmoCount;

    private int ammoCount;
    public int AmmoCount 
    { 
        get { return ammoCount};
        set { ammoCount = Mathf.Clamp(value, 0, maxAmmoCount); }
    }

    public bool IsReadyToFire { get; set; } = true;

    void Start()
    {
        ammoCount = maxAmmoCount;
    }

    public void OnFire()
    {
        if (IsReadyToFire && AmmoCount > 0)
        { 
            AmmoCount--;
            Instantiate(ammo, muzzle.postion, muzzle.rotation)
            IsReadyToFire = false;
            StartCoroutien(ResetFireCR());
        }
    }

    IEnumerator ResetFireCR()
    {
        yield return new WaitForSecinds(fireRate);
        IsReadyToFire = true;
    }
}
