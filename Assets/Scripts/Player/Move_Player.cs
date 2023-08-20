using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [Range(0, 10)][SerializeField] private float _speedPlayer = 1f;
    [Range(0, 20)][SerializeField] private float _jumpForce = 5f;
    [Space]
    [Header("Ground Cheker Settings")]
    [SerializeField] LayerMask _groundMask;
    [Range(0, 5)][SerializeField] float _rayDistance = 2.0f;
    [Space]
    [Header("Change Friction")]
    [SerializeField] PhysicsMaterial2D _lowFriction;
    [SerializeField] PhysicsMaterial2D _hightFriction;


    private float _horizontalMove = 0f;
    private Rigidbody2D _playerRb;
    private bool _isFacingRight = true;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * _rayDistance, Color.green);
        Move();
        Jump();
        ChangeFriction();
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new(_horizontalMove * 10,
                                             _playerRb.velocity.y);
        _playerRb.velocity = targetVelocity;
    }

    private void Move()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speedPlayer;
        if (_horizontalMove < 0f && _isFacingRight)
        {
            Flip();
        }
        else if (_horizontalMove > 0f && !_isFacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (CheckGrounding() && Input.GetKeyDown(KeyCode.Space))
        {
            _playerRb.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Flip() 
    {
        _isFacingRight = !_isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private bool CheckGrounding()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, Vector2.down, _rayDistance, _groundMask);
        return hit;
    }

    private void ChangeFriction()
    {
        if (_horizontalMove == 0f && CheckGrounding())
        {
            GetComponent<Collider2D>().sharedMaterial = _hightFriction;
        }
        else
        {
            GetComponent<Collider2D>().sharedMaterial = _lowFriction;
        }
    }


}
