﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Firefly : MonoBehaviour
{
    [Header("Vertical Motion")]
    [SerializeField]
    private float amplitude = 0.8f;

    [SerializeField]
    private float frequency = 1f;

    [Header("Score")]
    [SerializeField]
    private float pointsPerSpeed = 3;

    private float _moveSpeed = 5;

    private int _pointValue;

    private Vector2 _direction;

    private Vector3 _newPosition;

    private float _randomSeed;

    public int Points => _pointValue;

    private void Start()
    {
        _randomSeed = Random.Range(-Mathf.PI * 0.5f, Mathf.PI * 0.5f);
    }

    public void Setup(float speed, Vector2 direction)
    {
        _pointValue = Mathf.CeilToInt(speed * pointsPerSpeed);
        _direction = direction;
        _moveSpeed = speed;
    }

    private void Move()
    {
        var speed = _moveSpeed * Time.deltaTime;
        transform.Translate(_direction * speed);
    }

    private void Update()
    {
        _newPosition = transform.position;
        _newPosition.y += Mathf.Sin(Time.time * frequency * 2 * Mathf.PI + _randomSeed) * amplitude * Time.deltaTime;
        transform.position = _newPosition;
        Move();

        RemoveFirefly();
    }

    private void RemoveFirefly()
    {
        var mainCamera = Camera.main;
        Debug.Assert(mainCamera != null, "mainCamera != null");
        var position = mainCamera.WorldToViewportPoint(_newPosition);
        var isOutside = _direction == Vector2.right && position.x > 1 ||
                        _direction == Vector2.left && position.x < 0;
        if (isOutside) Destroy(gameObject);
    }
}
