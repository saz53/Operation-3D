using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothUI : MonoBehaviour
{
    public GameObject Tooth;
    public static bool GameisPaused = true;

    // Update is called once per frame
    void Update()
    {
        Tooth.SetActive(true);

    }
}
