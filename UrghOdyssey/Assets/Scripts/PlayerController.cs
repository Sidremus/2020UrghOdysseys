using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    AudioManager am;
    Vector2 mov;
    UIManager ui;
    WaveManager wm;
    PolygonCollider2D col;
    SpriteRenderer sr;
    bool isShootingGun1, isShootingGun2, isShielding, isAlive = true;
    float shieldCharge, weaponCharge, shieldBarScaleX, weaponChargeBarScaleX;
    public bool danaeMode = true;
    public float moveForce = 10f, gun1Rate = 10f, gun2WaitForSeconds = .3f, maxShieldCharge = 3f, maxWeaponCharge = 6f;
    
    public ParticleSystem gun1;
    ParticleSystem.EmissionModule gun1Emission;
    public ParticleSystem gun2;
    public ParticleSystem gun3;
    
    public ParticleSystem[] shields;
    public CapsuleCollider2D shieldCol;
    public SpriteRenderer shieldBar, weaponChargeBar;
    
    public ParticleSystem[] deathPS;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        am = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIManager>();
        wm = FindObjectOfType<WaveManager>();
        col = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        gun1Emission = gun1.emission;

        shieldCharge = maxShieldCharge;
        weaponCharge = maxWeaponCharge;
        shieldBarScaleX = shieldBar.transform.localScale.x;
        weaponChargeBarScaleX = weaponChargeBar.transform.localScale.x;
    }
    private void Start()
    {
        am.StartMainMusicSequence();
    }
    void Update()
    {
        if (isAlive) 
        {
            if (Gamepad.all.Count == 0) ReadControls();
            else ReadControlsWithGamepad();
            UpdateShipAnimation();
            UpdateChargeMeters();
        }
        UpdateCharges();
    }
    private void UpdateShipAnimation()
    {
        anim.SetFloat("Direction", mov.x);
    }
    private void UpdateChargeMeters()
    {
        Vector3 shieldBarScale = shieldBar.transform.localScale;
        shieldBarScale.x = shieldBarScaleX * (shieldCharge / maxShieldCharge);
        shieldBar.transform.localScale = shieldBarScale;
        if (shieldCharge < maxShieldCharge * .2f) shieldBar.color = Color.gray;
        else shieldBar.color = Color.white;

        Vector3 weaponChargeBarScale = weaponChargeBar.transform.localScale;
        weaponChargeBarScale.x = weaponChargeBarScaleX * (weaponCharge / maxWeaponCharge);
        weaponChargeBar.transform.localScale = weaponChargeBarScale;
        if (weaponCharge < maxWeaponCharge * .2f) weaponChargeBar.color = Color.gray;
        else weaponChargeBar.color = Color.white;
    }
    private void UpdateCharges()
    {
        if (isShielding)
        {
            shieldCharge = Mathf.Clamp(shieldCharge - Time.deltaTime, 0f, maxShieldCharge);
            if (shieldCharge == 0f) StopShielding();
        }
        else
        {
            shieldCharge = Mathf.Clamp(shieldCharge + Time.deltaTime / 2f, 0f, maxShieldCharge);
        }
        if (isShootingGun1 || isShootingGun2)
        {
            weaponCharge = Mathf.Clamp(weaponCharge - Time.deltaTime, 0f, maxWeaponCharge);
            if (weaponCharge == 0f && !isShootingGun2) { StopShooting(); isShootingGun2 = true; StartCoroutine(ShootGun2()); }
        }
        else
        {
            weaponCharge = Mathf.Clamp(weaponCharge + Time.deltaTime, 0f, maxWeaponCharge);
        }
    }
    void FixedUpdate()
    {
        if (isAlive) rb.AddForce(mov.normalized * moveForce);
    }
    private void ReadControls()
    {
        //Move
        if ((Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed) || 
            (Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed)) mov.x = -1f; 
        else if ((Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed) ||
                 (Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed)) mov.x = 1f;
        else mov.x = 0f;

        if ((Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed) ||
            (Keyboard.current.upArrowKey.isPressed && !Keyboard.current.downArrowKey.isPressed)) mov.y = 1f; 
        else if ((Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed) ||
                 (Keyboard.current.downArrowKey.isPressed && !Keyboard.current.upArrowKey.isPressed)) mov.y = -1f;
        else mov.y = 0f;

        //Gun1
        if (Keyboard.current.vKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame) 
            StartShooting(); 
        if (Keyboard.current.vKey.wasReleasedThisFrame || Keyboard.current.kKey.wasReleasedThisFrame || Keyboard.current.spaceKey.wasReleasedThisFrame) 
            StopShooting();

        //Shield
        if (Keyboard.current.cKey.wasPressedThisFrame || Keyboard.current.lKey.wasPressedThisFrame || Keyboard.current.shiftKey.wasPressedThisFrame) 
            StartShielding(); 
        if (Keyboard.current.cKey.wasReleasedThisFrame || Keyboard.current.lKey.wasReleasedThisFrame || Keyboard.current.shiftKey.wasReleasedThisFrame) 
            StopShielding();
    }
    private void ReadControlsWithGamepad()
    {
        //Move
        if ((Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed) || 
            (Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed) ||
            Gamepad.current.dpad.left.isPressed ||
            Gamepad.current.leftStick.ReadValue().x < -.5f) mov.x = -1f; 
        else if ((Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed) ||
                 (Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed) ||
            Gamepad.current.dpad.right.isPressed ||
            Gamepad.current.leftStick.ReadValue().x > .5f) mov.x = 1f;
        else mov.x = 0f;

        if ((Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed) ||
            (Keyboard.current.upArrowKey.isPressed && !Keyboard.current.downArrowKey.isPressed) ||
            Gamepad.current.dpad.up.isPressed ||
            Gamepad.current.leftStick.ReadValue().y > .5f) mov.y = 1f; 
        else if ((Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed) ||
                 (Keyboard.current.downArrowKey.isPressed && !Keyboard.current.upArrowKey.isPressed) ||
            Gamepad.current.dpad.down.isPressed ||
            Gamepad.current.leftStick.ReadValue().y < -.5f) mov.y = -1f;
        else mov.y = 0f;

        //Gun1
        if (Keyboard.current.vKey.wasPressedThisFrame || 
            Keyboard.current.kKey.wasPressedThisFrame || 
            Gamepad.current.buttonEast.wasPressedThisFrame ||
            Gamepad.current.buttonWest.wasPressedThisFrame) 
            StartShooting(); 
        if (Keyboard.current.vKey.wasReleasedThisFrame || 
            Keyboard.current.kKey.wasReleasedThisFrame ||
            Gamepad.current.buttonEast.wasReleasedThisFrame ||
            Gamepad.current.buttonWest.wasReleasedThisFrame) 
            StopShooting();

        //Shield
        if (Keyboard.current.cKey.wasPressedThisFrame || 
            Keyboard.current.lKey.wasPressedThisFrame ||
            Gamepad.current.buttonSouth.wasPressedThisFrame ||
            Gamepad.current.buttonNorth.wasPressedThisFrame) 
            StartShielding(); 
        if (Keyboard.current.cKey.wasReleasedThisFrame || 
            Keyboard.current.lKey.wasReleasedThisFrame ||
            Gamepad.current.buttonNorth.wasReleasedThisFrame ||
            Gamepad.current.buttonSouth.wasReleasedThisFrame) 
            StopShielding();
    }
    private void StopShooting()
    {
        isShootingGun1 = false;
        isShootingGun2 = false;
        gun1Emission.rateOverTime = 0f;
        am.Gun1(false);
    }
    private IEnumerator ShootGun2()
    {
        while (isShootingGun2)
        {
            gun2.Play();
            am.Gun2();
            yield return new WaitForSeconds(gun2WaitForSeconds);
        }
    }
    private void StartShooting()
    {
        StopShielding();
        gun1Emission.rateOverTime = gun1Rate;
        isShootingGun1 = true;
        am.Gun1(isShootingGun1);
        if(weaponCharge == maxWeaponCharge)
        {
            gun3.Play();
            am.Gun3();
            weaponCharge -= .5f;
        }
    }
    private void StopShielding()
    {
        if (!isShielding) return;
        foreach (ParticleSystem ps in shields) ps.Stop();
        isShielding = false;
        StartCoroutine(DelayedShieldColliderDeactivation());
    }
    private IEnumerator DelayedShieldColliderDeactivation()
    {
        yield return new WaitForSeconds(.75f);
        if (!isShielding) 
        {
            am.Shielding(false);
            am.ShieldOff();
            shieldCol.enabled = false;
        }
    }
    private void StartShielding()
    {
        isShielding = true;
        StopShooting();
        if (shieldCharge < maxShieldCharge * .2f) { am.ShieldOff(); return; }
        foreach (ParticleSystem ps in shields) ps.Play();
        shieldCol.enabled = true;
        am.Shielding(true);
        am.ShieldOn();
    }
    private void OnParticleCollision(GameObject other)
    {
        if (danaeMode) return;
        if (!isAlive) return;
        if (shieldCol.enabled) weaponCharge = Mathf.Clamp(weaponCharge + maxWeaponCharge / 20f, 0f, maxWeaponCharge);
        else StartCoroutine("ReceiveHit");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (danaeMode) return;
        if(!shieldCol.enabled)
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) StartCoroutine("ReceiveHit");
    }
    private IEnumerator ReceiveHit()
    {
        isAlive = false;
        col.enabled = false;
        GlobalStats.PlayerDeaths += 1;
        GlobalStats.PlayerLives--;
        if (GlobalStats.PlayerLives >= 0) ui.UpdateLifeCounter();
        StopShielding();
        isShootingGun1 = false;
        isShootingGun2 = false;
        StopShooting();
        foreach (ParticleSystem ps in deathPS) ps.Play();
        am.PlayerDeath1();
        yield return new WaitForSeconds(2.2f);
        am.PlayerDeath2();
        sr.enabled = false;
        yield return new WaitForSeconds(3f);
        wm.ResetCurrentWave();
        if (GlobalStats.PlayerLives < 0) GameOver();
        else
        {
            transform.position = new Vector3(0f, -0.4f, 0f);
            isAlive = true;
            sr.enabled = true;
            col.enabled = true;
        }
    }
    private void GameOver()
    {
        SceneManager.LoadScene(3);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {
            ReceiveHit();
        }
    }
}
