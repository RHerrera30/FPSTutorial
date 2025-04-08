using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;
    
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            Debug.Log("Player Dead");
        }
        else
        {
            Debug.Log("Player Hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie Hand"))
        {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
