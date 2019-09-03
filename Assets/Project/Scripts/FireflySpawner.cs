﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireflySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField]
    private Firefly[] fireflyPrefabs;

    [SerializeField]
    private float minSpawnDelay = 2;

    [SerializeField]
    private float maxSpawnDelay = 6;

    [Header("Spawn Offset")]
    [SerializeField]
    private float minOffset = -2;

    [SerializeField]
    private float maxOffset = 2;

    [Header("Scoring")]
    [SerializeField]
    private int minPower = 2;

    [SerializeField]
    private int maxPower = 6;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var position = transform.position;
        var spawnOffset = Random.Range(minOffset, maxOffset);

        var selectedPrefab = fireflyPrefabs[Random.Range(0, fireflyPrefabs.Length)];
        var firefly = Instantiate(selectedPrefab,
            new Vector3(position.x, position.y + spawnOffset, position.z),
            Quaternion.identity);

        var speed = Random.Range(minPower, maxPower);
        firefly.Setup(speed, transform.right);

        var spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(Spawn());
    }

}