using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss1 : MonoBehaviour
{
    enum BossPhase { Intro, HighReady, Shooting, LowReady, ReturnToHighReady, Dead};
    BossPhase currentPhase;
    public int maxHealth = 300, maxNumberOfShootCycles = 4, scoreValue = 100;
    public float lightningAttackDuration = 3f;
    public ParticleSystem[] lightningAttacksPS;
    public ParticleSystem[] guns;
    public ParticleSystem[] sideLasers;
    public ParticleSystem[] deathPS;
    public GameObject[] enemiesToSpawnAtHalfHealth;
    public BoxCollider2D lightningCollider;
    public Vector3 highReady, lowReady, leftPos, rightPos;
    public Material whiteFlashMaterial;
    SpriteRenderer sr;
    Material originalMaterial;
    PlayerController playerController;
    AudioManager am;
    Animator anim;
    int health, shootCycles;
    bool currentlyZapping;
    float aimTickrateInHZ = 4f;
    Tween currentMove;
    private void OnParticleCollision(GameObject other)
    {
        ReceiveHit();
    }

    private void ReceiveHit()
    {
        if (currentPhase == BossPhase.Dead) return;
        if (health % 10 == 0) Debug.Log("Boss health now at " + health);
        StartCoroutine(DamageFlash());
        health--;
        if (health <= 0) { currentPhase = BossPhase.Dead; Debug.Log("Boss defeated!"); return; }

        if (health == maxHealth / 2 && enemiesToSpawnAtHalfHealth.Length > 0) foreach (GameObject go in enemiesToSpawnAtHalfHealth) go.SetActive(true);
    }

    private IEnumerator DamageFlash()
    {
        if (health % 50 == 0) am.Boss1Screech2();
        if (health % 25 == 0) 
        { 
            sr.material = whiteFlashMaterial;
            yield return new WaitForSeconds(.04f);
            sr.material = originalMaterial;
            yield return new WaitForSeconds(.04f);
            sr.material = whiteFlashMaterial;
            yield return new WaitForSeconds(.04f);
            sr.material = originalMaterial;
            yield return new WaitForSeconds(.04f);
            sr.material = whiteFlashMaterial;
            yield return new WaitForSeconds(.04f);
            sr.material = originalMaterial;
        }
        yield return null;
    }

    private void DeathSequence1()
    {
        StartCoroutine(DamageFlash());
        GlobalStats.EnemiesKilled++;
        currentMove = transform.DOMove(highReady, 2f, false).SetEase(Ease.InOutCubic);
        currentMove.OnComplete(GoToDeathSpot);
        anim.SetBool("Extended", false);
        anim.SetBool("ReadyToAttack", false);
        anim.SetBool("Attacking", false);
        void GoToDeathSpot()
        {
            am.BossDeathSounds();
            StartCoroutine(DamageFlash());
            UIManager ui = FindObjectOfType<UIManager>();
            ui.IncreaseScoreCounter(scoreValue);
            foreach (ParticleSystem ps in lightningAttacksPS) { ps.Stop(); ps.Clear(); }
            foreach (ParticleSystem ps in guns) { ps.Stop(); ps.Clear(); }
            foreach (ParticleSystem ps in sideLasers) { ps.Stop(); ps.Clear(); }
            foreach (ParticleSystem ps in deathPS) { ps.Play(); ps.transform.parent = null; }
            Invoke("DeathSequence2", 4f);
        }
    }
    private void DeathSequence2()
    {
        sr.enabled = false;
        DeathSequence3();
    }
    private void DeathSequence3()
    {
        WaveManager wm = FindObjectOfType<WaveManager>();
        wm.SpawnNextWave();
        Destroy(gameObject);
    }
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
        am = FindObjectOfType<AudioManager>();
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
        StartCoroutine(AimAtPlayer());
        health = maxHealth;
        currentPhase = BossPhase.Intro;
        currentMove = transform.DOMove(highReady, 5f, false).SetEase(Ease.InOutCubic);
        currentMove.OnComplete(GoToHighReady);
        void GoToHighReady()
        {
            currentPhase = BossPhase.HighReady;
            StartCoroutine(BossBehavior());
        }
    }
    private IEnumerator AimAtPlayer()
    {
        while (currentPhase != BossPhase.Dead)
        {
            foreach (ParticleSystem ps in guns)
            {
                ps.transform.LookAt(playerController.transform);
            }
            yield return new WaitForSeconds(1 / aimTickrateInHZ);
        }
    }
    private IEnumerator BossBehavior()
    {
        while(currentPhase != BossPhase.Dead)
        {
            if (currentPhase == BossPhase.HighReady) yield return HighReadyBehavior();
            else if (currentPhase == BossPhase.Shooting) yield return ShootingBehavior();
            else if (currentPhase == BossPhase.LowReady) yield return LowReadyBehavior();
            else if (currentPhase == BossPhase.ReturnToHighReady) yield return ReturnToHighReadyBehavior();
        }
        DeathSequence1();
    }
    private WaitForSeconds HighReadyBehavior()
    {
        if (shootCycles < maxNumberOfShootCycles)
        {
            am.Boss1Screech();
            currentMove = transform.DOMove(highReady, 1f, false).SetEase(Ease.InOutCubic);
            currentMove.OnComplete(GoToShootingPhase);
            void GoToShootingPhase() { currentPhase = BossPhase.Shooting; }
            return new WaitForSeconds(1f);
        }
        else 
        {
            shootCycles = 0;
            foreach (ParticleSystem ps in sideLasers) ps.Play();
            currentPhase = BossPhase.LowReady; 
            anim.SetBool("Extended", true); 
            currentMove = transform.DOMove(highReady, 4f, false).SetEase(Ease.InOutCubic);
            currentMove.OnComplete(MoveCompleted);
            void MoveCompleted() { currentMove = transform.DOMove(lowReady, 3f, false).SetEase(Ease.InOutCubic); }
            return new WaitForSeconds(7f);
        }
    }
    private WaitForSeconds ShootingBehavior() 
    {
        if (shootCycles >= maxNumberOfShootCycles)
        {
            currentPhase = BossPhase.HighReady;
            currentMove = transform.DOMove(highReady, 2f, false).SetEase(Ease.InOutCubic);
            return new WaitForSeconds(2f);
        }
        else if (shootCycles == 0)
        {
            anim.SetTrigger("NeutralToLeft");
            currentMove = transform.DOMove(new Vector3(leftPos.x *.5f, leftPos.y, leftPos.z), .5f, false).SetEase(Ease.InCubic);
            currentMove.OnComplete(MoveCompleted1);
            void MoveCompleted1() 
            {
                anim.SetTrigger("LeftToNeutral");
                currentMove = transform.DOMove(leftPos, .5f, false).SetEase(Ease.OutCubic);
                currentMove.OnComplete(MoveCompleted2);
                void MoveCompleted2() 
                {
                    foreach (ParticleSystem ps in guns) if (!ps.isPlaying) ps.Play(); else { ps.Stop(); ps.Play(); }
                    shootCycles++;
                }
            }
            return new WaitForSeconds(5f);
        }
        else if (shootCycles % 2 == 1)
        {
            anim.SetTrigger("NeutralToRight");
            currentMove = transform.DOMove(new Vector3(rightPos.x * .5f, rightPos.y, rightPos.z), 1f, false).SetEase(Ease.InCubic);
            currentMove.OnComplete(MoveCompleted1);
            void MoveCompleted1()
            {
                anim.SetTrigger("RightToNeutral");
                currentMove = transform.DOMove(rightPos, 1f, false).SetEase(Ease.OutCubic);
                currentMove.OnComplete(MoveCompleted2);
                void MoveCompleted2()
                {
                    foreach (ParticleSystem ps in guns) if (!ps.isPlaying) ps.Play(); else { ps.Stop(); ps.Play(); }
                    shootCycles++;
                }
            }
            return new WaitForSeconds(5f);
        }
        else
        {
            anim.SetTrigger("NeutralToLeft");
            currentMove = transform.DOMove(new Vector3(leftPos.x * .5f, leftPos.y, leftPos.z), 1f, false).SetEase(Ease.InCubic);
            currentMove.OnComplete(MoveCompleted1);
            void MoveCompleted1()
            {
                anim.SetTrigger("LeftToNeutral");
                currentMove = transform.DOMove(leftPos, 1f, false).SetEase(Ease.OutCubic);
                currentMove.OnComplete(MoveCompleted2);
                void MoveCompleted2()
                {
                    foreach (ParticleSystem ps in guns) if (!ps.isPlaying) ps.Play(); else { ps.Stop(); ps.Play(); }
                    shootCycles++;
                }
            }
            return new WaitForSeconds(5f);
        }
    }
    private WaitForSeconds ReturnToHighReadyBehavior()
    {
        anim.SetBool("Extended", false);
        currentMove = transform.DOMove(highReady, 4f, false).SetEase(Ease.InOutCubic);
        currentMove.OnComplete(MoveCompleted);
        void MoveCompleted() { currentMove = transform.DOMove(highReady, 3f, false).SetEase(Ease.InOutCubic); }
        currentPhase = BossPhase.HighReady;
        return new WaitForSeconds(7f);
    }
    private WaitForSeconds LowReadyBehavior()
    {
        if(currentlyZapping) return new WaitForSeconds(1f);
        else StartCoroutine(StartLightningAttack());
        return new WaitForSeconds(3f);
    }
    public IEnumerator StartLightningAttack()
    {
        currentlyZapping = true;
        foreach (ParticleSystem ps in sideLasers) ps.Stop();
        anim.SetBool("ReadyToAttack", true);
        am.LightningAttackCharge();
        yield return new WaitForSeconds(am.lightningAttackCharge.length);
        foreach (ParticleSystem ps in lightningAttacksPS) ps.Play();
        lightningCollider.enabled = true;
        anim.SetBool("Attacking", true);
        am.LightningAttack(true);
        yield return new WaitForSeconds(lightningAttackDuration);
        StopLightningAttack();
    }
    public void StopLightningAttack()
    {
        anim.SetBool("Attacking", false);
        anim.SetBool("ReadyToAttack", false);
        am.LightningAttack(false);
        am.LightningAttackUnCharge();
        foreach (ParticleSystem ps in lightningAttacksPS) ps.Stop();
        lightningAttacksPS[0].Clear();
        lightningCollider.enabled = false;
    }
    public void StopZapping()
    {
        currentlyZapping = false; 
        currentPhase = BossPhase.ReturnToHighReady;
    }

}
