using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;
    public GameObject zombiePrefab;

    public TextMeshProUGUI titleWaveOver;
    public TextMeshProUGUI coolDownCounterTitle;
    public TextMeshProUGUI currentWaveUI;

    public float spawnDelay = 0.5f; //Delay between each zombie spawn
    
    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter;
    
    public List<Enemy> currentZombiesAlive;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;
        currentWaveUI.text = "Wave: " + currentWave;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            //Gen a random offset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(1f,1f), 0f, Random.Range(1f,1f));
            Vector3 spawnPosition = transform.position + spawnOffset;
            
            //Instantiate zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            
            //Get enemy script
            Enemy enemyScript = zombie.GetComponent<Enemy>();
            
            //Track this zombie
            currentZombiesAlive.Add(enemyScript);
            
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        //Get all dead zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();

        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        
        //Remove all dead zombies
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        
        zombiesToRemove.Clear();
        
        //Start cooldown if all zombies dead
        if (currentZombiesAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCooldown());
        }
        
        //Run the cooldown counter
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            //Reset the counter
            cooldownCounter = waveCooldown;
        }
        
        coolDownCounterTitle.text = cooldownCounter.ToString("F0");

    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        titleWaveOver.gameObject.SetActive(true);
        coolDownCounterTitle.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(waveCooldown);
        inCooldown = false;
        titleWaveOver.gameObject.SetActive(false);
        coolDownCounterTitle.gameObject.SetActive(false);
        
        currentZombiesPerWave *= 2;
        StartNextWave();
    }
}
