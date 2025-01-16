using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 1.0f;
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //移動方向設定
        float yoko = Input.GetAxis("horizontal");
        float tate = Input.GetAxis("vertical");
        Vector3 input = new Vector3(yoko, 0, tate);

//        return (Camera.main.CameraForward2D * input.z + player.CameraRight2D * input.x).normalized;

        //キャラの向いている方向を変える
//        RotateBody(move);

        //速度設定
        Vector3 move = input * _speed;

        _rb.velocity = move;
    }
}