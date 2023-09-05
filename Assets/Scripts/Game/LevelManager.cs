using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private AudioSource m_audioSource;

    public GameObject[] levels;
    public int scaleOffset = 20;
    public AudioClip[] soundTrackClips;

    private List<GameObject> myInstantieteLevels = new List<GameObject>();
    private float playerInitialZPosition = 0;
    private int levelCount = 1;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    } 

    private void Start()
    {
        PlaySound(soundTrackClips[Random.Range(0, soundTrackClips.Length-1)]);

        if ( levels.Length > 0)
        {
            InstanceLevel(0);
            InstanceLevel(0);
            InstanceLevel(0);
            InstanceLevel(0);
        }

        playerInitialZPosition = GameManager.Instance.player.transform.position.z;
    }

    private void Update()
    {        
        if (GameManager.Instance.player.transform.position.z >= playerInitialZPosition + scaleOffset)
        {
            playerInitialZPosition = GameManager.Instance.player.transform.position.z;

            if(levels.Length > 1)
            {
                int levelLoad = 0;

                if (levelCount <= levels.Length)
                {
                    levelLoad = levelCount - 1;
                }
                else
                {
                    levelCount = 0;
                    levelLoad = levelCount;
                    //levelLoad = Random.Range(0,levels.Length-1);
                }
                
                InstanceLevel(levelLoad);
            }
            else
            {
                InstanceLevel(0);
            }

            DestroyLevel(0);
            
            if(GameManager.Instance.points >= (GameManager.Instance.pointsAvarage * levelCount))
            {
                levelCount++;
            }

            GameManager.Instance.currentLevel = levelCount;
        }
    }

    public void InstanceLevel(int index)
    {
        Vector3 newLocation;

        if (myInstantieteLevels.Count >= 1)
        {
            Transform originLocation = myInstantieteLevels[myInstantieteLevels.Count-1].transform;
            newLocation = new Vector3(0, 0, originLocation.position.z + (originLocation.localScale.z * scaleOffset) );
        }
        else
        {
            newLocation = new Vector3(0, 0, 0);
        }

        GameObject m_level = Instantiate(levels[index], newLocation, Quaternion.identity, transform);
        myInstantieteLevels.Add(m_level);
        
        //transform.position = new Vector3(0, 0, transform.position.z - scaleOffset);
    }

    public void DestroyLevel(int index)
    {
        Destroy(myInstantieteLevels[index]);
        myInstantieteLevels.RemoveAt(index);
    }

    private void PlaySound (AudioClip audioClip, float pitch = 1)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.pitch = pitch;

        if ( m_audioSource.clip )
        {
            m_audioSource.Play();
        }
    }
}
