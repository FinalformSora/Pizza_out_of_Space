using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMiniGame : MonoBehaviour
{
    private GameObject player;
    private GameObject connectWires;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        connectWires = GameObject.FindWithTag("Wires");
    }
    public void QuitMini()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        connectWires.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;
    }
}
