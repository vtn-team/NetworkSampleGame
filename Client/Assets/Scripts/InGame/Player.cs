using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEventReceiver
{
    [SerializeField] float _speed = 1.0f;
    Rigidbody _rb;

    Vector3 _pow;
    bool _isSetup = false;
    float _time = 0.0f;

    public bool IsActive => true;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        NetworkManager.RegisterEventReceiver(this);
    }

    void Update()
    {
        if (!_isSetup)
        {
            if (NetworkManager.IsSetup)
            {
                NetworkManager.JoinNetwork();
                _isSetup = true;
            }
        }

        //移動方向設定
        float yoko = Input.GetAxis("Horizontal");
        float tate = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(yoko, 0, tate);

        //速度設定
        Vector3 move = input * _speed;

        _rb.velocity = move + _pow;
        _pow *= 0.9f;

        //
        if (_isSetup && NetworkManager.SystemSave.IsMMONetwork)
        {
            _time += Time.deltaTime;
            if (_time > 1.0f)
            {
                EventData data = new EventData((int)EventDefine.Move);
                data.DataPack("UserId", NetworkManager.GetUserId());
                data.DataPack("PosX", this.transform.position.x);
                data.DataPack("PosZ", this.transform.position.z);
                NetworkManager.SendEvent(data);
                _time = 0.0f;
            }
        }
    }

    //ここで
    void DoAction(string actionStr)
    {
        switch(actionStr)
        {
            case "left":
            case "Left":
            case "LEFT":
            case "L":
                _pow += new Vector3(-_speed * 10, 0, 0);
                break;

            case "right":
            case "Right":
            case "RIGHT":
            case "R":
                _pow += new Vector3(_speed * 10, 0, 0);
                break;

            case "up":
            case "Up":
            case "UP":
            case "U":
                _pow += new Vector3(0, 0, _speed * 10);
                break;

            case "down":
            case "Down":
            case "DOWN":
            case "D":
                _pow += new Vector3(0, 0, -_speed * 10);
                break;
        }
    }

    public void OnEventCall(EventData data)
    {
        switch((EventDefine)data.EventId)
        {
            case EventDefine.Message:
                {
                    var str = data.GetStringData("Message");
                    DoAction(str);
                }
                break;
        }
    }
}