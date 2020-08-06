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
    SpriteRenderer sr;

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
        //mStateMachine.RegisterState(new P3somethingState(this), (int)BOSSATTACKS.P3something); // bounce spawn trolley //  cancled
        //mStateMachine.RegisterState(new P3TROLLEYBOMBARDMENTState(this), (int)BOSSATTACKS.P3TROLLEYBOMBARDMENT); fire trolley out and they bounce // cancled
        mStateMachine.RegisterState(new DEADState(this), (int)BOSSATTACKS.DEAD);

        mStateMachine.ChangeState((int)BOSSATTACKS.IDLE);

        mTransform = GetComponent<Transform>();
        mRB2D = GetComponent<Rigidbody2D>();

        myTrolley = Instantiate(trolleyPrefab);
        myTrolley.transform.position = transform.position + (Vector3.left * 3);
        player = GameObject.Find("Player");
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        mStateMachine.Update();
        sr.color = new Color(1f, 1f, 1f, 1.0f);

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            currentHealth--;
            sr.color = new Color(1f, 1f, 1f, .5f);
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "PlayerWater")
        {
            col.gameObject.GetComponent<waterdropkillscript>().commitSudoku();
        }

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
        if (currtimer > 1.2f) //  change to dialogue finish
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
    float shotDelay = 4.2f;
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
    // numeric spring for recoil
    float spriPos;
    float spriVel;
    float recoilTargetpos;
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

            if (shotCount < 3)
            {
                // shoots a bullet
                owner.myTrolley.GetComponent<Trolleybehaviour>().ShootPri();
                // TODO :: add recoil
                spriVel = 20.0f;
                shotCount++;
                currtimer = 0;
            }
            else {
                moveTrolleytoDefultPos();
            }

        }
        if (isShooting && shotCount < 3 && currtimer <1.0f) {
            // update numeric spring
            Vector3 currpos = owner.myTrolley.transform.position;
            spriPos = currpos.x;
            NumbericSpring.spring(ref spriPos, ref spriVel, recoilTargetpos, 0.20f, 12, Time.deltaTime);
            owner.myTrolley.transform.position = new Vector3(spriPos, currpos.y, currpos.z);
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
        if (owner.currentHealth < 0 || phaseTimer > 150.0f) {

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
            recoilTargetpos = owner.transform.position.x;
            spriVel = 0.0f;

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
    float currtimer = 0.0f;
    float phaseTimer = 0.0f;
    float moveTimer = 0.0f;
    List<float> playercollectionpos = new List<float>();
    float trackingTimer;
    Camera camera;
    Vector3 newpos;

    float dashSpeed;

    enum state { 
        preDash,
        Dash,
        reseting
    
    }
    state currstate;
    public P1DASHFRONTState(BoomerAunty owner)
    {
        this.owner = owner;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void Enter()
    {
        owner.currentHealth = 100;
        currtimer = 0.0f;
        playercollectionpos.Add(owner.player.transform.position.y);
        dashSpeed = 18; 

        currstate = state.reseting;
    }

    public void Execute()
    {
        currtimer += Time.deltaTime;
        phaseTimer += Time.deltaTime;
        // move closer to player
        moveTimer += Time.deltaTime;


        if (currstate == state.reseting)
        {
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.8f, 0.95f), 0.5f, 0));// + new Vector3(0,ypos/2,0);
            newpos = new Vector3(p.x, p.y, 0);
            owner.transform.position += (newpos - owner.transform.position) * Time.deltaTime;
            moveTrolleytoDefultPos();

            playercollectionpos.Clear();
            playercollectionpos.Add(owner.player.transform.position.y);
            if (currtimer > 3.8f) {
                currstate = state.preDash;
                currtimer = 0.0f;
            }
        }
        else if (currstate == state.preDash)
        {
            float ypos = 0;
            for (var i = 0; i < playercollectionpos.Count; i++)
            {
                ypos += playercollectionpos[i];
            }
            ypos /= playercollectionpos.Count;
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.75f, 0.95f), 0, 0));// + new Vector3(0,ypos/2,0);
            newpos = new Vector3(p.x, ypos, 0);

            moveTrolleytoFirePos();
            owner.transform.position += (newpos - owner.transform.position) * Time.deltaTime;
            if (currtimer > delayBeforeDashing)
            {
                currstate = state.Dash;
                currtimer = 0.0f;
            }
        }
        else if (currstate == state.Dash)
        {
            // dash forwards
            owner.transform.position += Vector3.left * dashSpeed * Time.deltaTime;
            owner.myTrolley.transform.position = owner.transform.position + (Vector3.left * 3);
            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.08f, 0.1f), 0, 0));// + new Vector3(0,ypos/2,0);
                
            if (owner.transform.position.x < p.x)
            {
                currstate = state.reseting;
                currtimer = 0.0f;

            }
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
        if (owner.currentHealth < 0 || phaseTimer > 150.0f)
        {
            moveTrolleytoDefultPos();
            if (currstate == state.preDash)
            {
                owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P2ARCBOMBARDMENT);
            }
        }
        /*
        if (!isDashing && reseting == false)
        {
            // starts shooting process
            //isShooting = true;


            if (currtimer < delayBeforeDashing)
            {
                moveTrolleytoFirePos();

            }
            else { 
                isDashing = true;
                currtimer = 0;
            }

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
        else if (!isDashing && reseting == true)
        {
            moveTrolleytoDefultPos();
            owner.transform.position += (newpos - owner.transform.position) * Time.deltaTime;
        }   

        



        */
    }

    void moveTrolleytoFirePos()
    {
        float rotDiff = 315.0f - owner.myTrolley.transform.rotation.eulerAngles.z;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (rotDiff * Time.deltaTime)));
        owner.myTrolley.transform.position = owner.transform.position + Vector3.left * 3;
        if (owner.myTrolley.transform.rotation.eulerAngles.z < 315.05f && owner.myTrolley.transform.rotation.eulerAngles.z > 314.05f)
        {
            currstate = state.preDash; //  change state
            currtimer = 0;
        }
    }
    void moveTrolleytoDefultPos()
    {
        float rotDiff = 0.0f - owner.myTrolley.transform.rotation.eulerAngles.z;
        Vector3 targetDir = (newpos + Vector3.left *3) - owner.myTrolley.transform.position ;
        float distToTarget = targetDir.sqrMagnitude;
        //targetDir.Normalize();  

        owner.myTrolley.transform.position += targetDir * Time.deltaTime;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (rotDiff * Time.deltaTime)));
        if (distToTarget < 0.5f && owner.myTrolley.transform.rotation.eulerAngles.z < 0.05f && owner.myTrolley.transform.rotation.eulerAngles.z > -0.05f )
        {
            currstate = state.preDash; //  change state
            currtimer = 0.0f;
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
    float shotDelay = 4.2f;
    bool isShooting;
    float currtimer = 0.0f;
    float aligntimer = 0.0f;
    float phaseTimer = 0.0f;
    float moveTimer = 0.0f;
    List<float> playercollectionpos = new List<float>();
    float trackingTimer;
    Camera camera;
    bool moving;
    Vector3 newpos;
    // numeric spring for recoil
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
            isShooting = true;
        }
        else {
            moveTrolleytoFirePos();
        }
        if (isShooting)
        {
            if (currtimer > shotDelay) { 
                // shoots a bullet
                owner.myTrolley.GetComponent<Trolleybehaviour>().ShootArc(); //  change to arc bullet
            }


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
        if (owner.currentHealth < 0 || phaseTimer > 150.0f)
        {

            moveTrolleytoDefultPos();
            if (!isShooting && currtimer == 0)
            {
                owner.mStateMachine.ChangeState((int)BoomerAunty.BOSSATTACKS.P1DASHFRONT);
            }
        }
    }

    void moveTrolleytoFirePos()
    {
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(0.8f, 0.2f, 0));// + new Vector3(0,ypos/2,0);

        Vector3 tspos = owner.transform.position + (Vector3.left * 3);
        Vector3 tepos = new Vector3(p.x,p.y,0);
        float dist = (owner.transform.position - tepos).magnitude;

        // should be correct pls test
        aligntimer += Time.deltaTime;
        float mappedAligntimer = Easing.Cubic.Out(aligntimer);
        owner.myTrolley.transform.position = Vector3.Lerp(tspos, tepos, mappedAligntimer);


        float rotDiff = 0.0f - owner.myTrolley.transform.rotation.eulerAngles.z;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (rotDiff * Time.deltaTime)));
        
        if (dist < 0.5f && owner.myTrolley.transform.rotation.eulerAngles.z < 0.05f && owner.myTrolley.transform.rotation.eulerAngles.z > -0.05f)
        {
            isShooting = true;
            currtimer = 0.0f;
            aligntimer = 0.0f;
        }
    }
    void moveTrolleytoDefultPos()
    {
        Vector3 targetDir = (newpos + Vector3.left * 3) - owner.myTrolley.transform.position;
        float distToTarget = targetDir.sqrMagnitude;
        //targetDir.Normalize();  

        owner.myTrolley.transform.position += targetDir * Time.deltaTime;
        // should be correct pls test

        float rotDiff = 0.0f - owner.myTrolley.transform.rotation.eulerAngles.z;
        owner.myTrolley.transform.Rotate(new Vector3(0, 0, (rotDiff * Time.deltaTime)));

        if (owner.myTrolley.transform.rotation.eulerAngles.z < 0.05f && owner.myTrolley.transform.rotation.eulerAngles.z > -0.05f)
        {

            isShooting = false;
            currtimer = 0.0f;
            aligntimer = 0.0f;
        }

    }
    public void Exit()
    {

    }
}


public class DEADState : IState
{
    BoomerAunty owner;

    float delayBeforeShooting = 3.8f;
    float shotDelay = 6.2f;
    bool isShooting;
    float currtimer = 0.0f;
    float aligntimer = 0.0f;
    float phaseTimer = 0.0f;
    float moveTimer = 0.0f;
    List<float> playercollectionpos = new List<float>();
    float trackingTimer;
    Camera camera;
    bool moving;
    Vector3 newpos;
    // numeric spring for recoil
    public DEADState(BoomerAunty owner)
    {
        this.owner = owner;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void Enter()
    {

    }

    public void Execute()
    {
        // do death feed back

    }

    
    public void Exit()
    {
        // call end game
    }
}


