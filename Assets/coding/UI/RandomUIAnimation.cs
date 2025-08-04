using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Assets.coding.UI
{

    //這段程式碼會掛載在UI上
    //每隔一段隨機時間，會播放上面的Animation
    public class RandomUIAnimation : MonoBehaviour
    {
        public float minTime = 1f;
        public float maxTime = 5f;

        private float storeTime = 0f;
        public float time = 0f;
        public float offset = 10f;

        private RectTransform rectTrans;

        private void Start()
        {
            StartCoroutine(PlayRotation());
        }

        private IEnumerator PlayRotation()
        {
            rectTrans = this.GetComponent<RectTransform>();

            while (true)
            {
                float randomTime = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(randomTime);

                storeTime = 0f;
                var startPos = rectTrans.anchoredPosition;
                var midPos = startPos + new Vector2(0, offset);

                while (storeTime < time)
                {
                    storeTime += Time.deltaTime;
                    var angle = 180f + storeTime / time * 360f;
                    var rotatedPos = Quaternion.Euler(0, 0, angle) * (Vector3)(midPos - startPos);
                    var newPos = (Vector2)rotatedPos + startPos;
                    this.GetComponent<RectTransform>().anchoredPosition = newPos;
                    yield return null;
                }

                this.GetComponent<RectTransform>().anchoredPosition = startPos;
            }
        }
    }
}