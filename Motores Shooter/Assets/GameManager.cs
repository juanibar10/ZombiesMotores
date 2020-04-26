﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Material mat;
    public Transform player;
    public float revealSpeed;
    public AnimationCurve curve;
    float contador = 0;


    [Header("Zombies")]
    public int zombiesInMap;
    public int zombiesLeftToSpawn;

    public LayerMask spawnerLayer;
    public Spawner[] spawners;
    public List<Spawner> spawnerAroundPlayer = new List<Spawner>();


    private void Awake()
    {
        mat.SetInt("end", 1);
        mat.SetVector("PlayerPos", player.position);
        StartCoroutine(SetupMap());
        StartCoroutine(ZombieGeneration(5));
    }

    private void Update()
    {
        DetectPlayerNearSpawners();
    }

    IEnumerator SetupMap()
    {
        float forTime = 40f;
        print("a");
        for (int i = 0; i <= forTime; i++)
        {
            mat.SetFloat("revealScale", Mathf.Lerp(0, 1000, curve.Evaluate(i * (1f / forTime))));
            yield return new WaitForSeconds(1 * (1f / forTime));
        }
    }

    void DetectPlayerNearSpawners()
    {
        spawnerAroundPlayer = new List<Spawner>();

        Collider[] hitColliders = Physics.OverlapSphere(player.position, 20);

        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Spawner")
                spawnerAroundPlayer.Add(col.GetComponent<Spawner>());
        }
    }

    IEnumerator ZombieGeneration(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        int num = 0;
        num = Random.Range(0, spawnerAroundPlayer.Count);

        if (!spawnerAroundPlayer[num].spawning)
        {
            spawnerAroundPlayer[num].OnSpawnZombie();

            tiempo = Random.Range(1, 4);
            StartCoroutine(ZombieGeneration(tiempo));
        }
    }
}
