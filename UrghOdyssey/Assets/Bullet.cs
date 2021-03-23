using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    public ParticleSystem _particleSystem;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.down * ((speed * 0.01f) * -1);

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            _particleSystem.Play();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _particleSystem.Play();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        Invoke("Destroy(this.gameObject)", 3f);
    }
}