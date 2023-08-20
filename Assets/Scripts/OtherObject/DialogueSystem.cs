using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue System")]
    [SerializeField] private string[] _dialogue;
    [SerializeField] private GameObject _textPanel;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private TextMeshPro _textWindow;
    [SerializeField] private float _textSpeed = 1.0f;

    private int _index = 0;
    private bool _isStartPanel = true;

    private void Start()
    {
        _textPanel.SetActive(false);
        _startPanel.SetActive(false);
    }

    private void Update()
    {
        if (_startPanel.activeSelf && _isStartPanel == true && Input.GetKeyDown(KeyCode.E))
        {
            _isStartPanel = false;
            _startPanel.SetActive(false);
            _textPanel.SetActive(true);
        }
        if (_textWindow.isActiveAndEnabled && _isStartPanel == false) 
        {
            DialogueWindow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isStartPanel = true;
        if (collision.gameObject.CompareTag("Player") && _dialogue.Length > 0)
        {
            Debug.Log("Is ative");
            _startPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Is close");
            _startPanel.SetActive(false);
            _textPanel.SetActive(false);
            _isStartPanel = true;
        }
    }

    private void DialogueWindow()
    {
        if (_index < 0)
        {
            LastText();
        }
        else 
        if (_index == 0)
        {
            _textWindow.text = "";
            StartCoroutine(nameof(TypeLine));
            _index++;
        }
        else
        if (Input.GetKeyDown(KeyCode.E) && _index <= _dialogue.Length - 1) 
        {
            StopAllCoroutines();
            _textWindow.text = "";
            StartCoroutine(nameof(TypeLine));
            _index++;
        }
        else 
        if (Input.GetKeyDown(KeyCode.E) && _index > _dialogue.Length - 1)
        {
            CloseAllPanel();
            _index = -1;
        }
    }

    private void LastText()
    {
        if (_index == -1 && _isStartPanel == false)
        {
            StopAllCoroutines();
            _index = _dialogue.Length - 1;
            _textWindow.text = "";
            StartCoroutine(nameof(TypeLine));
            _index = -2;
            Debug.Log("It's a BUG!!!");
        }
        else
        if (_index == -2 && Input.GetKeyDown(KeyCode.E) && _isStartPanel == false)
        {
            Debug.LogError("It's a BUG!!!");
            _index = -1;
            CloseAllPanel();
        }
    }    

    private void CloseAllPanel()
    {
        Debug.Log("Is close whith metod");
        _startPanel.SetActive(false);
        _textPanel.SetActive(false);
        _isStartPanel = true;
    }

    IEnumerator TypeLine()
    {
        foreach (char symbol in _dialogue[_index].ToCharArray()) 
        {
            _textWindow.text += symbol;
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
