using Cysharp.Threading.Tasks;
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
            UniTask.RunOnThreadPool(async () =>
            {
                var result = await NetworkManager.GetRanking();

                await UniTask.SwitchToMainThread();
            }).Forget();
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