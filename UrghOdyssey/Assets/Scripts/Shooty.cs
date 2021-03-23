using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum axis
{
    X,
    Y,
    XANDY
}


public class Shooty : MonoBehaviour
{
    public int health = 10, scoreValue = 3;
    public bool useRandomShootingDelay;
    public float timeUntilFirstShot = 1.5f, timeBetweenShots = 1.5f;
    public ParticleSystem ps;
    UIManager ui;
    CircleCollider2D col;
    AudioManager am;
    Rigidbody2D rb;

    private float activationHeight = .75f;
    public bool hasReachedHeight = false;
    public axis axis;

    public float targetWorldspaceCoordinate;
    public Vector2 doubledirection;
    public float duration;
    public Ease ease;
    public int loops;

    public GameObject bullet;

    private Animator _animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        am = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIManager>();
        _animator = GetComponent<Animator>();
    }

    private void DeathSequence()
    {
        col.enabled = false;
        GlobalStats.EnemiesKilled++;
        Instantiate(ps, transform.position, Quaternion.identity);
        am.GearExplosion();
        ui.IncreaseScoreCounter(scoreValue);
        Destroy(gameObject, .3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) DeathSequence();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnableEnemies")) col.isTrigger = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnableEnemies"))
        {
            Destroy(gameObject, .3f);
        }
    }

    private void Update()
    {
        if (transform.position.y < activationHeight && !hasReachedHeight)
        {
            hasReachedHeight = true;
            StartCoroutine(MoveAround());
            StartCoroutine(ShootLoop());
            StartCoroutine(CalculateSpeed());
        }

        if (transform.position.x > oldX) _animator.SetFloat("Dir", -1);
        else if (transform.position.x < oldX) _animator.SetFloat("Dir", 1);
        //else _animator.SetFloat("Dir", 0);
        oldX = transform.position.x;

        if (hasReachedHeight)
       
        /*switch (posOrNeg)
        {
            case -1:
                _animator.SetInteger("Velocity", -2);
                break;
            case 1:
                _animator.SetInteger("Velocity", 2);
                break;
        }*/


        if (posOrNeg != 0)
        {
            //Debug.Log($"Name : {name}\n Direction : {posOrNeg}\n State : {_animator.GetInteger("Velocity")}");
        }
    }

    private float step1, step2, posOrNeg, oldX;


    private IEnumerator CalculateSpeed()
    {
        while (true)
        {
            step1 = transform.position.x;
            yield return new WaitForSeconds(.01f);
            step2 = transform.position.x;

            if (step2 > step1)
            {
                posOrNeg = 1;
            }
            else if (step2 < step1)
            {
                posOrNeg = -1;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        health--;
        am.PlayerGunImpact();
        if (health <= 0)
        {
            DeathSequence();
        }
    }

    private IEnumerator MoveAround()
    {
        rb.gravityScale = 0f;
        rb.drag = 0f;
        rb.isKinematic = true;

        switch (axis)
        {
            case axis.X:
                rb.DOMoveX(targetWorldspaceCoordinate, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo);
                break;

            case axis.Y:
                rb.DOMoveY(targetWorldspaceCoordinate, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo);
                break;

            case axis.XANDY:
                rb.DOMove(doubledirection, duration).SetEase(ease).SetLoops(loops, LoopType.Yoyo);
                break;
        }

        yield return null;
    }


    public IEnumerator ShootLoop()
    {
        if (useRandomShootingDelay)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, timeBetweenShots));
        }
        else
        {
            yield return new WaitForSeconds(timeUntilFirstShot);
        }

        while (true)
        {
            ps.Stop();
            ps.Play();
            Instantiate(bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void OnDestroy()
    {
        SendMessageUpwards("EnemyHasBeenDestroyed", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }
}