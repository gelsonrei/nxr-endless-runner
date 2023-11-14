using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using EndlessRunner;

public class LeadsUIControler : MonoBehaviour
{
    public Slider slider;
    public Button nextButton;
    public Button prevButton;
    public Toggle lgpd;
    public GameObject[] forms;

    public GameObject lgpdPanel;

    private bool isValid = false;
    private int idx = 0;

    void Start()
    {
        lgpdPanel.SetActive(false);
        lgpd.GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            lgpdPanel.SetActive(true);
        });

        for (int i = 0; i < forms.Length; i++)
        {
            if (forms[i].activeSelf)
            {
                forms[i].SetActive(false);
            }
        }

        if (forms.Length > 0)
        {
            forms[0].SetActive(true);
            
            slider.value = 1;
            slider.minValue = 1;
            slider.maxValue = forms.Length;

            idx = 0;
            int c = idx + 1;
            slider.GetComponentInChildren<Text>().text = "PASSO " + c + " DE " + forms.Length;
        }

        nextButton.interactable = false;

        UpdateListners();
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        for (int i = 0; i < forms.Length; i++)
        {
            foreach (InputField field in forms[i].GetComponentsInChildren<InputField>(true) )
            {
                field.onValueChange.AddListener(delegate
                {
                    CheckValidateForm();
                    UpdateListners();
                });
            }
        }
        
        lgpd.onValueChanged.AddListener(delegate
        {
            CheckValidateForm();
        });
    }

    void OnDisable()
    {
        for (int i = 0; i < forms.Length; i++)
        {
            foreach (InputField field in forms[i].GetComponentsInChildren<InputField>(true) )
            {
                field.onValueChange.RemoveAllListeners();
            }
        }

        lgpd.onValueChanged.RemoveAllListeners();
    }

    private void CheckValidateForm()
    {
        foreach (InputField field in forms[idx].GetComponentsInChildren<InputField>(true))
        {
            if (field.GetComponent<FieldsValidator>().GetIsFieldValid() == false)
            {
                isValid = false;
                break;
            }
            else
            {
                isValid = true;
            }
        }

        UpdateListners();
    }

    private void UpdateListners()
    {
        //prev button
        if(idx == 0)
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(delegate
            {
                Debug.Log("goto menu");

                MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
            });
        }
        else
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(delegate
            {
                Debug.Log("prev screen");

                PrevScreen();
            });
        }

        if (isValid == true)
        {
            if (idx == forms.Length-1)
            {
                if (lgpd.isOn)
                {
                    ColorBlock bt_colors = lgpd.GetComponentInChildren<Button>().colors;
                    bt_colors.normalColor = new Color32(0, 255, 0, 0); 
                    lgpd.GetComponentInChildren<Button>().colors = bt_colors;

                    nextButton.interactable = true;

                    nextButton.onClick.RemoveAllListeners();
                    nextButton.onClick.AddListener(delegate
                    {
                        Debug.Log("Submit Data");

                        List<String> formData = new List<String>();
                        for (int f = 0; f < forms.Length; f++)
                        {
                            foreach (InputField field in forms[f].GetComponentsInChildren<InputField>(true))
                            {
                                formData.Add(field.text);
                            }
                        }

                        TableManager.Init();

                        string mcpf;
                        //if (formData[4] == "000.000.000-00")
                        if (formData[1] == "000.000.000-00")
                        {
                            int mv = LeadManager.Select().Count + 1;
                            mcpf = "no-cpf-" + mv;
                        }
                        else
                        {
                            //mcpf = formData[4];
                            mcpf = formData[1];
                        }

                        //LeadManager.CreateOrUpdate(formData[0],formData[1],formData[2],mcpf,formData[3], ActivationManager.GetLength());
                        LeadManager.CreateOrUpdate(formData[0],"","",mcpf,"", ActivationManager.GetLength());

                        DataBase.InsertData("playerCharacter", 0);
                        DataBase.InsertData("playerSession", mcpf);

                        MenuCanvasManager.Instance.LoadScene("Game");
                    });
                }
                else
                {
                    ColorBlock bt_colors = lgpd.GetComponentInChildren<Button>().colors;
                    bt_colors.normalColor = new Color32(255, 0, 0, 100); 
                    lgpd.GetComponentInChildren<Button>().colors = bt_colors;
                    
                    nextButton.interactable = false;

                    nextButton.onClick.RemoveAllListeners();
                }
            }
            else
            {
                nextButton.interactable = true;

                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(delegate
                {
                    Debug.Log("next screen");

                    NextScreen();
                });
            }            
        }
        else
        {
            nextButton.interactable = false;

            nextButton.onClick.RemoveAllListeners();
        }
    }

    private void NextScreen()
    {
        forms[idx].SetActive(false);
        idx += 1;
        slider.value = idx + 1;
        int c = idx + 1;
        slider.GetComponentInChildren<Text>().text = "PASSO " + c + " DE " + forms.Length;
        forms[idx].SetActive(true);

        CheckValidateForm();
    }

    private void PrevScreen()
    {
        forms[idx].SetActive(false);
        idx -= 1;
        slider.value = idx + 1;
        int c = idx + 1;
        slider.GetComponentInChildren<Text>().text = "PASSO " + c + " DE " + forms.Length;
        forms[idx].SetActive(true);

        CheckValidateForm();
    }
}
