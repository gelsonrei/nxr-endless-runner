using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using EndlessRunner;

public class PlayerControl : MonoBehaviour
{
    private Animator m_animator;
    private AudioSource m_audioSource;
    private Rigidbody m_rigidbody;
    private Transform m_transform;
    private PlayerInput m_player_input;

    [HideInInspector]
    public BoxCollider m_box_colider;

    [HideInInspector]
    public bool isMagnetic = false;

    [Header("Characters")]
    public GameObject[] characters;

    [Header("Player Options")]
    public int lifes = 3;
    public float velocity = 4.0f;
    public float maxVelocity = 10.0f;
    public float leftOffset = -1.0f;
    public float rigthOffset = 1.0f;

    [Header("Touch Options")]
    public float minSwipeDistance = 50f; // Minimum distance to consider it a swipe
    private Vector2 touchStartPosition;
    private bool isSwiping = false;

    [Header("Audio")]
    public AudioClip[] runAudioClips;
    public AudioClip[] jumpAudioClips;
    public AudioClip[] slideAudioClips;
    public AudioClip[] moveAudioClips;
    public AudioClip[] idleAudioClips;


    private float next_x_position = 0f;
    private float timer = 0f;

    private RuntimeAnimatorController m_defult_animator_controler;
    private RuntimeAnimatorController m_special_animator_controler;

    [HideInInspector]
    public bool isSpecial = false;

    private GameObject item;
    private GameObject m_item;

    /*
    * Game
    */
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_transform = GetComponent<Transform>();
        m_player_input = GetComponent<PlayerInput>();
        m_box_colider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        m_transform.position = new Vector3(next_x_position, m_transform.position.y, m_transform.position.z);

        GameObject myCharacter = Instantiate(characters[(int)DataBase.SelectData("playerCharacter")], transform.position, Quaternion.identity, transform);
        m_animator.avatar = myCharacter.GetComponent<Animator>().avatar;
        //m_animator.runtimeAnimatorController = myCharacter.GetComponent<Animator>().runtimeAnimatorController;

        //m_defult_animator_controler = myCharacter.GetComponent<Variables>().declarations.Get<RuntimeAnimatorController>("DefaultAnimatorControler");
        //m_special_animator_controler = myCharacter.GetComponent<Variables>().declarations.Get<RuntimeAnimatorController>("SpecialAinmatorControler");
        //item = myCharacter.GetComponent<Variables>().declarations.Get<GameObject>("Item");

        m_defult_animator_controler = myCharacter.GetComponent<SlotPlayerVariablesables>().DefaultAnimatorControler;
        m_special_animator_controler = myCharacter.GetComponent<SlotPlayerVariablesables>().SpecialAinmatorControler;
        item = myCharacter.GetComponent<SlotPlayerVariablesables>().Item;

        m_animator.runtimeAnimatorController = m_defult_animator_controler;

        disableMagnetic();
    }


    private void Update()
    {
        if (lifes == 0)
        {
            OnToggleOff("isRunning");
            OnToggleOff("isJumping");
            OnToggleOff("isSliding");
            OnToggleOff("isLeft");
            OnToggleOff("isRigth");
            OnToggleOn("isLose");

            disableMagnetic();
            disableSpecial();
        }

        GameManager.Instance.distance = (int)(GameManager.Instance.levelManager.transform.position.z * -1);

        timer += Time.deltaTime;


        if (timer >= GameManager.Instance.tickInterval && m_animator.GetBool("isRunning") && velocity < maxVelocity)
        {
            velocity += 1.0f;
            timer = 0f;
        }

        if ((GameManager.Instance.points > 0 && GameManager.Instance.points % GameManager.Instance.pointsForSpecial == 0) && !isSpecial && lifes > 0)
        {
            enableSpecial(10);
        }

        /*
        * Swipe Control
        */
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 currentTouchPosition = Mouse.current.position.ReadValue();

                if (!isSwiping)
                {
                    touchStartPosition = currentTouchPosition;
                    isSwiping = true;
                }
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Vector2 currentTouchPosition = Mouse.current.position.ReadValue();
                isSwiping = false;

                float swipeDistance = Vector2.Distance(touchStartPosition, currentTouchPosition);

                if (swipeDistance >= minSwipeDistance)
                {
                    Vector2 swipeDirection = currentTouchPosition - touchStartPosition;
                    swipeDirection.Normalize();

                    if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                    {
                        if (swipeDirection.x > 0)
                        {
                            OnRigth();
                        }
                        else
                        {
                            OnLeft();
                        }
                    }
                    else
                    {
                        if (swipeDirection.y > 0)
                        {
                            OnJump();
                        }
                        else
                        {
                            OnSlide();
                        }
                    }
                }
            }
        }

        if (Touchscreen.current != null)
        {
            if (Touchscreen.current.primaryTouch.press.isPressed)
            {
                Vector2 currentTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                if (!isSwiping)
                {
                    touchStartPosition = currentTouchPosition;
                    isSwiping = true;
                }
            }

            if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            {
                Vector2 currentTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                isSwiping = false;

                float swipeDistance = Vector2.Distance(touchStartPosition, currentTouchPosition);

                if (swipeDistance >= minSwipeDistance)
                {
                    Vector2 swipeDirection = currentTouchPosition - touchStartPosition;
                    swipeDirection.Normalize();

                    if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                    {
                        if (swipeDirection.x > 0)
                        {
                            OnRigth();
                        }
                        else
                        {
                            OnLeft();
                        }
                    }
                    else
                    {
                        if (swipeDirection.y > 0)
                        {
                            OnJump();
                        }
                        else
                        {
                            OnSlide();
                        }
                    }
                }
            }
        }

        if (m_animator.GetBool("isRunning"))
        {
            //m_rigidbody.MovePosition(m_rigidbody.position + new Vector3(0, 0, velocity) * m_animator.deltaPosition.magnitude);
            GameManager.Instance.levelManager.transform.Translate(new Vector3(0, 0, -velocity) * Time.deltaTime * 3.0f);

            if (!isSpecial && !m_animator.GetBool("isJumping") && !m_animator.GetBool("isJumping") && !m_animator.GetBool("isSliding") && !m_animator.GetBool("isLeft") && !m_animator.GetBool("isRigth"))
            {
                PlaySound(runAudioClips[Random.Range(0, runAudioClips.Length - 1)], velocity / 4, true);
            }
        }

        /* this block is a pig code, refact in a future */
        if (m_animator.GetBool("isColide"))
        {
            if (velocity > 0)
            {
                velocity -= 0.05f;
            }
        }
        else
        {
            if (!isSpecial)
            {
                if (velocity < 4)
                {
                    velocity += 0.05f;
                }
            }
            else
            {
                if (velocity < 5)
                {
                    velocity += 0.05f;
                }
            }
        }
        /* this block is a pig code */
    }

    /*
    * Game Logic
    */

    public void enableMagnetic(float time)
    {
        m_box_colider.enabled = true;
        isMagnetic = true;

        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        gmc.ShowPowerUp();

        PulseShaderControl psc = transform.GetComponentInChildren<PulseShaderControl>();
        psc.enableShader();

        StartCoroutine(updateMagnetic(time));
    }

    private IEnumerator updateMagnetic(float time)
    {
        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        int index = 0;

        for (float i = 0; i < time; i++)
        {
            if (i >= (time / gmc.powerUpSprites.Length))
            {
                gmc.AnimatePowerUp(index);
                index = (index + 1) % gmc.powerUpSprites.Length;
            }

            yield return new WaitForSeconds(1);
        }

        disableMagnetic();
    }

    public void disableMagnetic()
    {
        m_box_colider.enabled = false;
        isMagnetic = false;

        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        gmc.HidePowerUp();

        PulseShaderControl psc = transform.GetComponentInChildren<PulseShaderControl>();
        psc.disableShader();
    }

    public void enableSpecial(float time)
    {
        isSpecial = true;
        velocity += 1.0f;

        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        gmc.ShowEspecial();

        m_animator.runtimeAnimatorController = m_special_animator_controler;

        m_item = Instantiate(item, transform.position, Quaternion.identity, transform);

        StartCoroutine(updateSpecial(time));
        OnToggleOn("isRunning");
    }

    private IEnumerator updateSpecial(float time)
    {
        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        int index = 0;

        for (float i = 0; i < time; i++)
        {
            if (i == 0)
            {
                gmc.ShowSpecialPopUp();
            }

            if (i == 2)
            {
                gmc.HideSpecialPopUp();
            }

            if (i >= (time / gmc.specialUpSprites.Length))
            {
                index = (index + 1) % gmc.specialUpSprites.Length;
                gmc.AnimateEspecial(index);
            }

            yield return new WaitForSeconds(1);
        }

        disableSpecial();
    }

    public void disableSpecial()
    {
        isSpecial = false;
        velocity -= 1.0f;

        Destroy(m_item);

        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        gmc.HideEspecial();

        if (!m_animator.GetBool("isLose") || lifes > 0)
        {
            m_animator.runtimeAnimatorController = m_defult_animator_controler;
            OnToggleOn("isRunning");
        }

    }

    public void takeHit()
    {
        OnToggleOn("isColide");

        lifes--;

        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
        gmc.UpdateLifes(lifes);

        if (lifes == 0)
        {
            OnToggleOn("isLose");

            if (gmc.newRecord) 
            {
                gmc.ShowRecordUI(5);
            } else {
                ShowLoseUI();
            }

        }
    }

    public void ShowLoseUI()
    {
        GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();

        gmc.ShowLoseUI();
    }

    /*
    * Audio
    */

    private void PlaySound(AudioClip audioClip, float pitch = 1, bool loop = false)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.pitch = pitch;
        m_audioSource.loop = loop;

        if (m_audioSource.clip)
        {
            if (!m_audioSource.isPlaying)
                m_audioSource.Play();
        }
    }

    /*
    * Controls
    */

    private void OnEnable()
    {
        //Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        //Debug.Log("OnDisable");
    }

    private void OnLeft()
    {
        if (m_animator.GetBool("isRunning") && !m_animator.GetBool("isJumping") && !m_animator.GetBool("isSliding"))
        {
            if (moveAudioClips.Length > 0)
            {
                PlaySound(moveAudioClips[Random.Range(0, moveAudioClips.Length - 1)], velocity / 4);
            }

            if (transform.position.x == rigthOffset)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;

                OnToggleOn("isLeft");

                next_x_position = 0;
            }

            else if (transform.position.x == 0)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;

                OnToggleOn("isLeft");

                next_x_position = leftOffset;
            }
        }
    }

    private void OnRigth()
    {
        if (m_animator.GetBool("isRunning") && !m_animator.GetBool("isJumping") && !m_animator.GetBool("isSliding"))
        {
            if (moveAudioClips.Length > 0)
            {
                PlaySound(moveAudioClips[Random.Range(0, moveAudioClips.Length - 1)], velocity / 4);
            }

            if (transform.position.x == leftOffset)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;

                OnToggleOn("isRigth");

                next_x_position = 0;
            }

            else if (transform.position.x == 0)
            {
                m_rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;

                OnToggleOn("isRigth");

                next_x_position = rigthOffset;
            }
        }
    }

    private void OnJump()
    {
        if (m_animator.GetBool("isRunning") && !m_animator.GetBool("isJumping") && !m_animator.GetBool("isSliding") && !m_animator.GetBool("isLeft") && !m_animator.GetBool("isRigth"))
        {
            if (isSpecial)
            {
                //PlaySound(item.GetComponent<Variables>().declarations.Get<AudioClip>("jumpSound"));
                PlaySound(item.GetComponent<SlotItemVariables>().itemSounds[0]);
            }
            else
            {
                if (jumpAudioClips.Length > 0)
                {
                    PlaySound(jumpAudioClips[Random.Range(0, jumpAudioClips.Length - 1)]);
                }
            }

            OnToggleOn("isJumping");
        }
    }

    private void OnSlide()
    {
        if (m_animator.GetBool("isRunning") && !m_animator.GetBool("isJumping") && !m_animator.GetBool("isSliding") && !m_animator.GetBool("isLeft") && !m_animator.GetBool("isRigth"))
        {
            if (isSpecial)
            {
                //PlaySound(item.GetComponent<Variables>().declarations.Get<AudioClip>("slideSound"));
                PlaySound(item.GetComponent<SlotItemVariables>().itemSounds[1]);
            }
            else
            {
                if (slideAudioClips.Length > 0)
                {
                    PlaySound(slideAudioClips[Random.Range(0, slideAudioClips.Length - 1)]);
                }
            }

            OnToggleOn("isSliding");
        }
    }

    public void OnStart()
    {
        if (!m_animator.GetBool("isRunning"))
        {
            if (idleAudioClips.Length > 0)
            {
                PlaySound(idleAudioClips[Random.Range(0, idleAudioClips.Length - 1)]);
            }

            OnToggleOn("isRunning");
        }
    }

    /*
    * Animations
    */

    public void ChangeCenterCapsuleCollider(float y)
    {
        transform.GetComponent<CapsuleCollider>().center = new Vector3(0, y, 0);
    }
    public void ChangeRadiusCapsuleCollider(float radius)
    {
        transform.GetComponent<CapsuleCollider>().radius = radius;
    }
    public void ChangeHeigthCapsuleCollider(float heigth)
    {
        transform.GetComponent<CapsuleCollider>().height = heigth;
    }

    private void OnAnimatorMove()
    {
        if (m_animator.GetBool("isJumping"))
        {
            m_rigidbody.MovePosition(m_rigidbody.position + new Vector3(0, 2.0f, 0) * m_animator.deltaPosition.magnitude);
        }

        else if (m_animator.GetBool("isRigth"))
        {
            if (m_rigidbody.position.x <= next_x_position)
            {
                m_rigidbody.MovePosition(m_rigidbody.position + new Vector3(velocity + 1.0f, 0, 0) * m_animator.deltaPosition.magnitude);
            }
            else
            {
                OnToggleOff("isRigth");
            }
        }

        else if (m_animator.GetBool("isLeft"))
        {
            if (m_rigidbody.position.x >= next_x_position)
            {
                m_rigidbody.MovePosition(m_rigidbody.position + new Vector3(-velocity - 1.0f, 0, 0) * m_animator.deltaPosition.magnitude);
            }
            else
            {
                OnToggleOff("isLeft");
            }
        }
    }

    public void OnToggleOff(string parameter)
    {
        m_animator.SetBool(parameter, false);

        m_transform.position = new Vector3(next_x_position, m_transform.position.y, m_transform.position.z);
        m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
    }

    public void OnToggleOn(string parameter)
    {
        m_animator.SetBool(parameter, true);
    }
}
