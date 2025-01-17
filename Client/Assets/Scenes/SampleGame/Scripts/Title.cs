using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SampleGame
{
    /// <summary>
    /// タイトル
    /// </summary>
    public class Title : MonoBehaviour
    {
        private void Start()
        {
            //ランキング取得

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("InGame");
            }
        }
    }
}