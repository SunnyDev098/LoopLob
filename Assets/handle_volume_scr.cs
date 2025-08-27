using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HandleDragSaveWithSFX : MonoBehaviour,  IDragHandler, IEndDragHandler
{

    public TextMeshProUGUI text;
    public Slider mySlider;
    public string playerPrefsKey ;
    public AudioSource sfxSource;         // Assign in Inspector
    public AudioClip sfx;                 // Assign in Inspector
    private float lastPlayedValue;
    private float sfxlastPlayedValue;
    private float sfx_volume;

    private const float VALUE_STEP = 0.01f;
    private void OnEnable()
    {
        mySlider.onValueChanged.AddListener(delegate
        {


            text.text = ((int)(mySlider.value *100) )  .ToString();


        });
    }
    void Start()
    {


        if (PlayerPrefs.HasKey(playerPrefsKey))
            mySlider.value = PlayerPrefs.GetFloat(playerPrefsKey);
        Debug.Log(PlayerPrefs.GetFloat(playerPrefsKey));
        lastPlayedValue = mySlider.value;
      //  mySlider.onValueChanged.AddListener(UpdateSlider); // CORRECT
    }

    private void Update()
    {
        if (Mathf.Abs(mySlider.value - lastPlayedValue) >= 0.01f)
        {
            lastPlayedValue = mySlider.value;


            //   game_manager_scr.vfx_volume = (int)(mySlider.value * 100);
            PlayerPrefs.SetFloat(playerPrefsKey, mySlider.value);
            PlayerPrefs.Save();

            if (playerPrefsKey == "music_volume")
            {
                Debug.Log("dawadw");
                MusicPlayer.SetVolume(mySlider.value);

            }

            else if (playerPrefsKey == "sfx_volume")
            {
                sfx_volume = mySlider.value;


                if (Mathf.Abs(mySlider.value - sfxlastPlayedValue) >= 0.05f)
                {

                    sfxlastPlayedValue = mySlider.value;
                    PlaySFX();

                   // Debug.Log("vfx");
                }
          
            }

        }


       

     
    }

    public void OnDrag(PointerEventData eventData)
    {
       // UpdateSlider(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
  
    }
    /*
    private void UpdateSlider(PointerEventData eventData)
    {
        RectTransform fillArea = mySlider.fillRect;
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            fillArea, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float pct = Mathf.InverseLerp(fillArea.rect.xMin, fillArea.rect.xMax, localPoint.x);
            float value = Mathf.Lerp(mySlider.minValue, mySlider.maxValue, pct);

            // 🚨 Only play SFX when a 0.01 change occurs
            if (Mathf.Abs(value - lastPlayedValue) >= 0.057f)
            {
                PlaySFX();
                lastPlayedValue = value;

            
                game_manager_scr.vfx_volume = (int)(mySlider.value * 100);
                PlayerPrefs.SetFloat(playerPrefsKey, mySlider.value);
                PlayerPrefs.Save();
            }

            mySlider.value = value;
        }
    }
    */

    private void PlaySFX()
    {
        if (sfxSource != null && sfx != null)
        {

            Debug.Log(sfx_volume);
            sfxSource.volume = Mathf.Clamp01(sfx_volume); // assuming vfx_volume is 0-100
            sfxSource.PlayOneShot(sfx, 1);
        }
    }
}
