using UnityEngine;

public class Firefly : MonoBehaviour
{
    [Header("Horizontal Motion")]
    [SerializeField]
    private float moveSpeed = 5;

    [Header("Vertical Motion")]
    [SerializeField]
    private float amplitude = 0.8f;

    [SerializeField]
    private float frequency = 1f;

    [Header("Score")]
    [SerializeField]
    private int pointValue;

    private Vector2 _direction;

    private Vector3 _newPosition;

    private void Update()
    {
        _newPosition = transform.position;
        _newPosition.y += Mathf.Sin(Time.time * frequency * 2 * Mathf.PI) * amplitude * Time.deltaTime;
        transform.position = _newPosition;
    }
}
