using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitPopMenu : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void QuitMini()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<PlayerController>().enabled = true;
    }
}
