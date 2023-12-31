using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Nxr.FormLeads;

public class TotemMenuUIControler : MonoBehaviour
{
    private AudioSource m_audioSource;
    private Button[] buttons;

    [Header("Ranking Settings")]
    public GameObject logo;
    public GameObject title;
    public GameObject content;
    public GameObject template;

    [Header("App Settings")]
    public GameObject appMenu;
    public string LeadsScene;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
    }

    void OnEnable()
    {
        if (DatabaseHandler.instance == null)
            return;
        
        //app config
        buttons[0].onClick.AddListener(
        () => {
            appMenu.SetActive(true);
        });

        //leads
        buttons[1].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[2]);
            //MenuCanvasManager.Instance.LoadScene("Game");
        });

        //ranking
        buttons[2].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[3]);
        });

        //credits
        buttons[3].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[4]);
        });

        //ranking
        foreach (Transform child in content.transform)
        {
            if (child.name != "Header")
            {
                Destroy(child.gameObject);
            }
        }

        //db
        List<Ranking> ranking = RankingManager.Select(10);

        content.SetActive(false);
        if (ranking.Count > 0)
        {
            StartCoroutine(HideAndShowGameObjectCoroutine());

            int order = 0;
            foreach (Ranking r in ranking)
            {
                order++;

                Lead l = LeadManager.GetOne(r.leadId);

                GameObject go = GameObject.Instantiate(template, transform.position, Quaternion.identity, content.transform);

                Text[] tx = go.GetComponentsInChildren<Text>();
                tx[0].text = order + "";
                tx[1].text = l.Name;
                tx[2].text = r.MaxDistance + "";
            }
        }        
    }

    void OnDisable()
    {
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
        buttons[3].onClick.RemoveAllListeners();
    }

    private IEnumerator HideAndShowGameObjectCoroutine()
    {
        // Hide the game object after 50 seconds.
        yield return new WaitForSeconds(10f);
        content.SetActive(true);
        logo.SetActive(false);
        title.SetActive(true);

        // Wait 20 seconds.
        yield return new WaitForSeconds(20f);

        // Show the game object again.
        content.SetActive(false);
        logo.SetActive(true);
        title.SetActive(false);

        // Start the coroutine again.
        StartCoroutine(HideAndShowGameObjectCoroutine());
    }
}
