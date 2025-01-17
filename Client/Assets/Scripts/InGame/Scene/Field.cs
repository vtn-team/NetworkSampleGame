using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour, IEventReceiver
{
    [SerializeField] GameObject _chat;
    [SerializeField] GameObject _chatViewButton;
    [SerializeField] CanvasGroup _users;
    [SerializeField] TMP_InputField _inputText;
    [SerializeField] TMP_Text _chatText;
    [SerializeField] TMP_Text _userList;

    public bool IsActive => gameObject.activeSelf;

    Queue<string> chatLog = new Queue<string>();

    private void Awake()
    {
        chatLog.Clear();

        _chatViewButton.SetActive(true);
        _chat.SetActive(false);
        _users.alpha = 0;

        _chatText.text = "";
        NetworkManager.RegisterEventReceiver(this);
    }

    public void SendChat()
    {
        if (_inputText.text == "") return;

        EventData data = new EventData((int)EventDefine.Message);
        data.DataPack("UserName", NetworkManager.SystemSave.UserName);  //データを格納する
        data.DataPack("Message", _inputText.text);                      //データを格納する
        NetworkManager.SendEvent(data);
        _inputText.text = "";
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

    public void OnEventCall(EventData data)
    {
        switch((EventDefine)data.EventId)
        {
            case EventDefine.Join:
                {
                    NetworkPlayer.CreatePlayer(data);
                }
                break;

            case EventDefine.Message:
                {
                    chatLog.Enqueue(data.GetStringData("UserName") + ": " + data.GetStringData("Message"));
                    while(chatLog.Count > 15)
                    {
                        chatLog.Dequeue();
                    }
                    _chatText.text = string.Join("\n", chatLog);
                }
                break;
        }
    }
}
