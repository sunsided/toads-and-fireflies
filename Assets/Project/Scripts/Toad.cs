using System.Collections;
using UnityEngine;

public class Toad : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private int player;

    [Header("Jumping")]
    [SerializeField]
    private float jumpSpeed = 1;

    [SerializeField]
    private Transform[] jumpPathPoints;

    [Header("Attacking")]
    [SerializeField]
    private Tongue tongue;

    [SerializeField]
    private float tongueDuration = 0.7f;

    [Header("Sprites")]
    [SerializeField]
    private Sprite idleSprite;

    [SerializeField]
    private Sprite jumpSprite;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip tongueClip;

    [SerializeField]
    private AudioClip jumpClip;

    [SerializeField]
    private AudioClip eatClip;

    [SerializeField]
    private float minPitch = 0.9f;

    [SerializeField]
    private float maxPitch = 1.2f;

    private bool _grounded;

    private bool _reachedTargetPoint;

    private int _positionIndex;

    private Vector2 _jumpDirection;

    private SpriteRenderer _renderer;

    private AudioSource _audioSource;

    public int Player => player;

    public void EatSomething()
    {
        PlayWithRandomPitch(eatClip);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();
        _grounded = true;
    }

    private void Update()
    {
        // Don't accept inputs when time is frozen.
        if (Time.timeScale <= 0) return;

        var jumpButton = Input.GetButtonDown($"Action{player}");
        if (jumpButton && _grounded)
        {
            _grounded = false;
            _jumpDirection = _positionIndex == 0 ? Vector2.right : Vector2.left;
            _renderer.sprite = jumpSprite;

            PlayWithRandomPitch(jumpClip);
        }
        else if (jumpButton)
        {
            Debug.Assert(!_grounded, "!_grounded");
            StartCoroutine(Attack());
        }

        Move();
    }

    private IEnumerator Attack()
    {
        PlayWithRandomPitch(tongueClip);

        tongue.gameObject.SetActive(true);
        yield return new WaitForSeconds(tongueDuration);
        tongue.gameObject.SetActive(false);
    }

    private void PlayWithRandomPitch(AudioClip clip)
    {
        _audioSource.pitch = Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(clip);
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

        tongue.gameObject.SetActive(false);

        transform.Rotate(Vector3.up, 180);
        _renderer.sprite = idleSprite;
        _grounded = true;
        return true;
    }
}
