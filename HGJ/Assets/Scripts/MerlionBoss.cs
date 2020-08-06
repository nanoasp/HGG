using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ML_IdleState : IState
{
    MerlionBoss owner;

    public ML_IdleState(MerlionBoss owner)
    {
        this.owner = owner;
    }

    void Spawn()
    {

    }

    public void Enter()
    {

    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}

public class ML_LaserAttackState : IState
{
    MerlionBoss owner;
    MerlionFowardLaser mLaser;
    float mStartDelay = 0.5f;
    float mEndDelay = 0.25f;
    float mSizeCounter = 0.0f;
    Transform mLaserTransform;
    const float laser_size = 3.0f;

    public ML_LaserAttackState(MerlionBoss owner)
    {
        this.owner = owner;
        mLaserTransform = owner.mLaser.transform;
        mLaser = owner.mLaser2.GetComponent<MerlionFowardLaser>();
    }

    void Spawn()
    {

    }

    public void Enter()
    {
        owner.mIsLaserAttack = true;
        owner.mAnimator.SetBool("IsLaser", true);
    }

    public void Execute()
    {
        if(mStartDelay > 0.0f)
        {
            mStartDelay -= Time.deltaTime;
            return;
        }

        if(mSizeCounter < laser_size)
        {
            mSizeCounter += 20.0f * Time.deltaTime;

            if (mSizeCounter > laser_size)
            {
                mSizeCounter = laser_size;
                mLaser.Activate();
            }
            
            mLaserTransform.localScale = new Vector3(laser_size, mSizeCounter, 0.0f);

            return;
        }

        if(!mLaser.mActive)
        {
            mLaserTransform.localScale = new Vector3(laser_size, 0.0f, 0.0f);

            mEndDelay -= Time.deltaTime;
            if(mEndDelay <=0.0f)
            {
                owner.mStateMachine.ChangeState(0);
            }
        }
    }

    public void Exit()
    {
        mLaserTransform.localScale = new Vector3(laser_size, 0.0f, 0.0f);
        owner.mIsLaserAttack = false;
        owner.mAnimator.SetBool("IsLaser", false);
        owner.mIdle = true;
        mStartDelay = 0.5f;
        mEndDelay = 0.25f;
        mSizeCounter = 0.0f;
    }
}

public class MerlionBoss : MonoBehaviour
{
    public enum ML_STATES
    {
        IDLE = 0,
        LASER_ATTACK,
        END
    };

    public GameObject mLaser;
    public GameObject mLaser2;
    public StateMachine mStateMachine = new StateMachine();
    public Animator mAnimator;

    public float mAttackInterval;
    public float mAttackCounter = 0.0f;
    public int mAttackPattern = 0;
    
    public bool mIdle = true;
    public bool mIsLaserAttack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //we default to idle state
        mStateMachine.RegisterState(new ML_IdleState(this), (int)ML_STATES.IDLE);
        mStateMachine.RegisterState(new ML_LaserAttackState(this), (int)ML_STATES.LASER_ATTACK);
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Update();
       
        if (mIdle)
        {
            mAttackCounter += Time.deltaTime;

            if(mAttackCounter >= mAttackInterval)
            {
                mStateMachine.ChangeState(mAttackPattern); 
                
                mAttackPattern += 1;

                if (mAttackPattern == (int)ML_STATES.END)
                    mAttackPattern = 1;
                
                mAttackCounter = 0.0f;
                mIdle = false;
            }
        }
    }
}
