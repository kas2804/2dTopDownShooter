using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    [SerializeField] private float _timeBetweenShots;

    // NEW: Variable to hold your sound file
    [SerializeField] private AudioClip _shootSound; 

    // NEW: Variable to hold the speaker
    private AudioSource _audioSource; 

    private bool _fireContinuosly;
    private float _lastFireTime;
    private bool _fireSingle;

    // NEW: Get the Audio Source when the game starts
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_fireContinuosly || _fireSingle)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;
            if(timeSinceLastFire >= _timeBetweenShots)
            {
                FireBullet();
                _lastFireTime = Time.time;
                _fireSingle = false;
            }
        }
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        rigidbody.linearVelocity = _bulletSpeed * transform.up;

        // NEW: Play the sound effect
        if (_shootSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_shootSound);
        }
    }

    private void OnFire(InputValue inputValue)
    {
        _fireContinuosly = inputValue.isPressed;

        if (inputValue.isPressed)
        {
            _fireSingle = true;
        }
    }
}