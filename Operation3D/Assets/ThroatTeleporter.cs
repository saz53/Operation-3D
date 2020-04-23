using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroatTeleporter : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = new Vector3(20, 2, -62);
            cc.enabled = true;
        }

    }
}
