using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceen : MonoBehaviour
{
    [SerializeField] GameObject main;

    public void PlayCutSceen()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(2);
    }
}
