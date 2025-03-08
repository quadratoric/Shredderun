using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float speed, shootCoolDown, damage;

    public float xpValue, detectionDistance;

    public GameObject bulletPrefab, xpSquarePrefab;

    public LayerMask enemyLayer;

    protected GameObject player;
    Health health;
    protected bool inRange;

    public bool active = true; // set this to false if you want to make an object only start attacking when the player enters the room.
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        health.OnDeath += EnemyDeath;
        GameManager.instance.enemeyHealths.Add(health);
    }

    protected virtual void Update()
    {
        if (!active) {
            GetComponent<Collider2D>().enabled = false;
            return;
        } else {
            GetComponent<Collider2D>().enabled = true;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized, detectionDistance, ~enemyLayer);
        inRange = false;
        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == "Player") {
                inRange = true;
            }
        } 
    }

    void EnemyDeath() {
        GameObject xp = Instantiate(xpSquarePrefab, transform.position, Quaternion.identity);
        xp.GetComponent<XPSquare>().xpValue = xpValue;
    }

    
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        return;
        
        
        Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}
