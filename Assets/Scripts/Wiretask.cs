using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiretask : MonoBehaviour
{
    public PlayerController player;
    public QuitPopMenu game;

    public int numWires;

    // Random Colors 
    public Color[] wireColors = new Color[4];

    // Left and right wires
    public Wire[] leftWires = new Wire[4];
    public Wire[] rightWires = new Wire[4];

    private int[] leftRandom = new int[4];
    private int[] rightRandom = new int[4];

    public Wire currentDraggedWire;

    public Wire currentHoveredWire;

    private bool isTaskCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Populates both the leftRandom and rightRandom array
        for (int i = 0; i < leftRandom.Length; i++) { leftRandom[i] = i; rightRandom[i] = i; };
        resetWires();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void resetWires()
    {
        // Resets and Randomizes wires
        for (int i = 0; i < leftRandom.Length; i++)
        {
            leftWires[i].Reset();
            rightWires[i].Reset();

            int randomIndex = Random.Range(0, leftRandom.Length - 1);
            int temp = leftRandom[i];
            leftRandom[i] = leftRandom[randomIndex];
            leftRandom[randomIndex] = temp;

            randomIndex = Random.Range(0, leftRandom.Length - 1);
            temp = rightRandom[i];
            rightRandom[i] = rightRandom[randomIndex];
            rightRandom[randomIndex] = temp;
        }

        // Recolors the wires
        for (int i = 0; i < leftWires.Length; i++)
        {
            leftWires[i].setColor(wireColors[leftRandom[i]]);
            rightWires[i].setColor(wireColors[rightRandom[i]]);
        }

        currentDraggedWire = null;
        currentHoveredWire = null;
        isTaskCompleted = false;

        StartCoroutine(CheckTaskCompletion());
    }

    private IEnumerator CheckTaskCompletion()
    {
        while (!isTaskCompleted)
        {
            int successfulWires = 0;
            for (int i = 0; i < rightWires.Length; i++)
            {
                if (rightWires[i].isSuccessful) { successfulWires++; }
            }

            if (successfulWires >= rightWires.Length)
            {
                Debug.Log("Task Completed");
                isTaskCompleted = true;
                player.modifyMoney(10);
                resetWires();
                game.QuitMini();
                player.GetComponent<FixMachine>().leaveMenu();
                this.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
