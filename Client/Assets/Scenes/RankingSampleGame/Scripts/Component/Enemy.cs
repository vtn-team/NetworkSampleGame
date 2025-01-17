using UnityEngine;
using UnityEngine.UI;

namespace SampleGame
{
    /// <summary>
    /// 落下するボール
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        void Update()
        {
            if(this.transform.position.y < -20)
            {
                ScoreManager.AddScore(-100);

                //消える
                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ScoreManager.AddScore(500);
                Destroy(this.gameObject);
            }
        }
    }
}