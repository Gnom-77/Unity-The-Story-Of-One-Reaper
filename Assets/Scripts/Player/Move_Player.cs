using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Move_Player : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [Range(0, 10)][SerializeField] private float _speedPlayer = 1f;
    [Range(0, 20)][SerializeField] private float _jumpForce = 5f;
    [Range(0, 20)][SerializeField] private float _jumpForceIfInTexture = 1f;
    [Space]
    [Header("Ground Cheker Settings")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _groundCheckInPlayer;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] LayerMask _groundMask;
    [Range(0, 5)][SerializeField] float _rayDistance = 2.0f;
    [Space]
    [Header("Change Friction")]
    [SerializeField] PhysicsMaterial2D _lowFriction;
    [SerializeField] PhysicsMaterial2D _hightFriction;
    [Space]
    [Header("Slope Movement")]
    [Range(0, 20)][SerializeField] private float _slopeForceRayLength = 5f;
    [Header("Player Dialogue Panel")]
    [SerializeField] GameObject _playerDialoguePanel;

    private Rigidbody2D _playerRb;

    private Vector2 _targetVelocity;
    private Vector2 _colliderSize;
    private Vector2 _slopeNormalPerpendecular;

    private float _horizontalMove = 0f;
    private float _slopeDownAngle;
    private float _slopeDownAngleOld;

    private bool _isFacingRight = true;
    private bool _isOnSlope;
    private bool _isJumping = false;

    private void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _colliderSize = GetComponent<CapsuleCollider2D>().size;
    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, Vector2.down * _slopeForceRayLength, Color.grey);
        Debug.DrawRay(_groundCheck.position, Vector2.down * _rayDistance, Color.red);
        if (!_playerDialoguePanel.activeSelf)
        {
            Move();
            Jump();
        }
        else if (_playerDialoguePanel.activeSelf)
        {
            _horizontalMove = 0;
        }
        ChangeFriction();
    }

    private void FixedUpdate()
    {
        CheckColliderInPlayer();
        SlopeAndChangePosition();
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
            _isJumping = true;
        }
        if (_playerRb.velocity.y <= 0.0f)
        {
            _isJumping = false;
        }
    }

    private void Flip() 
    {
        _isFacingRight = !_isFacingRight;
        Vector3 theScale = transform.localScale;
        Vector3 theScalePanel = _playerDialoguePanel.transform.localScale;
        theScale.x *= -1;
        theScalePanel.x *= -1;
        transform.localScale = theScale;
        _playerDialoguePanel.transform.localScale = theScalePanel;
    }

    private void SlopeAndChangePosition()
    {
        CheckSloping();

        ChangePlayerPosition();

        if (CheckGrounding() && !_isOnSlope && !_isJumping)
        {
            _targetVelocity.Set(_horizontalMove * 10, 0.0f);
            _playerRb.velocity = _targetVelocity;
        }
        else if (CheckGrounding() && _isOnSlope && !_isJumping)
        {
            _targetVelocity.Set(_horizontalMove * _slopeNormalPerpendecular.x * -10,
                _horizontalMove * _slopeNormalPerpendecular.y * -10);
            _playerRb.velocity = _targetVelocity;
        }
        else if (!CheckGrounding())
        {
            _targetVelocity.Set(_horizontalMove * 10,
                                                 _playerRb.velocity.y);
            _playerRb.velocity = _targetVelocity;
        }

    }

    private void ChangePlayerPosition()
    {
        _targetVelocity.Set(_horizontalMove * 10,
                                     _playerRb.velocity.y);
        _playerRb.velocity = _targetVelocity;
    }

    private bool CheckGrounding()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(_groundCheck.position, Vector2.down, _rayDistance, _groundMask);
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

    private void CheckSloping()
    {
        Vector2 checkPosition = transform.position - new Vector3(0.0f, _colliderSize.y / 2);
        SlopeCheckVerical(checkPosition);
    }

    private void SlopeCheckVerical(Vector2 checkPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, _slopeForceRayLength, _groundMask);

        if (hit)
        {
            _slopeNormalPerpendecular = Vector2.Perpendicular(hit.normal).normalized;

            _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(_slopeDownAngle != _slopeDownAngleOld)
            {
                _isOnSlope = true;
            }

            _slopeDownAngleOld = _slopeDownAngle;

            //Debug.DrawRay(hit.point, _slopeNormalPerpendecular, Color.red);
            //Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheckInPlayer.position, _groundCheckRadius);
    }

    private void CheckColliderInPlayer()
    {
        bool isGround;
        isGround = Physics2D.OverlapCircle(_groundCheckInPlayer.position, _groundCheckRadius, _groundMask);

        if (isGround)
        {
            _playerRb.AddForce(transform.up * _jumpForceIfInTexture, ForceMode2D.Impulse);
        }
    }
}
