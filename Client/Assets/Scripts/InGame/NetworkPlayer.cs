using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour, IEventReceiver
{
    public bool IsActive => gameObject.activeSelf;

    bool _isSetup = false;
    float _eraseTimer = 0.0f;
    string _userId;
    Vector3 _targetPosition = Vector3.zero;

    private void Update()
    {
        if(!_isSetup)
        {
            NetworkManager.RegisterEventReceiver(this);
            _isSetup = true;
        }

        _eraseTimer += Time.deltaTime;
        if (_eraseTimer > 5.0f) return;

        Vector3 moveVec = _targetPosition - this.transform.position;
        if (moveVec.magnitude < 2.0f) return;

        moveVec.Normalize();
        this.transform.position += moveVec * Time.deltaTime;
    }

    public void Setup(string userId)
    {
        _userId = userId;
    }

    public void OnEventCall(EventData data)
    {
        switch((EventDefine)data.EventId)
        {
            case EventDefine.Move:
                if (_userId != data.GetStringData("UserId")) break;

                _targetPosition.x = data.GetFloatData("PosX");
                _targetPosition.z = data.GetFloatData("PosZ");
                _eraseTimer = 0.0f;
                break;
        }
    }

    /// <summary>
    /// 参加したプレイヤーの生成
    /// </summary>
    /// <param name="join"></param>
    /// <returns></returns>
    static public NetworkPlayer CreatePlayer(EventData join)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/NetworkPlayer");
        var other = GameObject.Instantiate(prefab);
        var script = other.GetComponent<NetworkPlayer>();
        script.Setup(join.GetStringData("UserId"));
        return script;
    }
}