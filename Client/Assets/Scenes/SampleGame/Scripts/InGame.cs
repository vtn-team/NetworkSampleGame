using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SampleGame
{
    /// <summary>
    /// 簡単なインゲーム
    /// </summary>
    public class InGame : MonoBehaviour, IEventReceiver
    {
        [SerializeField] float _spawnInterval;
        [SerializeField] GameObject _player;
        [SerializeField] GameObject _enemy;
        [SerializeField] GameObject _comment;
        [SerializeField] GameObject _canvasRoot;

        float _timer = 0.0f;

        void Awake()
        {
            //イベントを受け取るためには登録が必要
            NetworkManager.RegisterEventReceiver(this);
        }

        public bool IsActive => true;

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _spawnInterval)
            {
                _timer -= _spawnInterval;
                GameObject.Instantiate(_enemy, new Vector3(UnityEngine.Random.Range(-20, 20), 50, -1), Quaternion.identity);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ランキング送信
            }
        }

        public void OnEventCall(EventData data)
        {
            switch (data.EventId)
            {
                //おうえん
                case 100:
                    {
                        //コメントオブジェクト生成
                        var go = GameObject.Instantiate(_comment, _canvasRoot.transform);
                        var script = go.GetComponent<CommentText>();
                        var transform = go.GetComponent<RectTransform>();
                        transform.position = new Vector3(UnityEngine.Random.Range(250, 1550), UnityEngine.Random.Range(-250, 550));
                        script.SetText(data.GetStringData("Message"));

                        //サイズ変化
                        _player.transform.localScale += new Vector3(1, 0, 0);
                    }
                    break;
            }
        }
    }
}