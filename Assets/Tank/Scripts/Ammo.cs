using UnityEngine;

public abstract class Ammo : MonoBehaviour
{
    [SerializeField, Range(1,10)] protected float dmg = 1;
}
