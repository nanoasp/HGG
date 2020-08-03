using System.Collections;
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
        P38WAYSPIN,
        P3TROLLEYBOMBARDMENT,
        DEAD

    };
    public GameObject trolleyPrefab;
    public GameObject myTrolley;
    public GameObject player;
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
        mStateMachine.RegisterState(new P1DASHFRONTState(this), (int)BOSSATTACKS.P1DASHFRONT);
        mStateMachine.RegisterState(new P2ARCBOMBARDMENTState(this), (int)BOSSATTACKS.P2ARCBOMBARDMENT);
        //mStateMachine.RegisterState(new P2SPINState(this), (int)BOSSATTACKS.P2SPIN); // bounce
        //mStateMachine.RegisterState(new P3SHOOTFRONTState(this), (int)BOSSATTACKS.P3SHOOTFRONT);
        //mStateMachine.RegisterState(new P3somethingState(this), (int)BOSSATTACKS.P3something); // bounce spawn trolley
        //mStateMachine.RegisterState(new P3TROLLEYBOMBARDMENTState(this), (int)BOSSATTACKS.P3TROLLEYBOMBARDMENT); fire trolley out and they bounce
        //mStateMachine.RegisterState(new DEADState(this), (int)BOSSATTACKS.DEAD);

        mStateMachine.ChangeState((int)BOSSATTACKS.IDLE);

        mTransform = GetComponent<Transform>();
        mRB2D = GetComponent<Rigidbody2D>();

        myTrolley = Instantiate(trolleyPrefab);
        myTrolley.transform.position = transform.position + (Vector3.left * 3);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Update();
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
            owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P1SHOOTFRONT);
        }
    }
    public void Exit()
    {

    }
}
public class P1SHOOTFRONTState : IState
{
    BoomerAunty owner;

    float delayBeforeShooting = 3.8f;
    float shotDelay = 0.8f;
    bool isShooting;
    float currtimer = 0.0f;
    float aligntimer = 0.0f;
    int shotCount;
    float phaseTimer = 0.0f;
    float moveTimer = 0.0f;
    List<float> playercollectionpos = new List<float>();
    float trackingTimer;
    Camera camera;
    bool moving;
    Vector3 newpos;
    public P1SHOOTFRONTState(BoomerAunty owner)
    {
        this.owner = owner;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void Enter()
    {
        owner.currentHealth = 100;
        isShooting = false;
        currtimer = 0.0f;
        playercollectionpos.Add(owner.player.transform.position.y);
        moving = false;
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
            // update numeric spring
            if (shotCount < 3)
            {
                // shoots a bullet
                owner.myTrolley.GetComponent<Trolleybehaviour>().ShootBullet();
                // TODO :: add recoil

                shotCount++;
                currtimer = 0;
            }
            else {
                moveTrolleytoDefultPos();
            }

        }


        // move closer to player
        moveTimer += Time.deltaTime;
        if (moveTimer > 1.2f && moving == false) {
            moving = true;
            float ypos = 0;
            for (var i = 0; i < playercollectionpos.Count; i++)
            {
                ypos += playercollectionpos[i];
            }
            ypos /= playercollectionpos.Count;
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.65f, 0.95f), 0, 0));// + new Vector3(0,ypos/2,0);

            newpos = new Vector3(p.x, ypos, 0);

        }
        if (moving) {
            owner.transform.position += (newpos - owner.transform.position) * Time.deltaTime;

            if (moveTimer > 2.2f)
            {
                moveTimer = 0.0f;
                moving = false;
            }
        }

        trackingTimer += Time.deltaTime;
        if (trackingTimer > 1.0f ){
            if (playercollectionpos.Count > 10) {
                playercollectionpos.RemoveAt(0);
            }
            playercollectionpos.Add(owner.player.transform.position.y);
            trackingTimer = 0;
        }
        phaseTimer += Time.deltaTime;
        if (owner.currentHealth < 0 || phaseTimer > 60.0f) {

            moveTrolleytoDefultPos();
            if (!isShooting && currtimer==0) { 
            owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P1DASHFRONT);
            }
        }
    }

    void moveTrolleytoFirePos() {
        Vector3 tspos = owner.transform.position + (Vector3.left * 3);
        Vector3 tepos = owner.transform.position + (Vector3.up * 4);


        // should be correct pls test
        aligntimer += Time.deltaTime;
        float mappedAligntimer = Easing.Cubic.Out(aligntimer);
        owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, mappedAligntimer);
        owner.myTrolley.transform.Rotate(new Vector3(0,0, (90 / (1.0f / Time.deltaTime))));
        if (aligntimer> 1.0f) {
            isShooting = true;
            currtimer = 0.0f;
            aligntimer = 0.0f;
        } 
    }
    void moveTrolleytoDefultPos()
    {
        Vector3 tepos = owner.transform.position + (Vector3.left*3);
        Vector3 tspos = owner.transform.position + (Vector3.up * 4);
        aligntimer += Time.deltaTime;
        // should be correct pls test
        float rotDiff = 0.0f + owner.myTrolley.transform.rotation.eulerAngles.z;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, -(rotDiff * Time.deltaTime)));
        //owner.myTrolley.transform.Rotate(new Vector3(0, 0, -(90 / (1.0f / Time.deltaTime))));
        if (aligntimer > 1.0f && owner.myTrolley.transform.rotation.eulerAngles.z < 0.05f && owner.myTrolley.transform.rotation.eulerAngles.z > -0.05f)
        {
            isShooting = false;
            currtimer = 0.0f;
            aligntimer = 0.0f;
            shotCount = 0;
        }
        else if(aligntimer <= 1.0f) {

            float mappedAligntimer = Easing.Cubic.Out(aligntimer);
            owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, mappedAligntimer);
        }
    }
    public void Exit()
    {

    }
}
public class P1DASHFRONTState : IState
{
    BoomerAunty owner;

    float delayBeforeDashing = 4.8f;
    bool isDashing;
    float currtimer = 0.0f;
    float phaseTimer = 0.0f;
    float moveTimer = 0.0f;
    List<float> playercollectionpos = new List<float>();
    float trackingTimer;
    Camera camera;
    bool reseting;
    Vector3 newpos;

    float dashSpeed;
    public P1DASHFRONTState(BoomerAunty owner)
    {
        this.owner = owner;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void Enter()
    {
        owner.currentHealth = 100;
        isDashing = false;
        currtimer = 0.0f;
        playercollectionpos.Add(owner.player.transform.position.y);
        reseting = false;
        dashSpeed = 18;
    }

    public void Execute()
    {
        currtimer += Time.deltaTime;
        // move closer to player
        moveTimer += Time.deltaTime;
        if (!isDashing && currtimer < delayBeforeDashing && reseting == false)
        {
            // starts shooting process
            //isShooting = true;
            moveTrolleytoFirePos();
        }
        else if (isDashing)
        {
            // dash forwards
            owner.transform.position += Vector3.left * dashSpeed * Time.deltaTime;
            owner.myTrolley.transform.position = owner.transform.position + (Vector3.left * 3);
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0), 0, 0));// + new Vector3(0,ypos/2,0);

            if (owner.transform.position.x < p.x)
            {
                isDashing = false;
                reseting = true;
            }
        }


        else if (moveTimer > 1.2f && reseting == false)
        {

            float ypos = 0;
            for (var i = 0; i < playercollectionpos.Count; i++)
            {
                ypos += playercollectionpos[i];
            }
            ypos /= playercollectionpos.Count;
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.55f, 0.95f), 0, 0));// + new Vector3(0,ypos/2,0);

            newpos = new Vector3(p.x, ypos, 0);

        }
        else if (!isDashing && reseting == false)
        {
            moveTrolleytoDefultPos();
            owner.transform.position += (newpos - owner.transform.position) * Time.deltaTime;
        }   

        trackingTimer += Time.deltaTime;
        if (trackingTimer > 0.8f)
        {
            if (playercollectionpos.Count > 3)
            {
                playercollectionpos.RemoveAt(0);
            }
            playercollectionpos.Add(owner.player.transform.position.y);
            trackingTimer = 0;
        }

        if (owner.currentHealth < 0 || phaseTimer > 60.0f)
        {
            moveTrolleytoDefultPos();
            if (isDashing && currtimer == 0 && reseting == false)
            {
                owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P2ARCBOMBARDMENT);
            }
        }
    }

    void moveTrolleytoFirePos()
    {
        float rotDiff = -45.0f - owner.myTrolley.transform.rotation.eulerAngles.z;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (rotDiff * Time.deltaTime)));

        if (owner.myTrolley.transform.rotation.eulerAngles.z < 0.05f && owner.myTrolley.transform.rotation.eulerAngles.z > -0.05f)
        {
            isDashing = true;
            currtimer = 0.0f;

        }
    }
    void moveTrolleytoDefultPos()
    {
        float rotDiff = -0.0f - owner.myTrolley.transform.rotation.eulerAngles.z;
        Vector3 targetDir = camera.ViewportToWorldPoint(new Vector3(0.8f, 0, 0)) - owner.transform.position;
        float distToTarget = targetDir.sqrMagnitude;
        targetDir.Normalize();  

        owner.myTrolley.transform.position += targetDir * Time.deltaTime;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (rotDiff * Time.deltaTime)));
        if (distToTarget < 0.5f)
        {
            isDashing = false;
            currtimer = 0.0f;
            reseting = false;
        }
    }
    public void Exit()
    {

    }
}

public class P2ARCBOMBARDMENTState : IState
{
    BoomerAunty owner;

    float delayBeforeShooting = 3.8f;
    float shotDelay = 2.2f;
    bool isShooting;
    float currtimer = 0.0f;
    float aligntimer = 0.0f;
    int shotCount;
    float phaseTimer = 0.0f;
    float moveTimer = 0.0f;
    List<float> playercollectionpos = new List<float>();
    float trackingTimer;
    Camera camera;
    bool moving;
    Vector3 newpos;
    public P2ARCBOMBARDMENTState(BoomerAunty owner)
    {
        this.owner = owner;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void Enter()
    {
        owner.currentHealth = 100;
        isShooting = false;
        currtimer = 0.0f;
        playercollectionpos.Add(owner.player.transform.position.y);
        moving = false;
    }

    public void Execute()
    {
        currtimer += Time.deltaTime;
        if (!isShooting && currtimer > delayBeforeShooting)
        {
            // starts shooting process
            //isShooting = true;
            moveTrolleytoFirePos();


        }
        else if (isShooting && currtimer > shotDelay)
        {
            // update numeric spring

                // shoots a bullet
                owner.myTrolley.GetComponent<Trolleybehaviour>().ShootBullet();// change to shoot arc bullet
                // TODO :: add recoil

                shotCount++;
            if (currtimer > shotDelay*2) { 
                moveTrolleytoDefultPos();

            }


        }


        // move closer to player
        moveTimer += Time.deltaTime;
        if (moveTimer > 1.2f && moving == false)
        {
            moving = true;
            float ypos = 0;
            for (var i = 0; i < playercollectionpos.Count; i++)
            {
                ypos += playercollectionpos[i];
            }
            ypos /= playercollectionpos.Count;
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.65f, 0.95f), 0, 0));// + new Vector3(0,ypos/2,0);

            newpos = new Vector3(p.x, ypos, 0);

        }
        if (moving)
        {
            owner.transform.position += (newpos - owner.transform.position) * Time.deltaTime;

            if (moveTimer > 2.2f)
            {
                moveTimer = 0.0f;
                moving = false;
            }
        }

        trackingTimer += Time.deltaTime;
        if (trackingTimer > 1.0f)
        {
            if (playercollectionpos.Count > 10)
            {
                playercollectionpos.RemoveAt(0);
            }
            playercollectionpos.Add(owner.player.transform.position.y);
            trackingTimer = 0;
        }
        phaseTimer += Time.deltaTime;
        if (owner.currentHealth < 0 || phaseTimer > 60.0f)
        {

            moveTrolleytoDefultPos();
            if (!isShooting && currtimer == 0)
            {
                owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P2SPIN);
            }
        }
    }

    void moveTrolleytoFirePos()
    {
        Vector3 tspos = owner.transform.position + (Vector3.left * 3);
        Vector3 tepos = owner.transform.position + (Vector3.up * 4);


        // should be correct pls test
        aligntimer += Time.deltaTime;
        float mappedAligntimer = Easing.Cubic.Out(aligntimer);
        owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, mappedAligntimer);
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (90 / (1.0f / Time.deltaTime))));
        if (aligntimer > 1.0f)
        {
            isShooting = true;
            currtimer = 0.0f;
            aligntimer = 0.0f;
        }
    }
    void moveTrolleytoDefultPos()
    {
        Vector3 tepos = owner.transform.position + (Vector3.left * 3);
        Vector3 tspos = owner.transform.position + (Vector3.up * 4);
        aligntimer += Time.deltaTime;
        // should be correct pls test
        float rotDiff = 0.0f + owner.myTrolley.transform.rotation.eulerAngles.z;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, -(rotDiff * Time.deltaTime)));
        //owner.myTrolley.transform.Rotate(new Vector3(0, 0, -(90 / (1.0f / Time.deltaTime))));
        if (aligntimer > 1.0f && owner.myTrolley.transform.rotation.eulerAngles.z < 0.05f && owner.myTrolley.transform.rotation.eulerAngles.z > -0.05f)
        {
            isShooting = false;
            currtimer = 0.0f;
            aligntimer = 0.0f;
            shotCount = 0;
        }
        else if (aligntimer <= 1.0f)
        {

            float mappedAligntimer = Easing.Cubic.Out(aligntimer);
            owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, mappedAligntimer);
        }
    }
    public void Exit()
    {

    }
}


