using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    public ParticleSystem _particleSystem;
    public GameObject[] children;

    public float speed;


    void Update()
    {
        transform.position -= Vector3.down * ((speed * 0.01f) * -1);
    }

    private void Start()
    {
        children = GetComponentsInChildren<GameObject>();
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Destroy(children[0]);
        Destroy(children[1]);
        
        _particleSystem.Play();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Destroy(children[0]);
        Destroy(children[1]);
        
        _particleSystem.Play();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }
}