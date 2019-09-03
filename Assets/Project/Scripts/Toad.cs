using System.Runtime.CompilerServices;
using UnityEngine;

public class Toad : MonoBehaviour
{
    [SerializeField]
    private int player;

    [SerializeField]
    private float jumpSpeed = 1;

    [SerializeField]
    private Transform[] jumpPathPoints;

    private bool _grounded;

    private bool _reachedTargetPoint;

    private int _positionIndex;

    private Vector2 _jumpDirection;

    public int Player => player;

    private void Awake()
    {
        _grounded = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _grounded = false;
            _jumpDirection = _positionIndex == 0 ? Vector2.right : Vector2.left;
            Debug.Log("Jump!");
        }

        Move();
    }

    private void Move()
    {
        if (!_grounded)
        {
            _reachedTargetPoint = Vector2.Distance(transform.position, jumpPathPoints[_positionIndex].position) <= .1f;
            if (_jumpDirection == Vector2.right)
            {
                if (_reachedTargetPoint)
                {
                    ++_positionIndex;
                }

                if (ReachedLandingPad())
                {
                    _positionIndex = jumpPathPoints.Length - 1;
                }
            }
            else
            {
                if (_reachedTargetPoint)
                {
                    --_positionIndex;
                }

                if (ReachedLandingPad())
                {
                    _positionIndex = 0;
                }
            }
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            jumpPathPoints[_positionIndex].position,
            jumpSpeed * Time.deltaTime);
    }

    private bool ReachedLandingPad()
    {
        var isAtRight = _jumpDirection == Vector2.right && _positionIndex >= jumpPathPoints.Length;
        var isAtLeft = _jumpDirection == Vector2.left && _positionIndex < 0;
        if (!isAtLeft && !isAtRight) return false;

        transform.Rotate(Vector3.up, 180);
        _grounded = true;
        return true;
    }
}
