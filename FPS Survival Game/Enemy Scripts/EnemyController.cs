using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    // 이 코드 없으면 HealthScript에서 enemy_Controller.Enemy_State 이런식으로 호출 못함
    public EnemyState Enemy_State { get; set; }

    public GameObject attack_Point;

    private EnemyAnimator enemy_Anim;
    private EnemyState enemy_State;
    private EnemyAudio enemy_Audio;
    private NavMeshAgent navAgent;
    private Transform target;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;
    public float chase_Distance = 20f;
    public float wait_Before_Attack = 2f;
    public float attack_Distance = 2.2f;
    public float chase_After_Attack_Distance = 2f;
    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;

    private float patrol_Timer;
    private float attack_Timer;
    private float current_Chase_Distance;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        enemy_Audio = GetComponentInChildren<EnemyAudio>();
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
    }

    void Start()
    {
        enemy_State = EnemyState.PATROL;
        patrol_Timer = patrol_For_This_Time;
        attack_Timer = wait_Before_Attack; // 좀비가 플레이어에 도달하면 바로 공격
        current_Chase_Distance = chase_Distance; // chase_Distance 초기값 저장
    }

    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }

        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        navAgent.isStopped = false; // 움직일 수 있음
        navAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime;
        if (patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;
            enemy_Audio.Play_ScreamSound();
        }
    }

    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }

        if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            // chase_Distance값을 기본값으로 리셋
            if (chase_Distance != current_Chase_Distance)
                chase_Distance = current_Chase_Distance;
        }
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;
            patrol_Timer = patrol_For_This_Time; // 바로 SetNewRandomDestination 하게끔

            // chase_Distance값을 기본값으로 리셋
            if (chase_Distance != current_Chase_Distance)
                chase_Distance = current_Chase_Distance;
        }
    }

    void Attack()
    {
        navAgent.velocity = Vector3.zero; // 공격이 시작되면 제자리에 서도록
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;
        if (attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();
            attack_Timer = 0f;
            enemy_Audio.Play_AttackSound();
        }

        if (Vector3.Distance(transform.position, target.position)
                 > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }

    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);
        navAgent.SetDestination(navHit.position);
    }

    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy) attack_Point.SetActive(false);
    }
}
