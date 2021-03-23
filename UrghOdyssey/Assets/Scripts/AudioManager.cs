using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioSource oneShotter;

    //////////
    /// 
    /// OneShots
    /// 
    //////////
    [Space(10)][Header("OneShots")]
    //gearExplosion 
    public AudioClip gearExplosion;
    [Range(0f,1f)] public float gearExplosionVolume = 1f;
    //Boss1Screech 
    public AudioClip boss1Screech;
    [Range(0f,1f)] public float boss1ScreechVolume = 1f;
    //Boss1Screech2 
    public AudioClip boss1Screech2;
    [Range(0f,1f)] public float boss1Screech2Volume = 1f;
    //shieldOn 
    public AudioClip shieldOn;
    [Range(0f,1f)] public float shieldOnVolume = 1f;
    //shieldOff 
    public AudioClip shieldOff;
    [Range(0f,1f)] public float shieldOffVolume = 1f;
    //lightningAttackCharge 
    public AudioClip lightningAttackCharge;
    [Range(0f,1f)] public float lightningAttackChargeVolume = 1f;
    //lightningAttackUnCharge 
    public AudioClip lightningAttackUnCharge;
    [Range(0f,1f)] public float lightningAttackUnChargeVolume = 1f;
    //playerDeath1 
    public AudioClip playerDeath1;
    [Range(0f,1f)] public float playerDeath1Volume = 1f;
    //playerDeath2 
    public AudioClip playerDeath2;
    [Range(0f,1f)] public float playerDeath2Volume = 1f;
    //gun2 
    public AudioClip gun2;
    [Range(0f,1f)] public float gun2Volume = 1f;
    //gun3 
    public AudioClip gun3;
    [Range(0f,1f)] public float gun3Volume = 1f;
    //playerGunImpact
    public AudioClip playerGunImpact;
    [Range(0f,1f)] public float playerGunImpactVolume = 1f;
    //intro music
    public AudioClip musicIntro;
    [Range(0f, 1f)] public float musicIntroVolume = 1f;
    //bossFinalDeath
    public AudioClip bossFinalDeath;
    [Range(0f, 1f)] public float bossFinalDeathVolume = 1f;

    //////////
    /// 
    /// Loops
    /// 
    //////////
    [Space(10)][Header("Loops")]
    //gun1 
    public AudioClip gun1;
    [Range(0f,1f)] public float gun1Volume = 1f;
    AudioSource gun1Src;
    //lightningAttack 
    public AudioClip lightningAttack;
    [Range(0f,1f)] public float lightningAttackVolume = 1f;
    AudioSource lightningAttackSrc;
    //shield 
    public AudioClip shield;
    [Range(0f,1f)] public float shieldVolume = 1f;
    AudioSource shieldSrc;
    //musicMainLoop
    public AudioClip musicMainLoop;
    [Range(0f,1f)] public float musicMainLoopVolume = 1f;
    AudioSource musicMainLoopSrc;
    //bossDeathLoop
    public AudioClip bossDeathLoop;
    [Range(0f,1f)] public float bossDeathLoopVolume = 1f;
    AudioSource bossDeathLoopSrc;


    void Awake()
    {
        oneShotter = gameObject.AddComponent<AudioSource>();

        //shield 
        shieldSrc = gameObject.AddComponent<AudioSource>();
        shieldSrc.playOnAwake = false;
        shieldSrc.volume = shieldVolume;
        shieldSrc.clip = shield;
        shieldSrc.loop = true;

        //gun1 
        gun1Src = gameObject.AddComponent<AudioSource>();
        gun1Src.playOnAwake = false;
        gun1Src.volume = gun1Volume;
        gun1Src.clip = gun1;
        gun1Src.loop = true;
        
        //lightningAttack 
        lightningAttackSrc = gameObject.AddComponent<AudioSource>();
        lightningAttackSrc.playOnAwake = false;
        lightningAttackSrc.volume = lightningAttackVolume;
        lightningAttackSrc.clip = lightningAttack;
        lightningAttackSrc.loop = true;
        
        //musicMainLoop 
        musicMainLoopSrc = gameObject.AddComponent<AudioSource>();
        musicMainLoopSrc.playOnAwake = false;
        musicMainLoopSrc.volume = musicMainLoopVolume;
        musicMainLoopSrc.clip = musicMainLoop;
        musicMainLoopSrc.loop = true;

        //bossDeathLoop 
        bossDeathLoopSrc = gameObject.AddComponent<AudioSource>();
        bossDeathLoopSrc.playOnAwake = false;
        bossDeathLoopSrc.volume = bossDeathLoopVolume;
        bossDeathLoopSrc.clip = bossDeathLoop;
        bossDeathLoopSrc.loop = true;
    }
    public void GearExplosion()
    {
        oneShotter.PlayOneShot(gearExplosion, gearExplosionVolume);
    }
    public void BossDeathSounds()
    {
        StartCoroutine(BossDeathSoundsRoutine());
    }
    private IEnumerator BossDeathSoundsRoutine()
    {
        bossDeathLoopSrc.Play();
        yield return new WaitForSeconds(3.5f);
        bossDeathLoopSrc.Stop();
        oneShotter.PlayOneShot(lightningAttackCharge, lightningAttackChargeVolume);
        yield return new WaitForSeconds(lightningAttackCharge.length);
        oneShotter.PlayOneShot(bossFinalDeath, bossFinalDeathVolume);
    }
    public void Boss1Screech()
    {
        oneShotter.PlayOneShot(boss1Screech, boss1ScreechVolume);
    }
    public void Boss1Screech2()
    {
        oneShotter.PlayOneShot(boss1Screech2, boss1Screech2Volume);
    }
    public void LightningAttackUnCharge()
    {
        oneShotter.PlayOneShot(lightningAttackUnCharge, lightningAttackUnChargeVolume);
    }
    public void PlayerDeath1()
    {
        oneShotter.PlayOneShot(playerDeath1, playerDeath1Volume);
    }
    public void PlayerDeath2()
    {
        oneShotter.PlayOneShot(playerDeath2, playerDeath2Volume);
    }
    public void Gun2()
    {
        oneShotter.PlayOneShot(gun2, gun2Volume);
    }
    public void Gun3()
    {
        oneShotter.PlayOneShot(gun3, gun3Volume);
    }
    public void PlayerGunImpact()
    {
        oneShotter.PlayOneShot(playerGunImpact, playerGunImpactVolume);
    }
    public void LightningAttackCharge()
    {
        oneShotter.PlayOneShot(lightningAttackCharge, lightningAttackChargeVolume);
    }
    public void ShieldOn()
    {
        oneShotter.PlayOneShot(shieldOn, shieldOnVolume);
    }
    public void ShieldOff()
    {
        oneShotter.PlayOneShot(shieldOff, shieldOffVolume);
    }
    public void Shielding(bool isshielding)
    {
        if (isshielding && !shieldSrc.isPlaying)
        {
            shieldSrc.Play();
        }
        else if(!isshielding && shieldSrc.isPlaying)
        {
            shieldSrc.Stop();
        }
    }
    public void LightningAttack(bool isZapping)
    {
        if (isZapping && !lightningAttackSrc.isPlaying) lightningAttackSrc.Play();
        else if (!isZapping) lightningAttackSrc.Stop();
    }
    public void Gun1(bool isShooting)
    {
        if (isShooting && !gun1Src.isPlaying) gun1Src.Play();
        else if (!isShooting) gun1Src.Stop();
    }
    public void StartMainMusicSequence()
    {
        StartCoroutine(MainMusicSequence());
        oneShotter.PlayOneShot(musicIntro, musicIntroVolume);
    }
    private IEnumerator MainMusicSequence()
    {
        yield return new WaitForSeconds(musicIntro.length);
        musicMainLoopSrc.Play();
    }
}
