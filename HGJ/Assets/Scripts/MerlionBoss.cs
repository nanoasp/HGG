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

public class ML_SingleBeamState : IState
{
    MerlionBoss owner;
    float mStartDelay = 0.5f;
    float mEndDelay = 0.25f;
    float mSizeCounter = 0.0f;
    Transform mLaserTransform;
    const float laser_size = 3.0f;

    float player_x;

    public ML_SingleBeamState(MerlionBoss owner)
    {
        this.owner = owner;
        mLaserTransform = owner.mLaser.transform;
    }

    void Spawn()
    {

    }

    public void Enter()
    {
        owner.mIsSingleShot = true;
        owner.mAnimator.SetBool("IsSingleShot", true);
    }

    public void Execute()
    {
        player_x = owner.mPlayer.transform.position.x;

        if (mStartDelay > 0.0f)
        {
            mStartDelay -= Time.deltaTime;
            return;
        }

        if (mSizeCounter < laser_size)
        {
            mSizeCounter += 20.0f * Time.deltaTime;

            if (mSizeCounter > laser_size)
            {
                mSizeCounter = laser_size;
                GameObject laser = MerlionBoss.Instantiate(owner.mSingleLaserPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, -90.0f)) as GameObject;
                laser.transform.position = new Vector2(player_x, laser.transform.position.y);
            }

            owner.mLaserMouth.transform.localScale = new Vector2(1.0f, 1.0f);
            mLaserTransform.localScale = new Vector3(laser_size, mSizeCounter, 0.0f);

            return;
        }

        mEndDelay -= Time.deltaTime;
        if (mEndDelay <= 0.0f)
        {
            owner.mStateMachine.ChangeState(0);
        }
    }

    public void Exit()
    {
        owner.mLaserMouth.transform.localScale = new Vector2(0.0f, 0.0f);
        mLaserTransform.localScale = new Vector3(laser_size, 0.0f, 0.0f);
        owner.mIsLaserAttack = false;
        owner.mAnimator.SetBool("IsSingleShot", false);
        owner.mIdle = true;
        mStartDelay = 0.5f;
        mEndDelay = 0.25f;
        mSizeCounter = 0.0f;
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

            owner.mLaserMouth.transform.localScale = new Vector2(1.0f, 1.0f);
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
        owner.mLaserMouth.transform.localScale = new Vector2(0.0f, 0.0f);
        mLaserTransform.localScale = new Vector3(laser_size, 0.0f, 0.0f);
        owner.mIsLaserAttack = false;
        owner.mAnimator.SetBool("IsLaser", false);
        owner.mIdle = true;
        mStartDelay = 0.5f;
        mEndDelay = 0.25f;
        mSizeCounter = 0.0f;
    }
}

public class ML_VolleyAttackState : IState
{
    MerlionBoss owner;
    MerlionAcrBulletSpawner mThrower;
    float mStartDelay = 0.5f;
    float mEndDelay = 0.25f;

    float mSpawnInterval = 0.1f;
    float mSpawnCounter = 0.0f;
    float mLife = 5.0f;

    public ML_VolleyAttackState(MerlionBoss owner)
    {
        this.owner = owner;
        mThrower = owner.mVolleySpawner.GetComponent<MerlionAcrBulletSpawner>();
        mSpawnCounter = mSpawnInterval;
    }

    public void Enter()
    {
        owner.mIsLaserAttack = true;
        owner.mAnimator.SetBool("IsLaser", true);
    }

    public void Execute()
    {
        if (mStartDelay > 0.0f)
        {
            mStartDelay -= Time.deltaTime;
            return;
        }

        if(mLife > 0.0f)
        {
            mSpawnCounter += Time.deltaTime;

            if(mSpawnCounter >= mSpawnInterval)
            {
                mThrower.Spawn();
                mSpawnCounter = 0.0f;
            }

            mLife -= Time.deltaTime;
            return;
        }

        mEndDelay -= Time.deltaTime;
        if (mEndDelay <= 0.0f)
        {
            owner.mStateMachine.ChangeState(0);
        }
    }

    public void Exit()
    {
        owner.mIsLaserAttack = false;
        owner.mAnimator.SetBool("IsLaser", false);
        owner.mIdle = true;
        mStartDelay = 0.5f;
        mEndDelay = 0.25f;
        mLife = 5.0f;
    }
}

public class ML_TailAttackState : IState
{
    MerlionBoss owner;
    MerlionAcrBulletSpawner mThrower;
    float mStartDelay = 0.5f;
    float mEndDelay = 0.25f;

    float mSpawnInterval = 0.3f;
    float mSpawnCounter = 0.0f;
    float mLife = 5.0f;

    float mOriginalY = -0.2f;
    float mNewY = -5.0f;

    public ML_TailAttackState(MerlionBoss owner)
    {
        this.owner = owner;
        mThrower = owner.mVolleySpawner.GetComponent<MerlionAcrBulletSpawner>();
        mSpawnCounter = mSpawnInterval;
    }

    public void Enter()
    {
        owner.mIsLaserAttack = true;
        owner.mImmune = true;
        //owner.mAnimator.SetBool("IsLaser", true);
    }

    public void Execute()
    {
        if(owner.transform.position.y > -5.0f && mLife > 0.0f)
        {
            float pos_y = owner.transform.position.y - Time.deltaTime * 5.0f;
            if (pos_y < -5.0f)
                pos_y = -5.0f;

            owner.transform.position = new Vector3(owner.transform.position.x, pos_y, owner.transform.position.z);
            return;
        }

        owner.mAnimator.SetBool("IsLaser", true);

        if (mStartDelay > 0.0f)
        {
            mStartDelay -= Time.deltaTime;
            return;
        }

        if (mLife == 5.0f)
            owner.mTailSpawner.GetComponent<MerlionTailSpawner>().Spawn();

        if (mLife > 0.0f)
        {
            mSpawnCounter += Time.deltaTime;

            if (mSpawnCounter >= mSpawnInterval)
            {
                mThrower.Spawn();
                mSpawnCounter = 0.0f;
            }

            mLife -= Time.deltaTime;
            return;
        }

        owner.mAnimator.SetBool("IsLaser", false);

        if (owner.transform.position.y < -0.2f)
        {
            float pos_y = owner.transform.position.y + Time.deltaTime * 5.0f;
            if (pos_y > -0.2f)
                pos_y = -0.2f;

            owner.transform.position = new Vector3(owner.transform.position.x, pos_y, owner.transform.position.z);
            return;
        }

        mEndDelay -= Time.deltaTime;
        if (mEndDelay <= 0.0f)
        {
            owner.mStateMachine.ChangeState(0);
        }
    }

    public void Exit()
    {
        owner.mIsLaserAttack = false;
        owner.mIdle = true;
        mStartDelay = 0.5f;
        mEndDelay = 0.25f;
        mLife = 5.0f;
        owner.mImmune = false;
    }
}

public class ML_HeadAttackState : IState
{
    MerlionBoss owner;
    MerlionHeadSpawner mThrower;
    float mStartDelay = 1.0f;
    float mEndDelay = 0.25f;

    float mLife = 6.0f;

    float mOriginalX = 5.6f;
    float mNewX = 13.0f;

    public ML_HeadAttackState(MerlionBoss owner)
    {
        this.owner = owner;
        mThrower = owner.mHeadSpawner.GetComponent<MerlionHeadSpawner>();
        owner.mImmune = true;
    }

    public void Enter()
    {
        owner.mIsLaserAttack = true;
    }

    public void Execute()
    {
        if (owner.transform.position.x < mNewX && mLife > 0.0f)
        {
            float pos_x = owner.transform.position.x + Time.deltaTime * 5.0f;
            if (pos_x >= mNewX)
                pos_x = mNewX;

            owner.transform.position = new Vector3(pos_x, owner.transform.position.y, owner.transform.position.z);
            return;
        }

        if (mStartDelay > 0.0f)
        {
            mStartDelay -= Time.deltaTime;
            return;
        }

        if (mLife == 6.0f)
            owner.mHeadSpawner.GetComponent<MerlionHeadSpawner>().Spawn();

        if (mLife > 0.0f)
        {
            mLife -= Time.deltaTime;
            return;
        }

        if (owner.transform.position.x > mOriginalX)
        {
            float pos_x = owner.transform.position.x - Time.deltaTime * 5.0f;
            if (pos_x <= mOriginalX)
                pos_x = mOriginalX;

            owner.transform.position = new Vector3(pos_x, owner.transform.position.y, owner.transform.position.z);
            return;
        }

        mEndDelay -= Time.deltaTime;
        if (mEndDelay <= 0.0f)
        {
            owner.mStateMachine.ChangeState(0);
        }
    }

    public void Exit()
    {
        owner.mIsLaserAttack = false;
        owner.mIdle = true;
        mStartDelay = 0.5f;
        mEndDelay = 0.25f;
        mLife = 6.0f;
        owner.mImmune = false;
    }
}

public class MerlionBoss : MonoBehaviour
{
    public enum ML_STATES
    {
        IDLE = 0,
        VOLLEY_ATTACK,
        SINGLE_ATTACK,
        TAIL_ATTACK,
        LASER_ATTACK,
        HEAD_ATTACK,
        END
    };

    public GameObject mLaser;
    public GameObject mLaserMouth;
    public GameObject mVolleySpawner;
    public GameObject mLaser2;
    public GameObject mPlayer;
    public GameObject mTailSpawner;
    public GameObject mHeadSpawner;
    public GameObject mSingleLaserPrefab;
    public StateMachine mStateMachine = new StateMachine();
    public Animator mAnimator;

    public float mAttackInterval;
    public float mAttackCounter = 0.0f;
    float mFlickerTime = 0.0f;

    public int mHp = 150;
    public int mAttackPattern = 0;
    
    public bool mIdle = true;
    public bool mImmune = false;
    public bool mIsLaserAttack = false;
    public bool mIsSingleShot = false;

    // Start is called before the first frame update
    void Start()
    {
        //we default to idle state
        mStateMachine.RegisterState(new ML_IdleState(this), (int)ML_STATES.IDLE);
        mStateMachine.RegisterState(new ML_VolleyAttackState(this), (int)ML_STATES.VOLLEY_ATTACK);
        mStateMachine.RegisterState(new ML_SingleBeamState(this), (int)ML_STATES.SINGLE_ATTACK);
        mStateMachine.RegisterState(new ML_TailAttackState(this), (int)ML_STATES.TAIL_ATTACK);
        mStateMachine.RegisterState(new ML_LaserAttackState(this), (int)ML_STATES.LASER_ATTACK);
        mStateMachine.RegisterState(new ML_HeadAttackState(this), (int)ML_STATES.HEAD_ATTACK);
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Update();

        if (mFlickerTime > 0.0f)
        {
            float alpha = GetComponent<SpriteRenderer>().color.a;

            if(alpha == 1.0f)
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            else
                GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            mFlickerTime -= Time.deltaTime;

            if (mFlickerTime <= 0.0f)
                mFlickerTime = 0.0f;
        }
        else
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

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
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack" || col.gameObject.tag == "Player" || col.gameObject.tag == "PlayerWater")
        {
            if (!mImmune)
            {
                mHp--;
                mFlickerTime = 0.5f;
            }
            if (col.gameObject.tag == "PlayerAttack")
            {
                Destroy(col.gameObject);
            }

            if (col.gameObject.tag == "PlayerWater")
            {
                col.gameObject.GetComponent<waterdropkillscript>().commitSudoku();
            }


        }
    }
}
