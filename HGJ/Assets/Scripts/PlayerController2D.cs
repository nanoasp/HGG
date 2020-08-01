/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://sharpcoderblog.com/blog/2d-platformer-character-controller
public class IdleState : IState
{
    PlayerController2D owner;
    
    public IdleState(PlayerController2D owner) 
    { 
        this.owner = owner; 
    }
    
    public void Enter()
    {
        //Debug.Log("entering player idle state");
    }
    
    public void Execute()
    {
        //Debug.Log("updating player idle state");
    }
    
    public void Exit()
    {
        //Debug.Log("exiting player idle state");
    }
}

public class MoveState : IState
{
    PlayerController2D owner;

    public MoveState(PlayerController2D owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        //Debug.Log("entering player move state");
        owner.MoveVFX.SetActive(true);
    }

    public void Execute()
    {
        //Debug.Log("updating player move state");
    }

    public void Exit()
    {
        //Debug.Log("exiting player move state");
        owner.MoveVFX.SetActive(false);
    }
}

public class AttackState : IState
{
    PlayerController2D owner;
    GameObject AttackCollider;

    float offset = 1.5f;

    public AttackState(PlayerController2D owner)
    {
        this.owner = owner;
    }

    void Spawn()
    {
        //Debug.Log("entering player attack state");
        Vector3 Owner_right = owner.mFacingRight ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(-1.0f, 0.0f, 0.0f);

        if (owner.mFacingRight)
            AttackCollider = PlayerController2D.Instantiate(owner.RightSmashPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        else
            AttackCollider = PlayerController2D.Instantiate(owner.LeftSmashPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;

        AttackCollider.transform.position = owner.transform.position + Owner_right * offset + new Vector3(0.0f, -1.0f, 0.0f);

        if(owner.mMainCamera != null)
            owner.mMainCamera.GetComponent<MainCamera>().Shake(2.0f, 0.1f);

        owner.isSmashing = true;
    }
    public void Enter()
    {
        owner.playSlam();
        Spawn();
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        //Debug.Log("exiting player attack state");
        //PlayerController2D.Destroy(AttackCollider, 0.0f);
        //owner.stopSlam();
        owner.isSmashing = false;
    }
}

public class RollingState : IState
{
    PlayerController2D owner;
    GameObject AttackCollider;

    public RollingState(PlayerController2D owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        //Debug.Log("entering player attack state");
        Vector3 Owner_right = owner.mFacingRight ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(-1.0f, 0.0f, 0.0f);

        AttackCollider = PlayerController2D.Instantiate(owner.RollingPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        AttackCollider.transform.parent = owner.transform;
        AttackCollider.transform.localPosition = new Vector3(0.0f, 0.1f, 0.0f);
        owner.isRolling = true;
        owner.RollingVFX.SetActive(true);
        owner.playRoll();

    }

    public void Execute()
    {
        //Debug.Log("updating player attack state");
        owner.mRollingCounter -= Time.deltaTime;
        owner.mRollingCounter = Mathf.Max(owner.mRollingCounter, 0.0f);
    }

    public void Exit()
    {
        //Debug.Log("exiting player attack state");
        PlayerController2D.Destroy(AttackCollider, 0.0f);
        owner.isRolling = false;
        owner.RollingVFX.SetActive(false);
        owner.stopRoll();


    }
}

public class WhirlwindState : IState
{
    PlayerController2D owner;
    GameObject AttackCollider;

    public WhirlwindState(PlayerController2D owner)
    {
        this.owner = owner;

    }

    public void Enter()
    {
        //Debug.Log("entering player attack state");
        Vector3 Owner_right = owner.mFacingRight ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(-1.0f, 0.0f, 0.0f);

        AttackCollider = PlayerController2D.Instantiate(owner.WhirlwindPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        AttackCollider.transform.parent = owner.transform;
        AttackCollider.transform.localPosition = new Vector3(0.0f, 0.25f, 0.0f);

        owner.isWhirlwind = true;
        owner.WhirlwindVFX.SetActive(true);
        owner.playSpin();

    }

    public void Execute()
    {
        //Debug.Log("updating player attack state");
        owner.mWhirlwindCounter -= Time.deltaTime;
        owner.mWhirlwindCounter = Mathf.Max(owner.mWhirlwindCounter, 0.0f);
    }

    public void Exit()
    {
        //Debug.Log("exiting player attack state");
        PlayerController2D.Destroy(AttackCollider, 0.0f);
        owner.isWhirlwind = false;
        owner.WhirlwindVFX.SetActive(false);
        owner.stopSpin();

    }
}

public class ThrowState : IState
{
    PlayerController2D owner;
    GameObject AttackCollider;

    float offset = 1.5f;
    float timer = 0.6f;
    float delay = 0.5f;

    public ThrowState(PlayerController2D owner)
    {
        this.owner = owner;
    }

    void Spawn()
    {
        //Debug.Log("entering player attack state");
        Vector3 Owner_right = owner.mFacingRight ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(-1.0f, 0.0f, 0.0f);

        if (owner.mFacingRight)
            AttackCollider = PlayerController2D.Instantiate(owner.RightProjectile, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, -90.0f)) as GameObject;
        else
            AttackCollider = PlayerController2D.Instantiate(owner.LeftProjectile, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 90.0f)) as GameObject;

        AttackCollider.transform.position = owner.transform.position + Owner_right * offset;
        AttackCollider.GetComponent<Rigidbody2D>().velocity = new Vector2 (Owner_right.x, Owner_right.y) * owner.mProjectileSpeed;
        
        owner.isThrowing = true;
        owner.playShoot();

    }
    public void Enter()
    {

    }

    public void Execute()
    {
        //Debug.Log("updating player attack state");

        timer += Time.deltaTime;

        if (timer > delay)
        {
            Spawn();
            timer = 0.0f;
        }
    }

    public void Exit()
    {
        //Debug.Log("exiting player attack state");
        owner.isThrowing = false;
        //owner.stopShoot();

    }
}

public class PlayerController2D : MonoBehaviour
{
    public enum PLAYER_STATES
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        ROLLING,
        WHIRLWIND,
        THROW
    };

    public GameObject LeftSmashPrefab;
    public GameObject RightSmashPrefab;
    public GameObject RollingPrefab;
    public GameObject WhirlwindPrefab;
    public GameObject mMainCamera;
    public GameObject LeftProjectile;
    public GameObject RightProjectile;
    public GameObject MoveVFX;
    public GameObject RollingVFX;
    public GameObject WhirlwindVFX;

    public float mHorizontalMoveSpeed = 5.0f;
    public float mVerticalMoveSpeed = 5.0f;
    
    public float mVerticalSmashMoveSpeed = 5.0f;
    public float mVerticalRollingMoveSpeed = 5.0f;
    public float mVerticalWhirlwindMoveSpeed = 5.0f;

    public float mHorizontalSmashMoveSpeed = 5.0f;
    public float mHorizontalRollingMoveSpeed = 5.0f;
    public float mHorizontalWhirlwindMoveSpeed = 5.0f;

    public float mProjectileSpeed = 8.0f;

    public const float mGravityScale = 0.0f;

    public bool mFacingRight = true;

    public bool isSmashing = false;
    public bool isRolling = false;
    public bool isWhirlwind = false;
    public bool isThrowing = false;

    public float mRollingCD = 2.0f;
    public float mWhirlwindCD = 2.0f;
    public float mSmashCD = 0.5f;

    public float mRollingCounter = 0.0f;
    public float mWhirlwindCounter = 0.0f;
    public float mSmashCounter = 0.0f;
    
    int mDirection = 0;
    int mVertical = 0;

    Rigidbody2D mRB2D;
    Collider2D mCollider;
    Transform mTransform;
    Animator mAnimator;

    StateMachine mStateMachine =  new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        //we default to idle state
        mStateMachine.RegisterState(new IdleState(this), (int)PLAYER_STATES.IDLE);
        mStateMachine.RegisterState(new MoveState(this), (int)PLAYER_STATES.MOVE);
        mStateMachine.RegisterState(new AttackState(this), (int)PLAYER_STATES.ATTACK);
        mStateMachine.RegisterState(new RollingState(this), (int)PLAYER_STATES.ROLLING);
        mStateMachine.RegisterState(new WhirlwindState(this), (int)PLAYER_STATES.WHIRLWIND);
        mStateMachine.RegisterState(new ThrowState(this), (int)PLAYER_STATES.THROW);

        mStateMachine.ChangeState((int)PLAYER_STATES.IDLE);

        mTransform = GetComponent<Transform>();
        mRB2D = GetComponent<Rigidbody2D>();
        mRB2D.gravityScale = mGravityScale;
        mFacingRight = mTransform.localScale.x > 0;

        mAnimator = this.GetComponent<Animator>();

        mRollingCounter = mRollingCD;
        mWhirlwindCounter = mWhirlwindCD;
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Update();

        //left
        if(Input.GetKey(KeyCode.A))
        {
            mDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))//right
        {
            mDirection = 1;
        }
        else//not moving left or right
        {
            mDirection = 0;
        }

        //up
        if (Input.GetKey(KeyCode.W))
        {
            mVertical = 1;
        }
        else if (Input.GetKey(KeyCode.S))//down
        {
            mVertical = -1;
        }
        else//not moving up or down
        {
            mVertical = 0;
        }

        //reading an input
        if (mDirection != 0)
        {
            //going right but not facing right
            if(mDirection > 0  && !mFacingRight)
            {
                mFacingRight = true;
                mTransform.localScale = new Vector3(Mathf.Abs(mTransform.localScale.x), mTransform.localScale.y, mTransform.localScale.z);
            }
            else if (mDirection < 0 && mFacingRight)//going left but not facing left
            {
                mFacingRight = false;
                mTransform.localScale = new Vector3(-Mathf.Abs(mTransform.localScale.x), mTransform.localScale.y, mTransform.localScale.z);
            }
        }

        //check for attack
        if(mWhirlwindCounter > 0.0f && (mWhirlwindCounter > 0.6f || isWhirlwind) && (Input.GetKey(KeyCode.Space)))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.WHIRLWIND);
            mAnimator.SetInteger("state", (int)PLAYER_STATES.WHIRLWIND);
        }
        else if (mRollingCounter > 0.1f && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.ROLLING);
            mAnimator.SetInteger("state", (int)PLAYER_STATES.ROLLING);
        }
        else if(mRollingCounter == mRollingCD && Input.GetMouseButtonDown(0))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.ATTACK);
            mAnimator.SetInteger("state", (int)PLAYER_STATES.ATTACK);
        }
        else if (Input.GetMouseButton(1))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.THROW);
            mAnimator.SetInteger("state", (int)PLAYER_STATES.WHIRLWIND);
        }
        else
        {
            if(mDirection == 0 && mVertical == 0)
            {
                mStateMachine.ChangeState((int)PLAYER_STATES.IDLE);
                mAnimator.SetInteger("state", (int)PLAYER_STATES.IDLE);
            }
            else
            {
                mStateMachine.ChangeState((int)PLAYER_STATES.MOVE);
                mAnimator.SetInteger("state", (int)PLAYER_STATES.MOVE);
            }
        }

        if (!isWhirlwind)
        { 
            mWhirlwindCounter += Time.deltaTime / 2;
            mWhirlwindCounter = Mathf.Min(mWhirlwindCounter, mWhirlwindCD);
        }

        if (!isRolling)
        {
            mRollingCounter += Time.deltaTime / 2;
            mRollingCounter = Mathf.Min(mRollingCounter, mRollingCD);
        }

        if (!isSmashing)
        {
            mSmashCounter += Time.deltaTime;
            mSmashCounter = Mathf.Min(mSmashCounter, mSmashCD);
        }
    }

    void FixedUpdate()
    {
        //Bounds colliderBounds = mCollider.bounds;
        //Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0.1f, 0);
        //
        //// Check if player is grounded
        ////mIsGrounded = Physics2D.OverlapCircle(groundCheckPos, 0.23f, layerMask);

        // Apply movement velocity

        float movespd_x, movespd_y;

        if (isSmashing)
        {
            movespd_x = mHorizontalSmashMoveSpeed;
            movespd_y = mVerticalSmashMoveSpeed;
        }
        else if (isRolling)
        {
            movespd_x = mHorizontalRollingMoveSpeed;
            movespd_y = mVerticalRollingMoveSpeed;
        }
        else if (isWhirlwind)
        {
            movespd_x = mHorizontalWhirlwindMoveSpeed;
            movespd_y = mVerticalWhirlwindMoveSpeed;
        }
        else
        {
            movespd_x = mHorizontalMoveSpeed;
            movespd_y = mVerticalMoveSpeed;
        }

        mRB2D.velocity = new Vector2((mDirection) * movespd_x, (mVertical) * movespd_y);
    }





    public void playRoll()
    {
        FindObjectOfType<AudioManager>().Play("Roll");

    }
    public void stopRoll()
    {
        FindObjectOfType<AudioManager>().Stop("Roll");

    }
    public void playSpin()
    {
        FindObjectOfType<AudioManager>().Play("Spin");

    }
    public void stopSpin()
    {
        FindObjectOfType<AudioManager>().Stop("Spin");

    }

    public void playShoot()
    {
        FindObjectOfType<AudioManager>().Play("Shoot");

    }
    public void stopShoot()
    {
        FindObjectOfType<AudioManager>().Stop("Shoot");

    }

    public void playSlam()
    {
        FindObjectOfType<AudioManager>().Play("Slam");

    }
    public void stopSlam()
    {
        FindObjectOfType<AudioManager>().Stop("Slam");

    }






}
*/