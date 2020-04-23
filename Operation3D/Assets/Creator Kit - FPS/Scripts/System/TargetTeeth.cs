using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CounterTeeth
{
    public static int count = 0;
}

public class TargetTeeth : MonoBehaviour
{
    public float health = 5.0f;
    public int pointValue;

    public ParticleSystem DestroyedEffect;
    public GameObject ToothUI;
    public GameObject Key;
    public Material CleanTeeth;

    //text objects
    public GameObject CrownText;
    public GameObject PlaqueText_1;
    public GameObject PlaqueText_2;
    public GameObject PlaqueText_3;
    public GameObject CavityText;

    //teleport
    public GameObject ThroatTeleport;
    

    [Header("Audio")]
    public RandomPlayer HitPlayer;
    public AudioSource IdleSource;


    float m_CurrentHealth;
    static bool CrownText_Appreared = false;

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
        if (DestroyedEffect)
            PoolSystem.Instance.InitPool(DestroyedEffect, 16);

        m_CurrentHealth = health;
        if (IdleSource != null)
            IdleSource.time = Random.Range(0.0f, IdleSource.clip.length);
    }

    public void Got(float damage)
    {
        m_CurrentHealth -= damage;

        if (HitPlayer != null)
            HitPlayer.PlayRandom();

        if (m_CurrentHealth > 0)
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


        GameSystem.Instance.TargetDestroyed(pointValue);

        if (ToothUI != null && ToothUI.gameObject.tag != "GoldTooth")
        {
            ToothUI.SetActive(true);
            GetComponent<MeshRenderer>().material = CleanTeeth;
            CounterTeeth.count = CounterTeeth.count + 1;

            if (CounterTeeth.count == 1)
            {
                StartCoroutine(ShowAndHide(PlaqueText_1, 5.0f));
            } else if (CounterTeeth.count == 8)
            {
                StartCoroutine(ShowAndHide(PlaqueText_2, 5.0f));
            } else if (CounterTeeth.count == 15)
            {
                StartCoroutine(ShowAndHide(PlaqueText_3, 5.0f));
            } else if (CounterTeeth.count == 19)
            {
                StartCoroutine(ShowAndHide(CavityText, 5.0f));
            }


        } else if (ToothUI != null && ToothUI.gameObject.tag == "GoldTooth" && CrownText_Appreared == false)
        {
            CrownText_Appreared = true;
            StartCoroutine(ShowAndHide(CrownText, 5.0f));
        }

      
        if (CounterTeeth.count == 26)
        {
            Key.active = true;
            ThroatTeleport.active = true;
        }
    }

}
