using System.Collections;
using TMPro;
using UnityEngine;

namespace SlimeStrategy.Exploration.Dialogue
{
    public class TypewriterEffect : MonoBehaviour
    {
        [SerializeField] private float typewriterSpeed = 30f;
        public Coroutine Run(string textToType, TMP_Text textLabel)
        {
            return StartCoroutine(routine: TypeText(textToType,textLabel));
        }

        private IEnumerator TypeText(string textToType, TMP_Text textLabel)
        {
            textLabel.text = string.Empty;
            //yield return new WaitForSeconds(2);
            float t = 0;
            int charIndex = 0;

            while (charIndex < textToType.Length)
            {
                t += Time.deltaTime * typewriterSpeed; 
                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(value: charIndex, min: 0, max: textToType.Length);

                textLabel.text = textToType[..charIndex];

                yield return null;
            }
            textLabel.text = textToType;
        }

    }
}
