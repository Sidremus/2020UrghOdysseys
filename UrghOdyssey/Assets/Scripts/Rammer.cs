using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rammer : MonoBehaviour
{
    public int health = 10, scoreValue = 5;
    float activationHeight = .75f, targetingTime = 2f, boostForce = 2f;
    public ParticleSystem rammerDeathPS;
    public ParticleSystem rammerEnginePS;
    bool isTargeting;
    UIManager ui;
    CircleCollider2D col;
    Rigidbody2D rb;
    AudioManager am;
    PlayerController player;
    Animator anim;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        am = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIManager>();
        player = FindObjectOfType<PlayerController>();
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
    private void Update()
    {
        if (transform.position.y < activationHeight && !isTargeting) { isTargeting = true; StartCoroutine(RammingBehavior()); }
    }

    private IEnumerator RammingBehavior()
    {
        rb.gravityScale = 0f;
        float oldRotation = transform.rotation.eulerAngles.z;
        rammerEnginePS.Play();
        float i = 0f;
        while (i < targetingTime)
        {
            transform.up = Vector3.Slerp(transform.up, (player.transform.position - transform.position) * -1, i * .75f);
            i += .05f;
            if (transform.rotation.eulerAngles.z > oldRotation + 1f) anim.SetFloat("Direction", 1f);
            else if (transform.rotation.eulerAngles.z < oldRotation - 1f) anim.SetFloat("Direction", -1f);
            else anim.SetFloat("Direction", 0f);
            oldRotation = transform.rotation.eulerAngles.z;
            yield return new WaitForSeconds(.05f);
        }
        anim.SetFloat("Direction", 0f);
        transform.up = (player.transform.position - transform.position) * -1;
        rb.drag = 0f;
        rb.AddForce(Vector3.Normalize(player.transform.position - transform.position) * boostForce, ForceMode2D.Impulse);
        yield return null;
    }

    private void DeathSequence()
    {
        col.enabled = false;
        GlobalStats.EnemiesKilled++;
        rb.Sleep();
        Instantiate(rammerDeathPS, transform.position, Quaternion.identity);
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

    private void OnDestroy()
    {
        SendMessageUpwards("EnemyHasBeenDestroyed", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }
}
