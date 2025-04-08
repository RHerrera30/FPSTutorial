using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Hit " + collision.gameObject.name + " !");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit a wall!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Beer"))
        {
            Debug.Log("Hit a bottle!");
            collision.gameObject.GetComponent<BeerBottle>().Shatter();
            //Don't destroy bullet in case shooting multiple bottles
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit a zombie!");
            collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        //Where I hit the object
        ContactPoint contact = objectHit.contacts[0];

        //Creating my bullet hole
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal));
        
        //SHOW HOLE
        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}
