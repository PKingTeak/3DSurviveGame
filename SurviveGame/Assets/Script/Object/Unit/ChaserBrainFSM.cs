using System;
using System.Collections;
using Attacker;
using Brain;
using Mover;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;


public class ChaseBrainFSM : MonoBehaviour, IBrain
{
    enum BrainState { Idle, Chase, Serach }

    [Header("Traget")]
    [SerializeField] Transform target;
    [SerializeField] LayerMask obstacleMask; //벽

    [Header("Vision")]
    [SerializeField] float detectRadius = 12f;    // 시야 반경
    [SerializeField] float fovAngle = 120f;       // 시야각(전방 기준)
    [SerializeField] float eyeHeight = 1.6f;      // 눈 높이
    [SerializeField] float rotateSpeed = 1.0f;

    [Header("Chase/Search")]
    [SerializeField] float attackRange = 1.6f;    // (브레인이 쓰는 대략 사거리)
    [SerializeField] float stopBuffer = 0.25f;    // 들썩임 방지
    [SerializeField] float loseSightGrace = 1.0f; // 시야 잃고도 추격 유지 기간
    [SerializeField] float searchDuration = 2.0f; // 마지막 지점 도달 후 탐색 유지 시간
    [SerializeField] float arriveThreshold = 0.6f;// Search 목적지 도착 판정

    BrainState state;
    IMover mover;
    IAttacker attacker;
    Unit self;

    Vector3 lastSeePos;
    float lastSeeTime;
    float searchEndTime;

    void Awake()
    {
        mover = GetComponent<IMover>();
        attacker = GetComponent<IAttacker>();
        self = GetComponent<Unit>();

        state = BrainState.Idle;
    }

    void Update() => Think();


    public void Think()
    {
        switch (state)
        {
            case BrainState.Idle:
                UpdateIdle();
                break;
            case BrainState.Chase:
                UpdateChase();
                break;
            case BrainState.Serach:
                UpdateSerach();
                break;
            
        }
    }


    void UpdateIdle()
    {
        mover?.Stop();
        if (IsVisible(target))
        {
            lastSeePos = target.position;
            lastSeeTime = Time.time;
            state = BrainState.Chase; //추격 시작
            
        }
    }

    void UpdateChase()
    { 
        if(!target) { state = BrainState.Idle; return; }  //타겟이 없을때

        bool visible = IsVisible(target);
        Vector3 toXZ = Flat(target.position - transform.position);
        float dist = toXZ.magnitude;

        if (visible)
        {
            lastSeePos = target.position;
            lastSeeTime = Time.time;

            if (dist > attackRange + stopBuffer)
            {
                mover?.Move(toXZ.normalized); //해당 방향으로 이동
            }
            else
            {
                mover?.Stop();
            }
        }

        else
        {
            if (Time.time - lastSeeTime < loseSightGrace)
            {
                Vector3 tolast = Flat(lastSeePos - transform.position);
                if (tolast.sqrMagnitude > arriveThreshold * arriveThreshold)
                {
                    mover?.Move(tolast.normalized);
                }
                else
                {
                    mover?.Stop();
                }
            }
            else
            {
                searchEndTime = Time.time + searchDuration;
                state = BrainState.Serach;//탐색 단계로 변환
            }
        }


        if (visible && dist <= attackRange + stopBuffer && attacker != null && attacker.canAttack && target.TryGetComponent(out Unit tu))
        {
            // 시도만, 최종 판정은 IAttacker 내부에서
            attacker.Attack(tu, 1);
        }

        if (toXZ.sqrMagnitude > 0.0001f)
        {
            transform.forward = toXZ.normalized;
        }



    }


    public void UpdateSerach()
    {
        if (IsVisible(target))
        {
            lastSeePos = target.position;
            lastSeeTime = Time.time;
            state = BrainState.Chase;
            return;
        }
        //이제 탐색
        Vector3 lastpos = Flat(lastSeePos - transform.position);

        float dist = lastpos.sqrMagnitude;

        if (dist > arriveThreshold * arriveThreshold)
        {
            Vector3 dir = lastpos.normalized;
            mover?.Move(dir);

            if (dir.sqrMagnitude > 1e-4f)
            {
                Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, rotateSpeed * Time.deltaTime);
            }
            return;
        }

        mover?.Stop();
        if (Time.time > searchDuration)
        {
            state = BrainState.Idle;
        }



    }





    bool IsVisible(Transform t)
    {
        if (!t) return false;

        Vector3 eye = transform.position + Vector3.up * eyeHeight;
        Vector3 to = t.position + Vector3.up * eyeHeight - eye;


        if (to.sqrMagnitude < detectRadius * detectRadius) return false;

        Vector3 toFlat = Flat(to); //평면에서만 추격 y=0으로 변환

        float ang = Vector3.Angle(transform.position, toFlat);
        if (ang < fovAngle * 0.5f) return false;

        float dist = to.magnitude;
        if(Physics.Raycast(eye,to.normalized , dist, obstacleMask, QueryTriggerInteraction.Ignore)) return false;


        return true;

    }

    Vector3 Flat(Vector3 v)
    {
        v.y = 0f;
        return v;
    }



}
