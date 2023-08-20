using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Soul_Diary : MonoBehaviour
{
    [SerializeField] private GameObject _soul_Diary;
    private bool _isOpen = false;
    private void Start()
    {
        _soul_Diary.SetActive(false);
        _isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_isOpen)
        {
            _soul_Diary.SetActive(true);
            _isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && _isOpen)
        {
            _soul_Diary.SetActive(false);
            _isOpen = false;
        }
    }
}
