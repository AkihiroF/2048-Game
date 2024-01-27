using TMPro;
using UnityEngine;

namespace BlockSystem.View
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI blockText;
        [SerializeField] private SpriteRenderer blockBackground;

        // Базовий колір, від якого буде починатися градієнт
        [SerializeField] private Color baseColor = Color.green; 
        [SerializeField] private Color startedColor = Color.white;

        public void SetNumber(int number)
        {
            if(number == 0)
            {
                blockText.text = string.Empty;
                blockBackground.color = startedColor;
                return;
            }

            blockText.text = number.ToString();
            UpdateColor(number);
        }

        private void UpdateColor(int number)
        {
            // Преобразування числа в зміщення відтінку
            float hueShift = Mathf.Log(number, 2) / 10; 
            // Лінійна інтерполяція між базовим кольором та чорним
            Color newColor = Color.Lerp(baseColor, Color.black, hueShift); 
            blockBackground.color = newColor;
        }
    }
}