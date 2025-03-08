using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinalBoss : Enemy
{
    bool canAttack = true;

    public AudioClip sound;

    public GameObject miniMe;

    public float spinCooldown = 2f;
    public float spinDuration = 2f;
    public float spinSpeed = 1f;

    public float deathTime = 5f;
    public float healthDivider = 10;
    public float enemySpawnSpacer = 0.2f;
    
    FinalBossHealth fakeHealth;

    public Collider2D myCollider;
    public SpriteRenderer myRenderer;

    List<GameObject> miniMes;

    protected override void Start()
    {
        base.Start();
        fakeHealth = GetComponent<FinalBossHealth>();
        fakeHealth.OnDeath += MainBodyDied;
        active = true;
    }

    void MainBodyDied() {
        Debug.Log("Reached Parent");
        fakeHealth.ignoreColissions = true;
        StartCoroutine(DeathSquence());
    }

    IEnumerator DeathSquence() {
        Debug.Log("Spawning started");
        
        miniMes = new List<GameObject>();

        Vector3 originalScale = transform.localScale;

        float healthMax = fakeHealth.maxHealth;

        int enemiesToSpawn = (int) (healthMax / healthDivider);
        Vector3 step = originalScale / enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++) {
            Vector3 newScale = transform.localScale - step;
            newScale.x = Mathf.Max(newScale.x, 0);
            newScale.y = Mathf.Max(newScale.y, 0);
            newScale.z = Mathf.Max(newScale.z, 0);
            transform.localScale = newScale;
            
            miniMes.Add(Instantiate(miniMe, transform.position, Quaternion.Euler(0,0,Random.Range(0, 360))));
            
            Physics2D.IgnoreCollision(miniMes[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());            
            
            yield return new WaitForSeconds(enemySpawnSpacer);
        }
        active = false;

        yield return new WaitForSeconds(deathTime);

        // RESPAWNING
        active = true;
        int remaining = 0;
        foreach (var mini in miniMes) {
            if (mini != null) {
                remaining++;
                
                mini.GetComponent<MiniMover>().ReturnHome(transform.position);
                yield return new WaitForSeconds(enemySpawnSpacer + 0.3f);
                Vector3 newScale = transform.localScale + step;
                newScale.x = Mathf.Min(newScale.x, originalScale.x);
                newScale.y = Mathf.Min(newScale.y, originalScale.y);
                newScale.z = Mathf.Min(newScale.z, originalScale.z);
                transform.localScale = newScale;

                fakeHealth.currentHealth += healthDivider;
            }
        }

        if (remaining == 0) {
            Destroy(gameObject);
        }

        fakeHealth.maxHealth = fakeHealth.currentHealth;

        fakeHealth.ignoreColissions = false;
    }

    protected override void Update()
    {
        base.Update();

        if (canAttack && inRange) {
            StartCoroutine(Attack());
        }
    }

    bool spin = false;

    void FixedUpdate()
        {
            if (!inRange) {
            return;
        }

        if (!spin) {

            Vector2 direction = player.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Smoothly rotate towards the target angle over time
            float smoothAngle = Mathf.LerpAngle(rb.rotation, targetAngle, 1.5f * Time.fixedDeltaTime);
            
            rb.MoveRotation(smoothAngle);
        } else {
            float newAngle = rb.rotation + spinSpeed * Time.fixedDeltaTime * 50;
            rb.MoveRotation(newAngle);
            return;
        }
    }

    bool alternate = true;

    IEnumerator Attack() {
        yield return null;
    }
}
