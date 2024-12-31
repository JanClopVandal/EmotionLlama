using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect VFXGraph;
    public Color EditorColor = Color.white; // Цвет, задаваемый в редакторе

    public void AnswerToColor(string txt)
    {
        // Регулярное выражение для поиска трёх чисел в формате (R, G, B)
        string pattern = @"\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3})\)";
        Match match = Regex.Match(txt, pattern);

        if (match.Success)
        {
            // Парсим числа из групп регулярного выражения
            if (int.TryParse(match.Groups[1].Value, out int r) &&
                int.TryParse(match.Groups[2].Value, out int g) &&
                int.TryParse(match.Groups[3].Value, out int b))
            {
                // Убеждаемся, что значения в допустимом диапазоне (0-255)
                r = Mathf.Clamp(r, 0, 255);
                g = Mathf.Clamp(g, 0, 255);
                b = Mathf.Clamp(b, 0, 255);

                // Возвращаем цвет
                ColorizeVFX(new Color(r / 255f, g / 255f, b / 255f));
            }
        }

    }
    private void ColorizeVFX(Color color)
    {
        if (VFXGraph == null)
        {
            Debug.LogError("VFXGraph is not assigned.");
            return;
        }

        // Создаем градиент из входного цвета и заданного в редакторе
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(color, 0f),
                new GradientColorKey(EditorColor, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(0.1f, 0.2f),
                new GradientAlphaKey(0.2f, 0.3f)
            }
        );

        // Применяем градиент к VFXGraph
        VFXGraph.SetGradient("ColorGradient", gradient);
    }
}
