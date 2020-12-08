using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearTrap : MonoBehaviour
{
    public float trapTime = 10f;
    private Animator animator;
    private Collider colliderTrigger;
    private bool isActive = true;

    private AudioSource trapSound;

    private NavMeshAgent trappedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        colliderTrigger = GetComponent<Collider>();
        trapSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Trap(GameObject enemy)
    {
        animator.SetBool("isTriggered", true);
        isActive = false;
        trapSound.Play();

        if (enemy.GetComponent<KlownAi>())
        {
            enemy.GetComponent<KlownAi>().trapped = true;
            enemy.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            enemy.GetComponent<NavMeshAgent>().isStopped = true;
        } else if (enemy.GetComponent<Princess>())
        {
            enemy.GetComponent<Princess>().trapped = true;
            enemy.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        } else if (enemy.GetComponent<Peanut>())
        {
            enemy.GetComponent<Peanut>().trapped = true;
            enemy.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        }

        StartCoroutine(Release(enemy));
    }

    IEnumerator Release(GameObject enemy)
    {
        yield return new WaitForSeconds(trapTime);
        animator.SetBool("isTriggered", false);
        isActive = true;
        if (enemy.GetComponent<KlownAi>())
        {
            enemy.GetComponent<KlownAi>().trapped = false;
            enemy.GetComponent<NavMeshAgent>().isStopped = false;
        }
        else if (enemy.GetComponent<Princess>())
        {
            enemy.GetComponent<Princess>().trapped = false;
        }
        else if (enemy.GetComponent<Peanut>())
        {
            enemy.GetComponent<Peanut>().trapped = false;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && isActive)
        {
            Trap(other.gameObject);
        }
    }
}
