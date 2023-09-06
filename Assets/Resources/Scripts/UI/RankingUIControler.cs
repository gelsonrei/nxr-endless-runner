using UnityEngine;
using UnityEngine.UI;

using EndlessRunner;

public class RankingUIControler : MonoBehaviour
{
    private AudioSource m_audioSource;
    private Button[] buttons;
    private Text[] texts;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
        texts = GetComponentsInChildren<Text>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    void OnEnable()
    {
        //home
        buttons[0].onClick.AddListener(
        () =>
        {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
        });

        //check rancking        
        if (DataBase.SelectData("maxDistance") > 0)
        {
            texts[1].text = DataBase.SelectData("maxDistance") + "M";
        }
    }

    void OnDisable()
    {
        //home
        buttons[0].onClick.RemoveAllListeners();
    }
}
