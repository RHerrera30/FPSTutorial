using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    
    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHealthUI.text = $"Health: {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            Debug.Log("Player Dead");
            PlayerDead();
            isDead = true; 
        }
        else
        {
            Debug.Log("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        // GetComponent<MouseMovement>().enabled = false;
        // GetComponent<PlayerMovement>().enabled = false;
        
        //Dying Animation
        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);
        
        GetComponent<ScreenBlackout>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        if (bloodyScreen.activeInHierarchy)
        {
            //bloodyScreen.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie Hand"))
        {
            if (!isDead)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}
