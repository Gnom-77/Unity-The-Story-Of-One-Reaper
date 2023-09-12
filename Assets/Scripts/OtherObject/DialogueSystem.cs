using Codice.Client.BaseCommands.Merge.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue System")]
    [SerializeField] private bool _isSkipped = true;
    [SerializeField] private byte[] _dialogQueue;
    [SerializeField] private string[] _dialogue;
    [SerializeField] private GameObject[] _textPanel;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _playerInDialogue;
    [SerializeField] private TextMeshPro[] _textWindow;
    [SerializeField] private float _textSpeed = 1.0f;

    private int _index = 0;
    private byte _numberOfCharacter;
    private bool _isStartPanel = true;
    private bool _triggerIsActive = false;


    private void Start()
    {
        foreach (GameObject text in _textPanel) 
        {
            text.SetActive(false);
        }
        _startPanel.SetActive(false);
        _playerInDialogue.SetActive(false);
    }

    private void Update()
    {
        if (_isStartPanel && Input.GetKeyDown(KeyCode.E) && _triggerIsActive)
        {
            CloseStartPanelAndOpenTextPanel();
        }
        if (_isStartPanel == false && _triggerIsActive) 
        {
            DialogueWindow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _dialogue.Length > 0 && _index == 0 && _isSkipped)
        {
            _isStartPanel = true;
            Debug.Log($"Trigger is ative {_triggerIsActive}");
            _startPanel.SetActive(true);
            _triggerIsActive = true;
        }
        else if (collision.gameObject.CompareTag("Player") && _dialogue.Length > 0 && _index == 0 && !_isSkipped)
        {
            Debug.Log($"Trigger is ative {_triggerIsActive}");
            CloseStartPanelAndOpenTextPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_index == _dialogue.Length - 1)
        {
            Debug.Log($"Trigger is ative {_triggerIsActive}");
            _triggerIsActive = false;
            CloseAllPanels();
        }
    }

    private void DialogueWindow()
    {
        if (_index == 0)
        {
            _numberOfCharacter = _dialogQueue[_index];
            CloseUnusedPanelsAndOpenUsedPanel();
            _textWindow[_numberOfCharacter].text = null;
            StartCoroutine(nameof(TypeLine));
            _index++;
        }
        else if (Input.GetKeyDown(KeyCode.E) && _index <= _dialogue.Length - 1 && _index != 0)
        {
            StopAllCoroutines();
            _numberOfCharacter = _dialogQueue[_index];
            CloseUnusedPanelsAndOpenUsedPanel();
            _textWindow[_numberOfCharacter].text = null;
            StartCoroutine(nameof(TypeLine));
            _index++;
        }
        else if (Input.GetKeyDown(KeyCode.E) && _index > _dialogue.Length - 1)
        {
            CloseAllPanels();
        }
    }

    private void CloseUnusedPanelsAndOpenUsedPanel()
    {
        foreach (GameObject panel in _textPanel)
        {

            panel.SetActive(false);
        }
        for (int i = 0; i < _textPanel.Length; i++)
        {
            if (_numberOfCharacter != i)
            {
                _textPanel[i].SetActive(false);
            }
            else
            {
                _textPanel[i].SetActive(true);
                return;
            }
        }
    }

    private void CloseAllPanels()
    {
        _triggerIsActive = false;
        _startPanel.SetActive(false);
        _playerInDialogue.SetActive(false);
        foreach (GameObject panel in _textPanel)
        {
            panel.SetActive(false);
        }
        _isStartPanel = true;
    }

    private void CloseStartPanelAndOpenTextPanel()
    {
        _triggerIsActive = true;
        _isStartPanel = false;
        _startPanel.SetActive(false);
        _playerInDialogue.SetActive(true);
    }




    IEnumerator TypeLine()
    {
        foreach (char symbol in _dialogue[_index].ToCharArray()) 
        {
            _textWindow[_numberOfCharacter].text += symbol;
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
