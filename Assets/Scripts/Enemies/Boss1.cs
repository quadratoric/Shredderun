using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy
{
    bool canAttack = true;

    public AudioClip sound;

    public Transform[] cardinals;
    public Transform[] interCardinals;

    public float spinCooldown = 2f;
    public float spinDuration = 2f;
    public float spinSpeed = 1f;
    

    protected override void Start()
    {
        base.Start();
        //active = false;
        StartCoroutine(SpinRoutine());
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

    IEnumerator SpinRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spinCooldown, spinCooldown + 3f)); // Adds randomness to the spin intervals
            spin = true;
            yield return new WaitForSeconds(spinDuration);
            spin = false;
        }
    }

    bool alternate = true;

    IEnumerator Attack() {
        canAttack = false;
        
        float modifier = 3 * Mathf.Log10(Mathf.Pow(GameManager.instance.enemyShootModifier, 2) + 1);

        yield return new WaitForSeconds(shootCoolDown / modifier);

        canAttack = true;
        
        Debug.Log("I was called");

        if (spin) {
            foreach (var t in  cardinals) {
                GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation);
                BulletController bulletCon = bullet.GetComponent<BulletController>();
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                bulletCon.damageValue = damage;
                alternate = false;
            }
            foreach (var t in interCardinals) {
                GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation);
                BulletController bulletCon = bullet.GetComponent<BulletController>();
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                bulletCon.damageValue = damage;
                alternate = true;
            }
        } else if (alternate) {
            foreach (var t in  cardinals) {
                GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation);
                BulletController bulletCon = bullet.GetComponent<BulletController>();
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                bulletCon.damageValue = damage;
                alternate = false;
            }
        } else {
            foreach (var t in interCardinals) {
                GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation);
                BulletController bulletCon = bullet.GetComponent<BulletController>();
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                bulletCon.damageValue = damage;
                alternate = true;
            }
        }
        
        AudioManager.instance.PlaySFX(sound, 0.5f);
    }
}
