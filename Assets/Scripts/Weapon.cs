using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    
    //Burst
    public int bulletsPerBurst = 3;
    [FormerlySerializedAs("currentBurst")] public int burstBulletsLeft;
    
    //Spread
    public float spreadIntensity;
    
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;
    
    //Bullet - Muzzle
    public GameObject muzzleEffect;
    
    //Loading
    public float reloadTime;
    public int magazineSize;
    public int bulletsLeft;
    public bool isReloading;
    
    //UI
    public TextMeshProUGUI ammoDisplay;
    
    //Animation
    public Animator animator;

    public enum WeaponModel
    {
        Pistol1911,
        M48
    }

    public WeaponModel thisWeaponModel;
    
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        
        bulletsLeft = magazineSize;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyMagazineSoundM1911.Play();
        }
        
        if (currentShootingMode == ShootingMode.Auto)
        {
            //Holding down left mouse button
            isShooting = Input.GetMouseButton(0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetMouseButtonDown(0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
        {
            Reload();
        }

        //Auto-reload empty magazine
        if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
        {
            //Reload();
        }
        
        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        if (AmmoManager.Instance.ammoDisplay)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
        }
}

    // ReSharper disable Unity.PerformanceAnalysis
    private void FireWeapon()
    {
        bulletsLeft--;
        
        //Muzzle flash, recoil animation, gunshot sound
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Recoil");
        // SoundManager.Instance.shootingSoundM1911.Play();
        
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        
        Debug.Log("Firing");
        
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        
        
        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        //Point the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
        
        //SHOOTIN'!
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        
        //Destroy the bullet
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        
        //Reset the shot
        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }
        
        //Burst Mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }
    
    private void Reload()
    {
        // SoundManager.Instance.reloadingSoundM1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        
        animator.SetTrigger("Reload");
        
        isReloading = true;
        Invoke(nameof(ReloadCompleted),reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from the middle of the screen to check where pointing
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            //Hit something
            targetPoint = hit.point;
        }
        else
        {
            //Hit nothing
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        
        float spreadX = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        
        //Returns the shooting direction and spread
        return direction + new Vector3(spreadX, spreadY, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
