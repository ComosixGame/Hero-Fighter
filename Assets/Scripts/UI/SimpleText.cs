using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class SimpleText : MonoBehaviour
{
    [Header("Rotation")]
        public bool Rotation = true;
        [Range(-10, 10), Tooltip("Value in Hz (revolutions per second).")]
        public float RotationSpeed = 1;
        public AnimationCurve RotationAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Rainbow")]
        public bool Rainbow = true;
        [Range(-10, 10), Tooltip("Value in Hz (revolutions per second).")]
        public float RainbowSpeed = 0.5f;
        [Range(0, 1)]
        public float RainbowSaturation = 1f;
        public AnimationCurve RainbowAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Options")]
        public bool RandomPeriod = true;
        
        private Image _image;
        private float _period;
        private TMP_Text _text;

        public void Start()
        {
            _image = GetComponent<Image>();
            _period = RandomPeriod ? Random.Range(0f, 1f) : 0;
            _text = GetComponent<TMP_Text>();
        }

        public void Update()
        {
            if (Rotation)
            {
                transform.localEulerAngles = new Vector3(0, 0, -360 * RotationAnimationCurve.Evaluate((RotationSpeed * Time.time + _period) % 1));
            }

            if (Rainbow)
            {
                // _image.color = Color.HSVToRGB(RainbowAnimationCurve.Evaluate((RainbowSpeed * Time.time + _period) % 1), RainbowSaturation, 1);
                _text.color = Color.HSVToRGB(RainbowAnimationCurve.Evaluate((RainbowSpeed * Time.time + _period) % 1), RainbowSaturation, 1);
            }
        }
}
