using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public float speed = 3f;
    public int health = 3;
    //public GameObject deathEffect;
    private Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            transform.LookAt(player);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("현제 적 채력: " + health);
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        /*if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }*/
        Destroy(gameObject);
    }
}