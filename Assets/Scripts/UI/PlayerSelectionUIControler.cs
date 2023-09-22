using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using EndlessRunner;

public class PlayerSelectionUIControler : MonoBehaviour
{
    public GameObject[] players;
    public string[] playerNames;
    public string[] playerDescriptions;
    public AudioClip[] playersAudioDescriptions;
    public Color[] playersColors;
    public int[] playersCost;
    public Image background;
    public Text title;
    public Text description;
    public Text points;
    public GameObject target;
    public GameObject lockScreen;

    private AudioSource[] m_audioSource;
    private Button[] buttons;

    private int currentIndex = 0;

    private void Awake()
    {
        m_audioSource = GetComponentsInChildren<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        lockScreen.SetActive(false);

        points.text = DataBase.SelectData("maxPoints") + "";
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        ChangePlayer(0);

        //home
        buttons[0].onClick.AddListener(
        () => {
            //m_audioSource[0].Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
        });

        //prev
        buttons[1].onClick.AddListener(
        () => {
            if (currentIndex <= 0)
            {
                currentIndex = playersColors.Length-1;
            }
            else
            {
                currentIndex--;
            }

            ChangePlayer(currentIndex);
        });

        //next
        buttons[2].onClick.AddListener(
        () => {
            if (currentIndex >= playersColors.Length-1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            ChangePlayer(currentIndex);
        });

        //play
        buttons[3].onClick.AddListener(
        () => {
            DataBase.InsertData("playerCharacter", currentIndex);

            MenuCanvasManager.Instance.LoadScene("Game");
        });
    }

    void OnDisable()
    {
        //home
        buttons[0].onClick.RemoveAllListeners();

        //prev
        buttons[1].onClick.RemoveAllListeners();

        //next
        buttons[2].onClick.RemoveAllListeners();

        //play
        buttons[3].onClick.RemoveAllListeners();
    }

    private void ChangePlayer(int index)
    {
        title.text = playerNames[currentIndex];
        description.text = playerDescriptions[currentIndex];
        background.color = playersColors[currentIndex];

        foreach (Transform child in target.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject m_player = Instantiate(players[currentIndex], target.transform.position, Quaternion.identity, target.transform);
        m_player.transform.localScale = new Vector3(15000, 15000, 15000);
        m_player.transform.Rotate(0,180,0);

        ChangeLayerMaskRecursively(m_player.transform, LayerMask.NameToLayer("UI"));

        //RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>("Animations/PlayerAnimatorController");
        //m_player.GetComponent<Animator>().runtimeAnimatorController = controller;
        m_player.GetComponent<Animator>().enabled = true;

        m_audioSource[1].clip = playersAudioDescriptions[currentIndex];
        m_audioSource[1].Play();

        if(DataBase.SelectData("maxPoints") >= playersCost[index])
        {
            buttons[3].GetComponentInChildren<Text>().text = "Selecionar";
            buttons[3].enabled = true;
            lockScreen.SetActive(false);
        }
        else
        {
            buttons[3].GetComponentInChildren<Text>().text = "Bloqueado";
            buttons[3].enabled = false;
            lockScreen.SetActive(true);
            lockScreen.GetComponentInChildren<Text>().text = "" + playersCost[index];
        }
    }

    private void ChangeLayerMaskRecursively(Transform parent, LayerMask newLayerMask)
    {
        foreach (Transform child in parent)
        {
            // Change LayerMask attribute of the child GameObject
            child.gameObject.layer = newLayerMask;

            // Recursive call for the child's children
            ChangeLayerMaskRecursively(child, newLayerMask);
        }
    }

}
