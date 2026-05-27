using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float _speed;

    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _screenBorder;

    private Rigidbody2D _rigidbody2D;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldowm;

    private Camera _camera;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleEnemyOffScreen();
        
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldowm -= Time.deltaTime;
        if (_changeDirectionCooldowm <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;
            _changeDirectionCooldowm = Random.Range(1f, 5f);
        }
        
    }

    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        
        if ((screenPosition.x < _screenBorder && _targetDirection.x < 0) || 
        (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
            
        }

        if ((screenPosition.y < _screenBorder && _targetDirection.y < 0) ||
        (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0))
        {
           _targetDirection = new Vector2(_targetDirection.x,-_targetDirection.y);
        }
        
        
    }
    private void RotateTowardsTarget()
    {
        

        // 1. Calculate the angle in degrees
        // Atan2 returns the angle where 0 degrees is to the RIGHT (perfect for your sprite)
        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;

        // 2. Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // 3. Smoothly rotate towards it
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody2D.MoveRotation(rotation);
    }

   
    private void SetVelocity()
    {
        _rigidbody2D.linearVelocity = transform.right * _speed;
    }
}
