using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CounterMucus
{
    public static int count = 0;
}

public class Target : MonoBehaviour
{
    public float health = 5.0f;
    public int pointValue;

    public ParticleSystem DestroyedEffect;
    public GameObject RedBloodCellUI;
    public GameObject Key;

    [Header("Audio")]
    public RandomPlayer HitPlayer;
    public AudioSource IdleSource;

    //text objects
    public GameObject MucusText_1;
    public GameObject MucusText_2;
    public GameObject MucusText_3;

    public bool Destroyed => m_Destroyed;

    bool m_Destroyed = false;
    float m_CurrentHealth;

    IEnumerator ShowAndHide(GameObject go, float delay)
    {
        go.active = true;
        yield return new WaitForSeconds(delay);
        go.active = false;
    }

    void Awake()
    {
        Helpers.RecursiveLayerChange(transform, LayerMask.NameToLayer("Target"));
    }

    void Start()
    {
        if(DestroyedEffect)
            PoolSystem.Instance.InitPool(DestroyedEffect, 16);
        
        m_CurrentHealth = health;
        if(IdleSource != null)
            IdleSource.time = Random.Range(0.0f, IdleSource.clip.length);
    }

    public void Got(float damage)
    {
        m_CurrentHealth -= damage;
        
        if(HitPlayer != null)
            HitPlayer.PlayRandom();
        
        if(m_CurrentHealth > 0)
            return;

        Vector3 position = transform.position;
        
        //the audiosource of the target will get destroyed, so we need to grab a world one and play the clip through it
        if (HitPlayer != null)
        {
            var source = WorldAudioPool.GetWorldSFXSource();
            source.transform.position = position;
            source.pitch = HitPlayer.source.pitch;
            source.PlayOneShot(HitPlayer.GetRandomClip());
        }

        if (DestroyedEffect != null)
        {
            var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
            effect.time = 0.0f;
            effect.Play();
            effect.transform.position = position;
        }

        m_Destroyed = true;
        gameObject.SetActive(false);
	       
        GameSystem.Instance.TargetDestroyed(pointValue);
	if(RedBloodCellUI != null){
	    RedBloodCellUI.SetActive(true);
            
        Destroy(RedBloodCellUI, 3);
            if (RedBloodCellUI.gameObject.tag == "Mucus")
            {
                CounterMucus.count = CounterMucus.count + 1;

                if (CounterMucus.count == 3)
                {
                    StartCoroutine(ShowAndHide(MucusText_1, 5.0f));
                }
                else if (CounterMucus.count == 8)
                {
                    StartCoroutine(ShowAndHide(MucusText_2, 5.0f));
                }
                else if (CounterMucus.count == 13)
                {
                    StartCoroutine(ShowAndHide(MucusText_3, 5.0f));
                }
            }

        }

    if (CounterMucus.count == 15)
        {
            Key.active = true;
        }
	
    }
	
}
