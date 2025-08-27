using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoubleSliderHandleDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public TextMeshProUGUI music_number_txt;
    public TextMeshProUGUI vfx_number_txt;

    public Slider sliderA;
    public RectTransform handleA;
    public string playerPrefKeyA = "SliderAValue";

    public Slider sliderB;
    public RectTransform handleB;
    public string playerPrefKeyB = "SliderBValue";

    private bool draggingA = false;
    private bool draggingB = false;

    // Helper: Checks if pointer is on this handle (for both handles)
    private bool IsPointerOverHandle(RectTransform handle, PointerEventData eventData)
    {
        Vector2 localMousePos;
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handle,
            eventData.position,
            eventData.pressEventCamera,
            out localMousePos
        ) && handle.rect.Contains(localMousePos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPointerOverHandle(handleA, eventData)) {
            draggingA = true;
            UpdateSliderValue(sliderA, eventData, playerPrefKeyA);
        }
        else if (IsPointerOverHandle(handleB, eventData)) {
            draggingB = true;
            UpdateSliderValue(sliderB, eventData, playerPrefKeyB);
        }
    }
    public void OnBeginDrag(PointerEventData eventData) {
        // Just mark as dragging, in case needed for DRAG logic
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (draggingA) {
            UpdateSliderValue(sliderA, eventData, playerPrefKeyA);
        }
        else if (draggingB) {
            UpdateSliderValue(sliderB, eventData, playerPrefKeyB);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingA = false;
        draggingB = false;
    }

    private void UpdateSliderValue(Slider slider, PointerEventData eventData, string playerPrefKey)
    {
        RectTransform fillArea = slider.fillRect;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(fillArea,
                eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float pct = Mathf.InverseLerp(
                fillArea.rect.xMin,
                fillArea.rect.xMax,
                localPoint.x
            );
            float value = Mathf.Lerp(slider.minValue, slider.maxValue, pct);
            slider.value = value;
            // Save new value
            PlayerPrefs.SetFloat(playerPrefKey, value);
            PlayerPrefs.Save();
        }
    }

    // Optional: On Start, load previous values
    private void Start()
    {
        if (PlayerPrefs.HasKey(playerPrefKeyA)) {
            sliderA.value = PlayerPrefs.GetFloat(playerPrefKeyA);
        }
        if (PlayerPrefs.HasKey(playerPrefKeyB)) {
            sliderB.value = PlayerPrefs.GetFloat(playerPrefKeyB);
        }
    }
}
