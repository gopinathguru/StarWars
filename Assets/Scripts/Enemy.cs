using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy")]
    [SerializeField] int health = 500;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] AudioClip explosionAFX;
    [SerializeField] float explosionVolume=1f;

    [Header("Enemy Laser settings:")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaser;
    [SerializeField] float projectileSpeed = 10f;

    // Use this for initialization
    void Start () {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

    }

    private void CountDownAndFire()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

    }

    // Update is called once per frame
    void Update () {
        CountDownAndFire();


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            GameObject explosionEffect = Instantiate(explosionVFX, transform.position, Quaternion.identity) as GameObject;
            Destroy(explosionEffect, 1f);
            AudioSource.PlayClipAtPoint(explosionAFX, Camera.main.transform.position, explosionVolume);

            Destroy(gameObject);
        }
    }
}
