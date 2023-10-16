using UnityEngine;
using UnityEngine.UI;

using EndlessRunner;
using System.Collections.Generic;

public class TotemRankingUIControler : MonoBehaviour
{
    private AudioSource m_audioSource;
    private Button[] buttons;
    public GameObject content;
    public GameObject template;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
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

        buttons[1].onClick.AddListener(
        () =>
        {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
        });

        //check rancking      
        foreach (Transform child in content.transform)
        {
            if (child.name != "Header")
            {
                Destroy(child.gameObject);
            }
        }

        //db
        TableManager.Init();
        List<Ranking> rancking = RankingManager.Select();
        int order = 0;
        foreach (Ranking r in rancking)
        {
            order++;

            Lead l = LeadManager.GetOne(r.Cpf);

            GameObject go = GameObject.Instantiate(template, transform.position, Quaternion.identity, content.transform);

            Text[] tx = go.GetComponentsInChildren<Text>();
            tx[0].text = order + "";
            tx[1].text = l.Name;
            tx[2].text = r.MaxDistance + "";

            content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, 167*order);
        }
    }

    void OnDisable()
    {
        //home
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
    }
}