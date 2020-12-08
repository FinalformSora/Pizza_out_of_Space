using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.UIElements;

public class sirenHeadAi : MonoBehaviour
{
    [SerializeField] Transform hold;
    public Transform target;
    [SerializeField] Transform setLocation;
    [SerializeField] Transform setLocation1;
    [SerializeField] Transform setLocation2;
    [SerializeField] Transform setLocation3;
    [SerializeField] Transform setLocation4;
    [SerializeField] Transform setLocation5;
    [SerializeField] Transform setLocation6;

    [SerializeField] public float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    //Components for Eye Vision
    [SerializeField] float captureRange = 2f;
    [Range(0, 360)]  public float viewAngle;
    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();
    [SerializeField] float meshResolution;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    NavMeshAgent navMeshAgent;

    float distanceToTarget = Mathf.Infinity;
    float distanceToLocation = Mathf.Infinity;
    float timer = 0;
    public int secs;

    public bool isProvoked = false;
    public bool isInSight = false;

    public AudioSource sirens;

    //Generate Fear Components
    private Fear playerFear;

    public bool attracted = false;
    public bool repelled = false;
    public bool endGame = false;

    void Start()
    {
        /*viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;*/
        hold = GameObject.FindGameObjectWithTag("Player").transform;
        target = hold;
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine("FindTaregtWithDelay", .3f);
        playerFear = target.GetComponent<Fear>();
    }

    void Update()
    {
        if (endGame)
        {
            if (!sirens.isPlaying)
            {
                sirens.Play();
            }
            ChaseTarget();
            distanceToTarget = Vector3.Distance(hold.position, transform.position);
            if (distanceToTarget <= captureRange)
            {
                FaceTarget();
                AttackTarget();
            }
        }
        else
        {
            if (target == null)
            {
                target = hold;
                attracted = false;
                repelled = false;
                isProvoked = false;
            }
            DrawRayCastsFromVision();
            distanceToTarget = Vector3.Distance(target.position, transform.position);
            distanceToLocation = Vector3.Distance(setLocation.position, transform.position);
            if (attracted)
            {
                ChaseTarget();
            }
            else if (repelled)
            {
                isProvoked = false;
                WalkPath();
            }
            else if (isProvoked && (!attracted || !repelled))
            {
                if (!sirens.isPlaying)
                {
                    sirens.Play();
                }
                EngageTarget();
            }
            else if (!attracted && !repelled)
            {
                if (distanceToTarget < chaseRange)
                {
                    playerFear.invokeFear();
                }
                else if (distanceToTarget > chaseRange)
                {
                    WalkPath();
                }
            }
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void EngageTarget()
    {
        if (isInSight)
        {
            ChaseTarget();
        }
        if (distanceToTarget <= captureRange)
        {
            FaceTarget();
            AttackTarget();
        }
        if (distanceToLocation >= chaseRange)
        {
            timer += Time.deltaTime;
            secs = (int)(timer % 60);
            //Debug.Log("Timer " + secs);
            if (secs >= 10)
            {
                isProvoked = false;
                isInSight = false;
                timer = 0;
                secs = 0;
            }
        }
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
        FaceTarget();
    }

    private void WalkPath()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(setLocation.position);
        int num = 0;
        //Debug.Log(distanceToLocation);
        if(distanceToLocation <= 3.5)
        {
            System.Random rnd = new System.Random();
            num = rnd.Next(1, 7);
            //Debug.Log("The rando " + num);
            switch(num)
            {
                case 1:
                    setLocation = setLocation1;
                    break;
                case 2:
                    setLocation = setLocation2;
                    break;
                case 3:
                    setLocation = setLocation3;
                    break;
                case 4:
                    setLocation = setLocation4;
                    break;
                case 5:
                    setLocation = setLocation5;
                    break;
                case 6:
                    setLocation = setLocation6;
                    break;
            }
        }
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
        //Debug.Log(name + " has seeked and is destroying " + target.name);
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    IEnumerator FindTaregtWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            LookVision();
        }
    }

    void LookVision()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, chaseRange, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle (transform.forward, dirToTarget) < viewAngle/2)
            {
                print("Siren Saw " + target);
                isProvoked = true;
                isInSight = true;
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast (transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    print("Siren Saw " + target);
                    visibleTargets.Add(target);

                }
            }
        }
    }

    void DrawRayCastsFromVision()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
            Debug.DrawLine(transform.position, transform.position + DirectionFromAngle(angle, true) * chaseRange, Color.yellow);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for(int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = viewPoints[i];

            if(i < vertexCount-2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, dir, out hit, chaseRange, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else 
        {
            return new ViewCastInfo(false, transform.position + dir * chaseRange, chaseRange, globalAngle);
        }

    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public Vector3 DirectionFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }

    //Coats an area that provokes AI to chase
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, captureRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward);
        //Gizmos.DrawRay()
    }
}
