using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathBarrierController : MonoBehaviour
{
    int currentTarget;

    public GameObject deathRemnant;

    List<GameObject> remnants;

    GameObject generator;
    List<Vector3> positions;

    void Start()
    {
        currentTarget = 0;
        generator = GameObject.FindGameObjectWithTag("Generator");
        positions = generator.GetComponent<HallGenerator>().pathPositions;
        positions.Insert(0, Vector3.zero);
        remnants = new List<GameObject>();
    }


    void Update()
    {
        GameManager.instance.deathRoom = currentTarget - 1;
        
        if (currentTarget > positions.Count) {
            return;
        }

        Vector3 targetPosition = positions[currentTarget];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, GameManager.instance.fractureSpeedModifer * Time.deltaTime);

        if (Vector3.Distance(targetPosition, transform.position) <= 0.01f) {
            currentTarget++;
            GameObject remnant = Instantiate(deathRemnant, targetPosition, Quaternion.identity);
            remnant.transform.parent = transform.parent;
            remnants.Add(remnant);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null && collision.gameObject.tag == "Player") {
            health.ChangeHealth(-5 * Time.deltaTime * 10);
        }
    }
}
