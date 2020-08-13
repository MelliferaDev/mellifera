using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class ImageColorLerp : MonoBehaviour
    {
        [SerializeField] private Color color1;
        [SerializeField] private Color color2;
        [SerializeField] private float lerpSpeed;

        private Image img;
    
        void Start()
        {
            img = GetComponent<Image>();
        }

        void Update()
        {
            float step = (Mathf.Sin(Time.time * lerpSpeed) + 1f) / 2f;
            img.color = Color.Lerp(color1, color2, step);
        }
    }
}
