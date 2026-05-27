using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
     private Rigidbody2D _rigidbody2D;
     private Vector2 _movementInput;
     private Vector2 _smooothedMovementInput;

     private Vector2 _movementInputSmoothVelocity;

     private Camera _camera;
      private Animator _animator;

     [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _screenBorder;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        RotateInDirectionOfInput();
        SetAnimation();
    }

    private void SetAnimation()
    {
        bool isMoving = _movementInput != Vector2.zero;

        _animator.SetBool("IsMoving", isMoving);
    }

    private void SetPlayerVelocity()
    {
        _smooothedMovementInput = Vector2.SmoothDamp(_smooothedMovementInput, _movementInput, ref _movementInputSmoothVelocity, 0.1f);
        _rigidbody2D.linearVelocity = _smooothedMovementInput * _speed;
        PreventPLayerGoingOffScreen();
    }

    private void PreventPLayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        
        if ((screenPosition.x < _screenBorder && _rigidbody2D.linearVelocity.x < 0) || 
        (screenPosition.x > _camera.pixelWidth - _screenBorder && _rigidbody2D.linearVelocity.x > 0))
        {
            _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);
            
        }

        if ((screenPosition.y < _screenBorder&& _rigidbody2D.linearVelocity.y < 0) ||
        (screenPosition.y > _camera.pixelHeight - _screenBorder && _rigidbody2D.linearVelocity.y > 0))
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0);
        }
        
    }

    private void RotateInDirectionOfInput()
    {
        if (_smooothedMovementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smooothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            
            _rigidbody2D.MoveRotation(rotation);
        }
    }


    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
       
    }
}
