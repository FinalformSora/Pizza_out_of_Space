using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    CodyHealth target;
    [SerializeField] float damage = 40f;

    KlownAi enemyAi;

    void Start()
    {
        enemyAi = FindObjectOfType<KlownAi>();
        target = FindObjectOfType<CodyHealth>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;
        //    if (target.dis)
        target.TakeDamage(damage);
        Debug.Log("Bang Bang");
    }
}
