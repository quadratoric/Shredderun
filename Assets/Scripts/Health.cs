using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;

    public bool disableOnDeath = false;

    public float currentHealth;

    public event Action OnDeath;

    public bool ignoreColissions = false;

    protected virtual void Start() {
        if (transform.gameObject.tag == "Player") {
            maxHealth += GameManager.instance.playerHealthModifier;
            currentHealth = maxHealth;
        } else {
            maxHealth += GameManager.instance.enemyHealthModifier;
            currentHealth = maxHealth;
        }
    
    }

    public virtual void ChangeHealth(float amount) {
        if (ignoreColissions) {
            return;
        }
        
        currentHealth += amount;

        if (currentHealth < 1) {
            currentHealth = 0;
            OnDeath?.Invoke();
            if (!disableOnDeath) {
                Destroy(gameObject);
            }
        }
    }

    public virtual void ChangeMaxHealth(float amount) {
        float percentage = currentHealth / maxHealth;
        maxHealth += amount;
        currentHealth = maxHealth * percentage;
    }
}
