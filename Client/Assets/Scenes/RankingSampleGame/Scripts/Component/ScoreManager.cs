using UnityEngine;
using UnityEngine.UI;

namespace SampleGame
{
    /// <summary>
    /// スコア
    /// </summary>
    public class ScoreManager
    {
        static ScoreManager _instance = new ScoreManager();
        ScoreManager() { }

        int _score = 0;

        static public int Score => _instance._score;

        static public void Reset()
        {
            _instance._score = 0;
        }
        static public void AddScore(int add)
        {
            _instance._score += add;
        }
    }
}