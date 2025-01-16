using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] GameObject _chat;
    [SerializeField] GameObject _chatViewButton;
    [SerializeField] CanvasGroup _users;

    Player _player;
    bool _isSetup = false;

    class UserCreateResult
    {
        public string uuid;
        public int level;
    }

    private void Awake()
    {
        _player = FindObjectOfType<Player>();

        _chat.SetActive(false);
        _users.alpha = 0;
        _chatViewButton.SetActive(false);
    }

    private void Update()
    {
        if(!_isSetup)
        {
            _chat.SetActive(true);

            _isSetup = true;
        }
    }

    public void OpenChat()
    {
        _chatViewButton.SetActive(false);
        _chat.SetActive(true);
    }
    public void CloseChat()
    {
        _chatViewButton.SetActive(true);
        _chat.SetActive(false);
    }
}
