using UnityEngine;
using TMPro;

[ExecuteAlways] // Works in edit mode too
public class TextWithIcon : MonoBehaviour
{
    public RectTransform icon;
    public TMP_Text textComponent;
    public float spacing = 5f; // Space between icon and text

    void Update()
    {
        if (icon == null || textComponent == null) return;

        // Get the preferred width of the text
        float textWidth = textComponent.preferredWidth;

        // Position the text
        textComponent.rectTransform.anchoredPosition =
            new Vector2(icon.rect.width + spacing, 0);

        // Adjust the parent's width to contain both
        RectTransform parentRT = GetComponent<RectTransform>();
        parentRT.sizeDelta = new Vector2(
            icon.rect.width + spacing + textWidth,
            Mathf.Max(icon.rect.height, textComponent.preferredHeight)
        );
    }
}