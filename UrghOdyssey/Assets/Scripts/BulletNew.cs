using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : MonoBehaviour
{
    public float speed;
    public ParticleSystem poof;
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
    private void Start()
    {
        Destroy(gameObject, 4f);
    }
    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = Instantiate(poof, transform.position, Quaternion.identity);
        ps.transform.parent = null;
        Destroy(gameObject);
    }
}
