﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{

    [Header("Player settings:")]
    [SerializeField] [Range(100, 10000)] int health = 1000;
    [SerializeField] [Range(1f, 50f)] float playerSpeed = 10f;
    [SerializeField] AudioClip playerDeathAFX;
    [SerializeField] float playerDeathAFXVolume = 1f;

    [Header("Player Laser")]
    [SerializeField] GameObject playerLaser;
    [SerializeField] AudioClip playerLaserSound;
    [SerializeField] [Range(0f,1f)] float playerLaserSoundVolume = 1f;
    [SerializeField] [Range(0f, 100f)] float projectileSpeed = 10f;
    [SerializeField] [Range(0f, 1f)] float projectileFiringPeriod = 0.1f;

    float padding = 1f;
    Coroutine firingCoroutine;

    float xMin;
    float xMax;

    float yMin;
    float yMax;

    // Use this for initialization
    void Start () {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding; 
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update () {
        MovePlayer();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(playerLaser, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerLaserSound, Camera.main.transform.position, playerLaserSoundVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void MovePlayer()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        var newXPos = transform.position.x + deltaX;
        newXPos = Mathf.Clamp(newXPos, xMin, xMax);
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
        var newYPos = transform.position.y + deltaY;
        newYPos = Mathf.Clamp(newYPos, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(playerDeathAFX, Camera.main.transform.position, playerDeathAFXVolume);
            FindObjectOfType<Level>().LoadEndGame();
        }
    }

    public int GetPlayerHealth()
    {
        return health;
    }

}
