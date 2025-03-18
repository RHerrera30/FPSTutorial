using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
