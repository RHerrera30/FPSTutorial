using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [FormerlySerializedAs("damageRadis")] [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 200f;

    private float countDown;
    private bool hasExploded = false;
    public bool hasBeenThrown = false;
    
    public enum ThrowableType
    {
        Grenade
    }
    
    public ThrowableType throwableType;

    private void Start()
    {
        countDown = delay;
        
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0f && !hasExploded) 
            {
                Explode();
                hasExploded = true; 
            }
        }
        
    }

    private void Explode()
    {
        GetThrowableEffect();
        
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        //Visual effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        //Physical effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            Debug.Log(objectInRange);
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius, 0f, ForceMode.Impulse);
            }
        }
    }
}
