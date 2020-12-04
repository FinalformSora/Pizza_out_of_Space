using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    CodyHealth target;
    [SerializeField] float damage = 40f;
    [SerializeField] float slowSpeed = 0;

    KlownAi enemyAi;
    sirenHeadAi sAi;
    PlayerController pc;

    void Start()
    {
        enemyAi = FindObjectOfType<KlownAi>();
        sAi = FindObjectOfType<sirenHeadAi>();
        target = FindObjectOfType<CodyHealth>();
        pc = FindObjectOfType<PlayerController>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;
        //    if (target.dis)
        target.TakeDamage(damage);
        Debug.Log("Bang Bang");
    }
    public void SlowCody()
    {
        if (target == null) return;
        pc.slowed = true;
        pc.SlowTarget(slowSpeed);
    }
}
