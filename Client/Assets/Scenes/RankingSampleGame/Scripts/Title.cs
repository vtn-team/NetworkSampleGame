using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SampleGame
{
    /// <summary>
    /// タイトル
    /// </summary>
    public class Title : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _ranking;
        private void Start()
        {
            ScoreManager.Reset();

            //ランキング取得
            UniTask.RunOnThreadPool(async () =>
            {
                var result = await NetworkManager.GetRanking();

                await UniTask.SwitchToMainThread();

                string text = "";
                int rank = 1;
                foreach(var rd in result.RankingData)
                {
                    text += rank + " : " + rd.UserName + " - Score:" + rd.Score + "\n";
                    rank++;
                }
                _ranking.text = text;
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