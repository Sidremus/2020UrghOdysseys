using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public int health = 10;
    public int scoreValue = 1;
    public ParticleSystem ps;
    UIManager ui;
    CircleCollider2D col;
    Rigidbody2D rb;
    AudioManager am;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        am = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIManager>();
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

    private void DeathSequence()
    {
        col.enabled = false;
        rb.Sleep();
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
    private void OnDestroy()
    {
        SendMessageUpwards("EnemyHasBeenDestroyed", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }
}