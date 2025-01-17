using UnityEngine;
using UnityEngine.UI;

namespace SampleGame
{
    /// <summary>
    /// スコア表示
    /// </summary>
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] Text _scoreText;

        private void Update()
        {
            _scoreText.text = ScoreManager.Score.ToString();
        }
    }
}