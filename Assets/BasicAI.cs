using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public delegate void AIState();
    public AIState currentState;
    private float stateTime = 0;

    private Creature myCreature;
    [SerializeField] private Creature targetCreature;
    [SerializeField] private float sightRange = 10;
    private LayerMask terrainMask;
    [SerializeField] private float jumpForce = 3.0f;

    private void ResetStateTime()
    {
        stateTime = 0;
    }

    private void ChangeToState(AIState state)
    {
        currentState = state;
        ResetStateTime();
    }

    private void AITick()
    {
        currentState();
        stateTime += Time.fixedDeltaTime;
    }

    private bool patrolRight = true;

    private void PatrolState()
    {
        if (CanSeeTarget()) {
            ChangeToState(AttackState);
        }

        if(stateTime < 1){
            myCreature.Stop();
            return;
        }

        int patrolDirection = 1;
        if (patrolRight)
        {
            myCreature.MoveRight();
        } else {
            myCreature.MoveLeft();
            patrolDirection = -1;
        }

        Vector3 footCheckPosition = myCreature.transform.position + new Vector3(patrolDirection,0,0) - new Vector3(0,1,0);
        Vector3 moveCheckPosition = myCreature.transform.position + new Vector3(patrolDirection,0,0);
        if(Physics2D.OverlapCircle(moveCheckPosition,.1f,terrainMask) != null){
            ResetStateTime();
            //Debug.Log("something blocks us");
            //myCreature.Stop();
            patrolRight = !patrolRight;
        }
        else if(Physics2D.OverlapCircle(footCheckPosition,.1f,terrainMask) == null){
            ResetStateTime();
            //Debug.Log("no ground ahead");
            //myCreature.Stop();
            patrolRight = !patrolRight;
        }
    }

    bool hasJumped = false;
    private void AttackState()
    {
        if (stateTime < 1.0f) {
            if (!hasJumped) {
                myCreature.InstantJump(jumpForce);
            }

            hasJumped = true;
            return;
        }

        myCreature.Attack();

        if (!CanSeeTarget()) {
            ChangeToState(PatrolState);
        }

        ResetStateTime();
        hasJumped = false;
    }

    private bool CanSeeTarget()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(myCreature.transform.position, targetCreature.transform.position);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                hit.collider.gameObject.CompareTag("BlocksLOS"))
            {
                return false;
            }
        }

        if (Vector3.Distance(myCreature.transform.position, targetCreature.transform.position) <= sightRange) {
            return true;
        }

        return false;
    }

    void Awake()
    {
        currentState = PatrolState;
    }

    // Start is called before the first frame update
    void Start()
    {
        myCreature = GetComponent<Creature>();
        terrainMask = LayerMask.GetMask("Terrain");
    }

    void Update()
    {
        if (currentState == AttackState)
        {
            myCreature.AimWeaponAtGradually(targetCreature.transform.position);
        }
    }

    void FixedUpdate()
    {
        AITick();        
    }
}
