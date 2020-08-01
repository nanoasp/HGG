﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerAunty : MonoBehaviour
{
    enum bossPhases
    {
        IDLE,
        ALERT,
        ANGRY,
        ENRAGED

    };
    public enum BOSSATTACKS
    {
        IDLE,
        P1SHOOTFRONT,
        P1DASHFRONT,
        P2ARCBOMBARDMENT,
        P2SPIN,
        P3SHOOTFRONT,
        P3something,
        P3TROLLEYBOMBARDMENT,
        DEAD

    };
    public GameObject trolleyPrefab;
    public GameObject myTrolley;
    public GameObject bullet1Prefab;
    public GameObject bullet2Prefab;
    public GameObject bullet3Prefab;

    public float currentHealth;
    public bool invul;


    Rigidbody2D mRB2D;
    Collider2D mCollider;
    Transform mTransform;
    public StateMachine mStateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        //we default to idle state
        mStateMachine.RegisterState(new BAIdleState(this), (int)BOSSATTACKS.IDLE);
        mStateMachine.RegisterState(new P1SHOOTFRONTState(this), (int)BOSSATTACKS.P1SHOOTFRONT);
        //mStateMachine.RegisterState(new P1DASHFRONTState(this), (int)BOSSATTACKS.P1DASHFRONT);
        //mStateMachine.RegisterState(new P2ARCBOMBARDMENTState(this), (int)BOSSATTACKS.P2ARCBOMBARDMENT);
        //mStateMachine.RegisterState(new P2SPINState(this), (int)BOSSATTACKS.P2SPIN);
        //mStateMachine.RegisterState(new P3SHOOTFRONTState(this), (int)BOSSATTACKS.P3SHOOTFRONT);
        //mStateMachine.RegisterState(new P3somethingState(this), (int)BOSSATTACKS.P3something);
        //mStateMachine.RegisterState(new P3TROLLEYBOMBARDMENTState(this), (int)BOSSATTACKS.P3TROLLEYBOMBARDMENT);
        //mStateMachine.RegisterState(new DEADState(this), (int)BOSSATTACKS.DEAD);

        mStateMachine.ChangeState((int)BOSSATTACKS.IDLE);

        mTransform = GetComponent<Transform>();
        mRB2D = GetComponent<Rigidbody2D>();

        myTrolley = Instantiate(trolleyPrefab);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class BAIdleState : IState
{
    BoomerAunty  owner;
    float currtimer = 0.0f;

    public BAIdleState(BoomerAunty owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        currtimer += Time.deltaTime;
        if (currtimer > 1.2f)
        {
            owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P1DASHFRONT);
        }
    }
    public void Exit()
    {

    }
}
public class P1SHOOTFRONTState : IState
{
    BoomerAunty owner;

    float delayBeforeShooting = 2.8f;
    float shotDelay = 0.6f;
    bool isShooting;
    float currtimer = 0.0f;
    float aligntimer = 0.0f;
    int shotCount;
    float phaseTimer = 0.0f;
    public P1SHOOTFRONTState(BoomerAunty owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.currentHealth = 100;
        isShooting = false;
        currtimer = 0.0f;
    }

    public void Execute()
    {
        currtimer += Time.deltaTime;
        if (!isShooting && currtimer > delayBeforeShooting) {
            // starts shooting process
            //isShooting = true;
            moveTrolleytoFirePos();
            

        }
        else if (isShooting && currtimer > shotDelay)
        {
            if (shotCount < 3)
            {
                // shoots a bullet
                owner.myTrolley.GetComponent<Trolleybehaviour>().ShootBullet();
                shotCount++;
            }
            else {
                moveTrolleytoDefultPos();
            }

        }

        if (owner.currentHealth < 0 || phaseTimer > 60.0f) {
            moveTrolleytoDefultPos();
            if (isShooting && currtimer==0) { 
            owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P1DASHFRONT);
            }
        }
    }

    void moveTrolleytoFirePos() {
        Vector3 tspos = owner.myTrolley.transform.position + (Vector3.left);
        Vector3 tepos = owner.myTrolley.transform.position + (Vector3.up) + (Vector3.right);
        Quaternion tsrot = owner.myTrolley.transform.rotation;

        // should be correct pls test
        aligntimer = Easing.Circular.Out(aligntimer, 0.4f);
        owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, aligntimer);
        owner.myTrolley.transform.Rotate(new Vector3(0,0, -(90 / (0.4f / Time.deltaTime))));
        if (aligntimer> 0.0f) {
            isShooting = true;
            currtimer = 0.0f;
        } 
    }
    void moveTrolleytoDefultPos()
    {
        Vector3 tepos = owner.myTrolley.transform.position + (Vector3.left);
        Vector3 tspos = owner.myTrolley.transform.position + (Vector3.up) + (Vector3.right);
        // should be correct pls test
        aligntimer = Easing.Circular.Out(aligntimer, 0.4f);
        owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, aligntimer);
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (90 / (0.4f / Time.deltaTime))));
        if (aligntimer > 0.0f)
        {
            isShooting = false;
            currtimer = 0.0f;
        }
    }
    public void Exit()
    {

    }
}