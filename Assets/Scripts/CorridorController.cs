using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CorridorController : MonoBehaviour
{
    CameraController cam;
    bool entered = false;

    public bool isBoss = false, isSafe = false;

    public int hallLevel;
    public int roomNumber;

    public GameObject[] enemyPrefabs;

    public GameObject[] bosses;

    GameObject boss;

    GameObject barrier;

    bool destroyed = false;

    List<Enemy> enemies;
    
    void Start()
    {
        enemies = new List<Enemy>();
        
        cam = Camera.main.gameObject.GetComponent<CameraController>();
        int maxSpawn = Random.Range(1,3);
        if (!isBoss && !isSafe) {
            for (int i = 0; i <= hallLevel && i < maxSpawn; i++) {
                enemies.Add(SpawnEnemey(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]).GetComponent<Enemy>());
            }
        } else if (isBoss) {
            enemies.Add(SpawnBoss().GetComponent<Enemy>());
        }

        barrier = transform.Find("Barrier")?.gameObject;
    }

    void Update()
    {
        if (isBoss) {
            if (boss == null && !destroyed) {
                BarrierDestroyer destroyer = barrier.GetComponent<BarrierDestroyer>();
                destroyer.DestroyMe();
                destroyed = true;
            }
        }   
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !entered) {
            cam.targetPosition = transform.position;
            GameManager.instance.currentRoom = roomNumber;
            entered = true;
            foreach (var anemone in enemies) {
                anemone.active = true;
            }
        }    
    }

    GameObject SpawnEnemey(GameObject enemy) {
        float x = Random.Range(-4, 4);
        float y = Random.Range(-4, 4);

        Vector2 spawnPosition = transform.position + new Vector3(x, y, 0);

        GameObject instance = Instantiate(enemy, spawnPosition, Quaternion.identity);
        instance.transform.parent = transform;

        return instance;
    }

    GameObject SpawnBoss() {
        boss = Instantiate(bosses[hallLevel], transform.position, Quaternion.identity);
        boss.transform.parent = transform;
        return boss;
    }
}
