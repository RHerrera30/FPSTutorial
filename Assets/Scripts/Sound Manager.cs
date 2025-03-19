using UnityEngine;
using UnityEngine.Serialization;
using static Weapon;

public class SoundManager : MonoBehaviour
{ 
    public static SoundManager Instance { get; set; }

    [FormerlySerializedAs("shootingSoundM1911")] public AudioSource ShootingChannel;
    public AudioSource emptyMagazineSoundM1911;
    
    //M1911
    public AudioClip m1911Shot;
    public AudioSource reloadingSoundM1911;
    
    //M48
    public AudioClip m48Shot;
    public AudioSource reloadingSoundM48;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(m1911Shot);
                break;
            case WeaponModel.M48:
                ShootingChannel.PlayOneShot(m48Shot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadingSoundM1911.Play();
                break;
            case WeaponModel.M48:
                reloadingSoundM48.Play();
                break;
        }
    }
}
