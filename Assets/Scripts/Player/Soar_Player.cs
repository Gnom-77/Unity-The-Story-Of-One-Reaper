using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soar_Player : MonoBehaviour
{
    [SerializeField] private float _soarGravity = 1f;
    private float _mainGravity;
    private Rigidbody2D _player_rb;

    private void Start()
    {
        _player_rb = GetComponent<Rigidbody2D>();
        _mainGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _player_rb.velocity.y < 0)
        {
            _player_rb.gravityScale = _soarGravity;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _player_rb.gravityScale = _mainGravity;
        }
    }
}
