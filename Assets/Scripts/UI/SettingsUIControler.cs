using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsUIControler : MonoBehaviour
{
    [SerializeField]
    private AudioMixer m_audioMixer;
    private AudioSource m_audioSource;
    private Button[] buttons;
    private Slider[] sliders;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
        sliders = GetComponentsInChildren<Slider>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        //Sliders
            float soundTrackValue;
            if (m_audioMixer.GetFloat("SoundTrackVolume", out soundTrackValue))
            {
                sliders[0].value = Mathf.Pow(10f, soundTrackValue / 20f);
            }
            sliders[0].onValueChanged.AddListener(delegate { OnSliderValueChanged("SoundTrackVolume", sliders[0].value); });

            float EfxValue;
            if (m_audioMixer.GetFloat("EfxVolume", out EfxValue))
            {
                sliders[1].value = Mathf.Pow(10f, EfxValue / 20f);
            }
            sliders[1].onValueChanged.AddListener(delegate { OnSliderValueChanged("EfxVolume", sliders[1].value); });

            float UIValue;
            if (m_audioMixer.GetFloat("UIVolume", out UIValue))
            {
                sliders[2].value = Mathf.Pow(10f, UIValue / 20f);
            }
            sliders[2].onValueChanged.AddListener(delegate { OnSliderValueChanged("UIVolume", sliders[2].value); });

            float VoiceOverValue;
            if (m_audioMixer.GetFloat("VoiceOverVolume", out VoiceOverValue))
            {
                sliders[3].value = Mathf.Pow(10f, VoiceOverValue / 20f);
            }
            sliders[3].onValueChanged.AddListener(delegate { OnSliderValueChanged("VoiceOverVolume", sliders[3].value); });

        //Butons
            //home
            buttons[0].onClick.AddListener(
            () => {
                m_audioSource.Play();

                MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
            });

            //back
            buttons[1].onClick.AddListener(
            () => {
                m_audioSource.Play();

                MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
            });
    }

    void OnDisable()
    {
        //Sliders
            sliders[0].onValueChanged.RemoveAllListeners();
            sliders[1].onValueChanged.RemoveAllListeners();
            sliders[2].onValueChanged.RemoveAllListeners();
            sliders[3].onValueChanged.RemoveAllListeners();
        //Butons
            //home
            buttons[0].onClick.RemoveAllListeners();

            //back
            buttons[1].onClick.RemoveAllListeners();
    }

    private void OnSliderValueChanged(string mixerGroupName, float sliderValue)
    {
        Debug.Log(sliderValue);
        float volumeDB = 20f * Mathf.Log10(sliderValue);
        m_audioMixer.SetFloat(mixerGroupName, volumeDB);
    }

}
