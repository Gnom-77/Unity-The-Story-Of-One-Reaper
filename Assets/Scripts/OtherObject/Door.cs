using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPanel;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _newPlayerPosition;

    private bool _doorIsActive = false;

    private void Start()
    {
        _buttonPanel.SetActive(false);
    }

    private void Update()
    {
        if (_doorIsActive && Input.GetKeyDown(KeyCode.E)) 
        {
            _player.transform.position = _newPlayerPosition.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _doorIsActive = true;
            _buttonPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _doorIsActive = false;
            _buttonPanel.SetActive(false);
        }
    }
}
