using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCellUI : MonoBehaviour
{
    public GameObject RedBloodCell;
    public static bool GameisPaused = true;

    // Update is called once per frame
    void Update()
    {
        RedBloodCell.SetActive(true);

    }
}
