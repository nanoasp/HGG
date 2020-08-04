using System.Collections;
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
        //owner.MoveVFX.SetActive(true);
    }

    public void Execute()
    {
        //Debug.Log("updating player move state");
    }

    public void Exit()
    {
        //Debug.Log("exiting player move state");
        //owner.MoveVFX.SetActive(false);
    }
}

public class ChargeState : IState
{
    PlayerController2D owner;

    public ChargeState(PlayerController2D owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        //Debug.Log("entering player move state");
        //owner.MoveVFX.SetActive(true);
    }

    public void Execute()
    {
        //Debug.Log("updating player move state");
    }

    public void Exit()
    {
        //Debug.Log("exiting player move state");
        //owner.MoveVFX.SetActive(false);
    }
}

public class AttackState : IState
{
    PlayerController2D owner;
    GameObject AttackCollider;

    float offset = 1.5f;
    float timer = 0.3f;
    float delay = 0.2f;

    public AttackState(PlayerController2D owner)
    {
        this.owner = owner;
    }

    void Spawn()
    {
        //Debug.Log("entering player attack state");
        Vector3 Owner_right = owner.mFacingRight ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(-1.0f, 0.0f, 0.0f);

        AttackCollider = PlayerController2D.Instantiate(owner.ProjectilePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, -90.0f)) as GameObject;

        AttackCollider.transform.position = owner.transform.position + Owner_right * offset;
        AttackCollider.GetComponent<Rigidbody2D>().velocity = new Vector2 (Owner_right.x, Owner_right.y) * owner.mProjectileSpeed;
        
        //owner.playShoot();

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
        //owner.stopShoot();

    }
}



public class BeamAttackState : IState
{
    PlayerController2D owner;
    GameObject AttackCollider;
    Transform mAnikiOne;
    Transform mAnikiTwo;

    float timer = 0.3f;
    float delay = 0.00f;

    public BeamAttackState(PlayerController2D owner)
    {
        this.owner = owner;
        mAnikiOne = owner.mAnikiOne_obj.GetComponent<Transform>();
        mAnikiTwo = owner.mAnikiTwo_obj.GetComponent<Transform>();
    }

    void Spawn()
    {
        //Debug.Log("entering player attack state");
        Vector3 Owner_right = owner.mFacingRight ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(-1.0f, 0.0f, 0.0f);
        Vector2 velocity = new Vector2(Owner_right.x, Owner_right.y) * Mathf.SmoothStep(0.0f, owner.mBeamProjectileSpeed, owner.mBeamCounter / owner.mBeamMax);
        float size = 5.0f * (owner.mBeamCounter / owner.mBeamMax);
        float offset = owner.mAnikiState == PlayerController2D.ANIKI_STATES.FRONTAL ? 1.0f : 1.5f;

        size = size > 1.5f ? size : 1.5f;

        AttackCollider = PlayerController2D.Instantiate(owner.WaterProjectilePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0)) as GameObject;
        AttackCollider.transform.position = mAnikiOne.transform.position + Owner_right * offset;
        AttackCollider.GetComponent<Rigidbody2D>().velocity = velocity;
        AttackCollider.GetComponent<Transform>().localScale = new Vector3(size, size, 5.0f);

        AttackCollider = PlayerController2D.Instantiate(owner.WaterProjectilePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0)) as GameObject;
        AttackCollider.transform.position = mAnikiTwo.transform.position + Owner_right * offset;
        AttackCollider.GetComponent<Rigidbody2D>().velocity = velocity;
        AttackCollider.GetComponent<Transform>().localScale = new Vector3(size, size, 5.0f);
        //owner.playShoot();
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        //Debug.Log("updating player attack state");

        if (owner.mBeamCounter < 20.0f)
            delay = 0.5f;
        else
            delay = 0.0f;

        timer += Time.deltaTime;
        
        if (timer > delay)
        {
            Spawn();
            timer = 0.0f;
        }

        Spawn();
    }

    public void Exit()
    {
        //Debug.Log("exiting player attack state");
        //owner.stopShoot();

    }
}

public class PlayerController2D : MonoBehaviour
{
 
    //////////////////////////////////////////////////////
    ///                                                ///
    /// Player variables                               ///
    ///                                                ///
    //////////////////////////////////////////////////////
    public enum PLAYER_STATES
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        CHARGE,
        BEAM_ATTACK
    };

    public GameObject ProjectilePrefab;
    public GameObject WaterProjectilePrefab;
    public GameObject mMainCamera;

    public GameObject mMarkerOne_obj;
    public GameObject mMarkerTwo_obj;

    AnikiMarker mMarkerOne;
    AnikiMarker mMarkerTwo;

    public GameObject mAnikiOne_obj;
    public GameObject mAnikiTwo_obj;

    Aniki mAnikiOne;
    Aniki mAnikiTwo;

    public float mHorizontalMoveSpeed = 5.0f;
    public float mVerticalMoveSpeed = 5.0f;

    public float mHorizontalAttackMoveSpeed = 5.0f;
    public float mVerticalAttackMoveSpeed = 5.0f;

    public float mHorizontalChargeMoveSpeed = 2.5f;
    public float mVerticalChargeMoveSpeed = 2.5f;

    public float mHorizontalBeamMoveSpeed = 5.0f;
    public float mVerticalBeamMoveSpeed = 5.0f;

    public float mProjectileSpeed = 8.0f;

    public const float mGravityScale = 0.0f;

    public bool mFacingRight = true;
    
    int mHorizontal = 0;
    int mVertical = 0;

    Rigidbody2D mRB2D;
    Collider2D mCollider;
    Transform mTransform;
    Animator mAnimator;

    StateMachine mStateMachine =  new StateMachine();
    
    //////////////////////////////////////////////////////
    ///                                                ///
    /// Beam variables                                 ///
    ///                                                ///
    //////////////////////////////////////////////////////
 
    public float mBeamChargeRate = 5.0f;
    public float mBeamDepletionRate = 5.5f;
    public float mBeamMax = 100.0f;
    public float mBeamCounter = 0.0f;
    public float mBeamThreshold = 30.0f;
    public float mBeamProjectileSpeed = 20.0f;

    //////////////////////////////////////////////////////
    ///                                                ///
    /// Aniki variables                                ///
    ///                                                ///
    //////////////////////////////////////////////////////

    public enum ANIKI_STATES
    {
        SANDWICH = 0,
        FRONTAL,
    };

    public ANIKI_STATES mAnikiState = ANIKI_STATES.SANDWICH;
    float mChangeTimer = 0.0f;
    public float mChangeDelay = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        //we default to idle state
        mStateMachine.RegisterState(new IdleState(this), (int)PLAYER_STATES.IDLE);
        mStateMachine.RegisterState(new MoveState(this), (int)PLAYER_STATES.MOVE);
        mStateMachine.RegisterState(new AttackState(this), (int)PLAYER_STATES.ATTACK);
        mStateMachine.RegisterState(new ChargeState(this), (int)PLAYER_STATES.CHARGE);
        mStateMachine.RegisterState(new BeamAttackState(this), (int)PLAYER_STATES.BEAM_ATTACK);

        mStateMachine.ChangeState((int)PLAYER_STATES.IDLE);

        mTransform = GetComponent<Transform>();
        mRB2D = GetComponent<Rigidbody2D>();
        mRB2D.gravityScale = mGravityScale;
        mFacingRight = mTransform.localScale.x >= 0;

        //mAnimator = this.GetComponent<Animator>();

        mMarkerOne = mMarkerOne_obj.GetComponent<AnikiMarker>();
        mMarkerTwo = mMarkerTwo_obj.GetComponent<AnikiMarker>();
        mAnikiOne = mAnikiOne_obj.GetComponent<Aniki>();
        mAnikiTwo = mAnikiTwo_obj.GetComponent<Aniki>();
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Update();

        //character movement
        {
            //left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                mHorizontal = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))//right
            {
                mHorizontal = 1;
            }
            else//not moving left or right
            {
                mHorizontal = 0;
            }

            //up
            if (Input.GetKey(KeyCode.UpArrow))
            {
                mVertical = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))//down
            {
                mVertical = -1;
            }
            else//not moving up or down
            {
                mVertical = 0;
            }
        }

        //check for attack
        if (mBeamCounter > 0.0f && (mBeamCounter >= mBeamThreshold || (PLAYER_STATES)mStateMachine.GetStateNumber() == PLAYER_STATES.BEAM_ATTACK) && Input.GetKey(KeyCode.Space))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.BEAM_ATTACK);
            //mAnimator.SetInteger("state", (int)PLAYER_STATES.BEAM_ATTACK);
        }
        else if (mChangeTimer <= 0.0f && Input.GetKey(KeyCode.X))
        {
            mChangeTimer = mChangeDelay;
            mAnikiState = mAnikiState == ANIKI_STATES.FRONTAL ? ANIKI_STATES.SANDWICH : ANIKI_STATES.FRONTAL;

            mMarkerOne.Toggle();
            mMarkerTwo.Toggle();
            mAnikiOne.Toggle();
            mAnikiTwo.Toggle();
        }
        else if(Input.GetKey(KeyCode.C))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.CHARGE);
            //mAnimator.SetInteger("state", (int)PLAYER_STATES.CHARGE);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            mStateMachine.ChangeState((int)PLAYER_STATES.ATTACK);
            //mAnimator.SetInteger("state", (int)PLAYER_STATES.ATTACK);
        }
        else
        {
            if(mHorizontal == 0 && mVertical == 0)
            {
                mStateMachine.ChangeState((int)PLAYER_STATES.IDLE);
                //mAnimator.SetInteger("state", (int)PLAYER_STATES.IDLE);
            }
            else
            {
                mStateMachine.ChangeState((int)PLAYER_STATES.MOVE);
                //mAnimator.SetInteger("state", (int)PLAYER_STATES.MOVE);
            }
        }

        switch ((PLAYER_STATES)(mStateMachine.GetStateNumber()))
        {
            case PLAYER_STATES.BEAM_ATTACK:
                mBeamCounter -= mBeamDepletionRate * Time.deltaTime;

                if (mBeamCounter < 0.0f)
                    mBeamCounter = 0.0f;
                break;
            case PLAYER_STATES.CHARGE:
                mBeamCounter += mBeamChargeRate * Time.deltaTime;
                
                if (mBeamCounter > mBeamMax)
                    mBeamCounter = mBeamMax;

                break;
            default:
                break;
        }

        mChangeTimer -= Time.deltaTime;

        if (mChangeTimer <= 0.0f)
            mChangeTimer = 0.0f;
    }

    void FixedUpdate()
    {
        // Apply movement velocity
        float movespd_x, movespd_y;

        switch((PLAYER_STATES)mStateMachine.GetStateNumber())
        {
            default:
            case PLAYER_STATES.MOVE:
                movespd_x = mHorizontalMoveSpeed;
                movespd_y = mVerticalMoveSpeed;
                break;
            case PLAYER_STATES.ATTACK:
                movespd_x = mHorizontalAttackMoveSpeed;
                movespd_y = mVerticalAttackMoveSpeed;
                break;
            case PLAYER_STATES.BEAM_ATTACK:
                movespd_x = mHorizontalBeamMoveSpeed;
                movespd_y = mVerticalBeamMoveSpeed;
                break;
            case PLAYER_STATES.CHARGE:
                movespd_x = mHorizontalChargeMoveSpeed;
                movespd_y = mVerticalChargeMoveSpeed;
                break;
        }

        mRB2D.velocity = new Vector2((mHorizontal) * movespd_x, (mVertical) * movespd_y);
    }

    //public void playRoll()
    //{
    //    FindObjectOfType<AudioManager>().Play("Roll");
    //
    //}

}
