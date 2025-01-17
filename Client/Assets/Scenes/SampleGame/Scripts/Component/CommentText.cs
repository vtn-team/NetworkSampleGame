using UnityEngine;
using UnityEngine.UI;

namespace SampleGame
{
    /// <summary>
    /// コメント
    /// </summary>
    public class CommentText : MonoBehaviour
    {
        [SerializeField] float _deathTime;
        [SerializeField] Text _text;

        float _timer = 0.0f;

        public void SetText(string text)
        {
            _text.text = text;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if(_timer > _deathTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}