using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using EndlessRunner;

public class GameCanvasManager : MonoBehaviour
{
    private AudioSource m_audioSource;
    private Image m_image;

    [Header("Game HUD")]
    public Text pointsLabel;
    public Text distanceLabel;
    public GameObject[] lifesImage;
    public Button pauseButton;
    public GameObject powerUpImage;
    public Sprite[] powerUpSprites;
    public GameObject specialImage;
    public Sprite[] specialUpSprites;
    public GameObject specialPopUp;
    public Text recordLabel;
    public Text newRecordLabel;

    [Header("Game UIS")]
    public GameObject pauseUI;
    public GameObject startUI;
    public GameObject loseUI;
    public GameObject countDownUI;
    public GameObject loaderScreen;
    public GameObject recordUI;

    private float currentDistance;
    private float currentPoints;
    
    public bool newRecord = false;
    private int mr = 0;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_image = GetComponent<Image>();
    }

    void Start()
    {
        HidePauseUI();
        HideLoseUI();
        ShowStartUI();

        HideEspecial();
        HidePowerUp();
        
        
        UpdateLifes(GameManager.Instance.player.GetComponent<PlayerControl>().lifes);

        // --- //

        TableManager.Init();
        List<Ranking> rancking = RankingManager.Select(1);
        mr = rancking.Count > 0 ? rancking[0].MaxDistance : 0;
        
        PopulateRecord();
    }

    void Update()
    {
        
    }

    private void ShowOverlay()
    {
        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 0.5f);
    }

    private void HideOverlay()
    {
        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 0.0f);
    }

    /*
    * HUD
    */

    //Poinst
        public void PopulatePoints(int points)
        {
            if (pointsLabel)
            {
                currentPoints = points;
                pointsLabel.text = points.ToString();
            }
        }

        public void PopulateRecord()
        {
            if (recordLabel)
            {
                if (newRecord)
                {
                    recordLabel.text = currentDistance.ToString() + "M";
                } 
                else 
                {
                    recordLabel.text = mr + "M";
                    //recordLabel.text = DataBase.SelectData("maxDistance").ToString() + "M";
                }
            }
        }

        //Distance
        public void PopulateDistance(int distance)
        {
            if(distanceLabel)
            {
                currentDistance = distance;
                distanceLabel.text = distance.ToString() + " M";
            }
            
            if (currentDistance > mr)
            {
                newRecord = true;
                
                PopulateRecord();
            }
        }

        //PauseButton
        private void EnablePauseButton()
        {
            pauseButton.enabled = true;
            pauseButton.interactable = true;

            pauseButton.onClick.AddListener(ShowPauseUI);
        }

        private void DisablePauseButton()
        {
            pauseButton.enabled = false;
            pauseButton.interactable = false;

            pauseButton.onClick.RemoveAllListeners();
        }

        //power up
        public void ShowPowerUp()
        {
            powerUpImage.SetActive(true);
        }

        public void HidePowerUp()
        {
            powerUpImage.SetActive(false);
        }

        public void AnimatePowerUp(int index)
        {
            powerUpImage.GetComponent<AudioSource>().Play();

            Image image = powerUpImage.GetComponent<Image>();
            image.sprite = powerUpSprites[index];
        }

        //special
        public void ShowEspecial()
        {
            specialImage.SetActive(true);
        }

        public void HideEspecial()
        {
            specialImage.SetActive(false);
        }

        public void AnimateEspecial(int index)
        {
            specialImage.GetComponent<AudioSource>().Play();

            Image image = specialImage.GetComponent<Image>();
            image.sprite = specialUpSprites[index];
        }

        public void ShowSpecialPopUp()
        {
            specialPopUp.SetActive(true);
        }

        public void HideSpecialPopUp()
        {
            specialPopUp.SetActive(false);
        }

        //lifes
        public void UpdateLifes(int life)
        {
            for (int i = 0; i < lifesImage.Length; i++)
            {
                //lifesImage[i].SetActive(false);
                lifesImage[i].GetComponent<Image>().color = new Color(lifesImage[i].GetComponent<Image>().color.r, lifesImage[i].GetComponent<Image>().color.g, lifesImage[i].GetComponent<Image>().color.b, 0.5f);
            }

            for (int i=0; i < life; i++)
            {
                //lifesImage[i].SetActive(true);
                lifesImage[i].GetComponent<Image>().color = new Color(lifesImage[i].GetComponent<Image>().color.r, lifesImage[i].GetComponent<Image>().color.g, lifesImage[i].GetComponent<Image>().color.b, 1.0f);
            }
        }

    /*
    * UIS
    */

        //Pause UI
        public void ShowPauseUI()
        {
            ShowOverlay();
            DisablePauseButton();

            pauseUI.SetActive(true);

            GameManager.Instance.PauseGame();

            Button[] buttons = pauseUI.GetComponentsInChildren<Button>();

            buttons[0].onClick.AddListener(
            () => {
                HidePauseUI();
            });

            buttons[1].onClick.AddListener(
            () => {
                //LoadScene("MobileMenu");
                LoadScene("TotemMenu");
            });
        }

        public void HidePauseUI()
        {
            HideOverlay();
            EnablePauseButton();

            pauseUI.SetActive(false);

            GameManager.Instance.ResumeGame();

            Button[] buttons = pauseUI.GetComponentsInChildren<Button>();

            foreach (Button b in buttons)
            {
                b.onClick.RemoveAllListeners();
            }
        }

        //StartUI
        public void ShowStartUI()
        {
            ShowOverlay();
            DisablePauseButton();

            startUI.SetActive(true);

            Button[] buttons = startUI.GetComponentsInChildren<Button>();

            buttons[0].onClick.AddListener(
            () => {
                ShowCountDown();
                
                HideStartUI();
            });
        }

        public void HideStartUI()
        {
            HideOverlay();
            EnablePauseButton();

            startUI.SetActive(false);

            Button[] buttons = startUI.GetComponentsInChildren<Button>();

            foreach (Button b in buttons)
            {
                b.onClick.RemoveAllListeners();
            }
        }
        
        public void ShowRecordUI(int time)
        {
            newRecordLabel.text = currentDistance.ToString() + " metros";
            recordUI.SetActive(true);
            
            Invoke(nameof(HideRecordUI), time);
        }
        
        public void HideRecordUI()
        {
            recordUI.SetActive(false);
            ShowLoseUI();
        }

        //Lose UI
        public void ShowLoseUI()
        {
            if (!loseUI.active)
            {
                ShowOverlay();
                DisablePauseButton();

                loseUI.SetActive(true);

                //save DB
                if ( currentDistance > DataBase.SelectData("maxDistance") )
                {
                    DataBase.InsertData("maxDistance", currentDistance);
                }

                float maxPoints = DataBase.SelectData("maxPoints");
                DataBase.InsertData("maxPoints", maxPoints + currentPoints);

                TableManager.Init();
                RankingManager.Create( DataBase.SelectSData("playerSession"), (int) currentDistance, (int) currentPoints );

                // ---- //

                Text[] texts = loseUI.GetComponentsInChildren<Text>();
                texts[0].text = "Você alcançou " + currentDistance + "M";

                Button[] buttons = loseUI.GetComponentsInChildren<Button>();
                buttons[0].onClick.AddListener(
                () => {
                    Scene scene = SceneManager.GetActiveScene();
                    LoadScene(scene.name);
                });

                buttons[1].onClick.AddListener(
                () => {
                    //LoadScene("MobileMenu");
                    LoadScene("TotemMenu");
                });
            }
        }

        public void HideLoseUI()
        {
            HideOverlay();
            EnablePauseButton();

            loseUI.SetActive(false);

            Button[] buttons = loseUI.GetComponentsInChildren<Button>();

            foreach (Button b in buttons)
            {
                b.onClick.RemoveAllListeners();
            }
        }

        //COUNTDOWN
        public void ShowCountDown()
        {
            DisablePauseButton();

            countDownUI.SetActive(true);

            StartCoroutine(UpdateCountDown());
        }

        public IEnumerator UpdateCountDown()
        {
            for (int i=3; i > 0; i--)
            {
                countDownUI.GetComponent<AudioSource>().Play();
                countDownUI.GetComponentInChildren<Text>().text = "" + i;
                
                yield return new WaitForSeconds(1);
            }

            HideCountDown();
        }

        public void HideCountDown()
        {
            EnablePauseButton();

            countDownUI.SetActive(false);

            GameManager.Instance.player.GetComponent<PlayerControl>().OnStart();
        }

        //Load Scene
        public void LoadScene(string sceneName)
        {
            GameManager.Instance.ResumeGame();
            
            loaderScreen.SetActive(true);
            loaderScreen.GetComponent<SceneManagement>().LoadScene(sceneName);
        }
}
