using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SampleGame
{
    /// <summary>
    /// リザルト
    /// </summary>
    public class Result : MonoBehaviour
    {
        void Start()
        {
            ScoreManager.Reset();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}