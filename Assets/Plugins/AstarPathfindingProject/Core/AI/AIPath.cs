//#define ASTARDEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using System;

/** AI for following paths.
 * This AI is the default movement script which comes with the A* Pathfinding Project.
 * It is in no way required by the rest of the system, so feel free to write your own. But I hope this script will make it easier
 * to set up movement for the characters in your game. This script is not written for high performance, so I do not recommend using it for large groups of units.
 * \n
 * \n
 * This script will try to follow a target transform, in regular intervals, the path to that target will be recalculated.
 * It will on FixedUpdate try to move towards the next point in the path.
 * However it will only move in the forward direction, but it will rotate around it's Y-axis
 * to make it reach the target.
 * 
 * \section variables Quick overview of the variables
 * In the inspector in Unity, you will see a bunch of variables. You can view detailed information further down, but here's a quick overview.\n
 * The #repathRate determines how often it will search for new paths, if you have fast moving targets, you might want to set it to a lower value.\n
 * The #target variable is where the AI will try to move, it can be a point on the ground where the player has clicked in an RTS for example.
 * Or it can be the player object in a zombie game.\n
 * The speed is self-explanatory, so is turningSpeed, however #slowdownDistance might require some explanation.
 * It is the approximate distance from the target where the AI will start to slow down. Note that this doesn't only affect the end point of the path
 * but also any intermediate points, so be sure to set #forwardLook and #pickNextWaypointDist to a higher value than this.\n
 * #pickNextWaypointDist is simply determines within what range it will switch to target the next waypoint in the path.\n
 * #forwardLook will try to calculate an interpolated target point on the current segment in the path so that it has a distance of #forwardLook from the AI\n
 * Below is an image illustrating several variables as well as some internal ones, but which are relevant for understanding how it works.
 * Note that the #forwardLook range will not match up exactly with the target point practically, even though that's the goal.
 * \shadowimage{aipath_variables.png}
 * This script has many movement fallbacks.
 * If it finds a NavmeshController, it will use that, otherwise it will look for a character controller, then for a rigidbody and if it hasn't been able to find any
 * it will use Transform.Translate which is guaranteed to always work.
 */
[RequireComponent(typeof(Seeker))]
[AddComponentMenu("Pathfinding/AI/AIPath (generic)")]
public class AIPath : MonoBehaviour
{

    /** Determines how often it will search for new paths. 
     * If you have fast moving targets or AIs, you might want to set it to a lower value.
     * The value is in seconds between path requests.
     */

    public static bool IsOk = false;

    public static bool IsNav = false;
    public float repathRate = 0.5F;

    /** Target to move towards.
     * The AI will try to follow/move towards this target.
     * It can be a point on the ground where the player has clicked in an RTS for example, or it can be the player object in a zombie game.
     */
    protected static Transform target;      //目标物体
    protected Transform targetPoints;
    protected Transform target1;
    public GUIStyle myStyle;
    protected int step;
    private Transform people;       //用于获得游戏中的人物
    private int Flash_Botton;       //用于提示button的闪烁
    private float num;          //用于计算偏转角度
    private Transform text;

    /** Enables or disables searching for paths.
     * Setting this to false does not stop any active path requests from being calculated or stop it from continuing to follow the current path.
     * \see #canMove
     */
    public bool canSearch = true;

    /** Enables or disables movement.
      * \see #canSearch */
    public bool canMove = true;

    /** Maximum velocity.
     * This is the maximum speed in world units per second.
     */
    public float speed = 3;

    /** Rotation speed.
     * Rotation is calculated using Quaternion.SLerp. This variable represents the damping, the higher, the faster it will be able to rotate.
     */
    public float turningSpeed = 5;

    /** Distance from the target point where the AI will start to slow down.
     * Note that this doesn't only affect the end point of the path
     * but also any intermediate points, so be sure to set #forwardLook and #pickNextWaypointDist to a higher value than this
     */
    public float slowdownDistance = 0.6F;

    /** Determines within what range it will switch to target the next waypoint in the path */
    public float pickNextWaypointDist = 2;

    /** Target point is Interpolated on the current segment in the path so that it has a distance of #forwardLook from the AI.
      * See the detailed description of AIPath for an illustrative image */
    public float forwardLook = 1;

    /** Distance to the end point to consider the end of path to be reached.
     * When this has been reached, the AI will not move anymore until the target changes and OnTargetReached will be called.
     */
    public float endReachedDistance = 0.2F;

    /** Do a closest point on path check when receiving path callback.
     * Usually the AI has moved a bit between requesting the path, and getting it back, and there is usually a small gap between the AI
     * and the closest node.
     * If this option is enabled, it will simulate, when the path callback is received, movement between the closest node and the current
     * AI position. This helps to reduce the moments when the AI just get a new path back, and thinks it ought to move backwards to the start of the new path
     * even though it really should just proceed forward.
     */
    public bool closestOnPathCheck = true;

    protected float minMoveScale = 0.05F;

    /** Cached Seeker component */
    protected Seeker seeker;

    /** Cached Transform component */
    protected Transform tr;

    /** Time when the last path request was sent */
    private float lastRepath = -9999;

    /** Current path which is followed */
    protected Path path;

    /** Cached CharacterController component */
    protected CharacterController controller;

    /** Cached NavmeshController component */
    protected NavmeshController navController;

    protected RVOController rvoController;

    /** Cached Rigidbody component */
    protected Rigidbody rigid;

    /** Current index in the path which is current target */
    protected int currentWaypointIndex = 0;

    /** Holds if the end-of-path is reached
     * \see TargetReached */
    protected bool targetReached = false;

    /** Only when the previous path has been returned should be search for a new path */
    protected bool canSearchAgain = true;

    protected Vector3 lastFoundWaypointPosition;
    protected float lastFoundWaypointTime = -9999;
    protected bool flag = false;
    protected float f_curtime = 0.0f;
    protected bool flag1 = false;
    protected bool flag2 = false;
    protected bool flag3 = false;
    protected GameObject myParent;
    protected GameObject myParent1;
    //判断是否到达了目标
    protected bool isReach = false;
    public static bool ReachInSeeker = false;
    //我的位置，和目标位置
    protected string MyLocation;
    protected string TargetLocation;

    Vector3 po;

    /** Returns if the end-of-path has been reached
     * \see targetReached */
    
    public bool TargetReached
    {
        get
        {
            return targetReached;
        }
    }

    /** Holds if the Start function has been run.
     * Used to test if coroutines should be started in OnEnable to prevent calculating paths
     * in the awake stage (or rather before start on frame 0).
     */
    private bool startHasRun = false;
    private int ans = 0;
    protected GameObject tar;
    protected GameObject tar1;
    protected GameObject fa;
    

    /** Initializes reference variables.
     * If you override this function you should in most cases call base.Awake () at the start of it.
      * */
    protected virtual void Awake()
    {
        seeker = GetComponent<Seeker>();
        myParent = GameObject.Find("father");
        myParent1 = GameObject.Find("father1");
        /*tar1 = GameObject.Find("door1");
        tar = GameObject.Find("door2");
        tar.transform.parent = tar1.transform;
        tar = GameObject.Find("door3");
        tar.transform.parent = tar1.transform;
        tar = GameObject.Find("door4");
        tar.transform.parent = tar1.transform;*/
        targetPoints = myParent.transform;
        //This is a simple optimization, cache the transform component lookup
 
        /*target = targetPoints;
        target1 = target;
        po = target.position;
        if (po.y < 48.0)
            step = 1;
        else if (po.y >= 48.0 && po.y < 90.0)
            step = 2;
        else
            step = 3;*/
        //text = GameObject.Find("Cube1").transform;
        tr = transform;

        //Cache some other components (not all are necessarily there)
        controller = GetComponent<CharacterController>();
        navController = GetComponent<NavmeshController>();
        rvoController = GetComponent<RVOController>();
        if (rvoController != null) rvoController.enableRotation = false;
        rigid = rigidbody;
        people = GameObject.Find("people").transform;
        Flash_Botton = 0;
    }
    /*
    //成功找到father，可以寻路...
    public static void FindFather() {
        targetPoints = GameObject.Find("father").transform;
        target = targetPoints;
        target1 = target;
        po = target.position;
        if (po.y < 48.0)
            step = 1;
        else if (po.y >= 48.0 && po.y < 90.0)
            step = 2;
        else
            step = 3;
        tr = transform;

        //Cache some other components (not all are necessarily there)
        controller = GetComponent<CharacterController>();
        navController = GetComponent<NavmeshController>();
        rvoController = GetComponent<RVOController>();
        if (rvoController != null) rvoController.enableRotation = false;
        rigid = rigidbody;
    }
    */
    /** Starts searching for paths.
     * If you override this function you should in most cases call base.Start () at the start of it.
     * \see OnEnable
     * \see RepeatTrySearchPath
     */
    protected virtual void Start()
    {
        startHasRun = true;
        OnEnable();
    }

    /** Run at start and when reenabled.
     * Starts RepeatTrySearchPath.
     * 
     * \see Start
     */
    protected virtual void OnEnable()
    {

        lastRepath = -9999;
        canSearchAgain = true;

        lastFoundWaypointPosition = GetFeetPosition();

        if (startHasRun)
        {
            //Make sure we receive callbacks when paths complete
            seeker.pathCallback += OnPathComplete;

            StartCoroutine(RepeatTrySearchPath());
        }
    }

    public void OnDisable()
    {
        // Abort calculation of path
        if (seeker != null && !seeker.IsDone()) seeker.GetCurrentPath().Error();

        // Release current path
        if (path != null) path.Release(this);
        path = null;

        //Make sure we receive callbacks when paths complete
        seeker.pathCallback -= OnPathComplete;
    }

    /** Tries to search for a path every #repathRate seconds.
      * \see TrySearchPath
      */
    protected IEnumerator RepeatTrySearchPath()
    {
        while (true)
        {
            float v = TrySearchPath();
            yield return new WaitForSeconds(v);
        }
    }

    /** Tries to search for a path.
     * Will search for a new path if there was a sufficient time since the last repath and both
     * #canSearchAgain and #canSearch are true and there is a target.
     * 
     * \returns The time to wait until calling this function again (based on #repathRate) 
     */
    public float TrySearchPath()
    {
        if (Time.time - lastRepath >= repathRate && canSearchAgain && canSearch && target != null)
        {
            SearchPath();
            return repathRate;
        }
        else
        {
            //StartCoroutine (WaitForRepath ());
            float v = repathRate - (Time.time - lastRepath);
            return v < 0 ? 0 : v;
        }
    }

    /** Requests a path to the target */
    public virtual void SearchPath()
    {

        if (target == null) throw new System.InvalidOperationException("Target is null");

        lastRepath = Time.time;
        //This is where we should search to
        Vector3 targetPosition = target.position;

        canSearchAgain = false;

        //Alternative way of requesting the path
        //ABPath p = ABPath.Construct (GetFeetPosition(),targetPoint,null);
        //seeker.StartPath (p);

        //We should search from the current position
        seeker.StartPath(GetFeetPosition(), targetPosition);
    }

    public virtual void OnTargetReached()
    {
        //设置isReach为true.在BotAI里面会调用
        if(target != null)
            isReach = true;
        print("isReach = " + isReach);
        ReachInSeeker = true;
        flag = true;
        Transform[] endPoints = new Transform[targetPoints.childCount];
        int c = 0;
        foreach (Transform child in targetPoints)
        {
            endPoints[c] = child;
            c++;
        }
        //print("reach");
        if (targetPoints.childCount != 0)
        {
            //Destroy(endPoints[0]);
            
            if (IsNav)
                endPoints[0].transform.parent = myParent1.transform;
            else
                endPoints[0].transform.parent = null;
            target1 = target;
            target = endPoints[0];
            po = target.position;
            if (po.y < 40)
                step = 1;
            else if (po.y >= 48.0 && po.y < 90.0)
                step = 2;
            else if (po.y > 90.0)
                step = 3;
        }
        else {
            target1 = target;
            //target = myParent.transform;
            target = null;
           // isReach = false;
            print("isReach = " + isReach);
            ReachInSeeker = false;
        }
        //End of path has been reached
        //If you want custom logic for when the AI has reached it's destination
        //add it here
        //You can also create a new script which inherits from this one
        //and override the function in that script
    }
    

    /** Called when a requested path has finished calculation.
      * A path is first requested by #SearchPath, it is then calculated, probably in the same or the next frame.
      * Finally it is returned to the seeker which forwards it to this function.\n
      */
   
    public virtual void OnPathComplete(Path _p)
    {
        ABPath p = _p as ABPath;
        if (p == null) throw new System.Exception("This function only handles ABPaths, do not use special path types");

        canSearchAgain = true;

        //Claim the new path
        p.Claim(this);

        // Path couldn't be calculated of some reason.
        // More info in p.errorLog (debug string)
        if (p.error)
        {
            p.Release(this);
            return;
        }

        //Release the previous path
        if (path != null) path.Release(this);

        //Replace the old path
        path = p;

        //Reset some variables
        currentWaypointIndex = 0;
        targetReached = false;

        //The next row can be used to find out if the path could be found or not
        //If it couldn't (error == true), then a message has probably been logged to the console
        //however it can also be got using p.errorLog
        //if (p.error)

        if (closestOnPathCheck)
        {
            Vector3 p1 = Time.time - lastFoundWaypointTime < 0.3f ? lastFoundWaypointPosition : p.originalStartPoint;
            Vector3 p2 = GetFeetPosition();
            Vector3 dir = p2 - p1;
            float magn = dir.magnitude;
            dir /= magn;
            int steps = (int)(magn / pickNextWaypointDist);

#if ASTARDEBUG
			Debug.DrawLine (p1,p2,Color.red,1);
#endif

            for (int i = 0; i <= steps; i++)
            {
                CalculateVelocity(p1);
                p1 += dir;
            }

        }
    }

    public virtual Vector3 GetFeetPosition()
    {
        if (rvoController != null)
        {
            print("rvoController = " + (tr.position - Vector3.up * rvoController.height * 0.5f));
            return tr.position - Vector3.up * rvoController.height * 0.5f;
        }
        else
            if (controller != null)
            {
                print("controller = " + (tr.position - Vector3.up * controller.height * 0.5f));
                return tr.position - Vector3.up * controller.height * 0.5F;
            }
        
        return tr.position;
    }
   
    public virtual void Update()
    {

        GUI.Button(new Rect(210, 30, 150, 150), "asfhusbng");
        if (!canMove) { return; }

        Vector3 dir = CalculateVelocity(GetFeetPosition());

        //Rotate towards targetDirection (filled in by CalculateVelocity)
        RotateTowards(targetDirection);

        if (rvoController != null)
        {
            rvoController.Move(dir);
        }
        else
            if (navController != null)
            {
#if FALSE
			navController.SimpleMove (GetFeetPosition(),dir);
#endif
            }
            else if (controller != null)
            {
                controller.SimpleMove(dir);
            }
            else if (rigid != null)
            {
                rigid.AddForce(dir);
            }
            else
            {
                transform.Translate(dir * Time.deltaTime, Space.World);
            }
    }

    /** Point to where the AI is heading.
      * Filled in by #CalculateVelocity */
    protected Vector3 targetPoint;
    /** Relative direction to where the AI is heading.
     * Filled in by #CalculateVelocity */
    protected Vector3 targetDirection;

    protected float XZSqrMagnitude(Vector3 a, Vector3 b)
    {
        float dx = b.x - a.x;
        float dz = b.z - a.z;
        return dx * dx + dz * dz;
    }

    /** Calculates desired velocity.
     * Finds the target path segment and returns the forward direction, scaled with speed.
     * A whole bunch of restrictions on the velocity is applied to make sure it doesn't overshoot, does not look too far ahead,
     * and slows down when close to the target.
     * /see speed
     * /see endReachedDistance
     * /see slowdownDistance
     * /see CalculateTargetPoint
     * /see targetPoint
     * /see targetDirection
     * /see currentWaypointIndex
     */
    protected Vector3 CalculateVelocity(Vector3 currentPosition)
    {
        if (target == null)
            return Vector3.zero;
        if (path == null || path.vectorPath == null || path.vectorPath.Count == 0) return Vector3.zero;

        List<Vector3> vPath = path.vectorPath;
        //Vector3 currentPosition = GetFeetPosition();

        if (vPath.Count == 1)
        {
            vPath.Insert(0, currentPosition);
        }

        if (currentWaypointIndex >= vPath.Count) { currentWaypointIndex = vPath.Count - 1; }

        if (currentWaypointIndex <= 1) currentWaypointIndex = 1;

        while (true)
        {
            if (currentWaypointIndex < vPath.Count - 1)
            {
                //There is a "next path segment"
                float dist = XZSqrMagnitude(vPath[currentWaypointIndex], currentPosition);
                //Mathfx.DistancePointSegmentStrict (vPath[currentWaypointIndex+1],vPath[currentWaypointIndex+2],currentPosition);
                if (dist < pickNextWaypointDist * pickNextWaypointDist)
                {
                    lastFoundWaypointPosition = currentPosition;
                    lastFoundWaypointTime = Time.time;
                    currentWaypointIndex++;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        Vector3 dir = vPath[currentWaypointIndex] - vPath[currentWaypointIndex - 1];
        Vector3 targetPosition = CalculateTargetPoint(currentPosition, vPath[currentWaypointIndex - 1], vPath[currentWaypointIndex]);
        //vPath[currentWaypointIndex] + Vector3.ClampMagnitude (dir,forwardLook);



        dir = targetPosition - currentPosition;
        dir.y = 0;
        float targetDist = dir.magnitude;

        float slowdown = Mathf.Clamp01(targetDist / slowdownDistance);

        this.targetDirection = dir;
        this.targetPoint = targetPosition;
        if (target == myParent.transform)
        {
            targetReached = true; OnTargetReached();
            return Vector3.zero;
        }
        // print("target_name = " + target.name);
        Vector3 pot = target.position;              //获得当前target的位置
        float distancex, distancez, distancey;
        distancex = Math.Abs(people.position.x - pot.x);      //计算他们之间的距离
        distancez = Math.Abs(people.position.z - pot.z);
        distancey = Math.Abs(people.position.y - pot.y);
        //if ((currentWaypointIndex == vPath.Count - 1) && (targetDist - 0.507f< endReachedDistance))
        //if ((currentWaypointIndex == vPath.Count - 1) || (targetDist <=  endReachedDistance))
        //if (currentWaypointIndex == vPath.Count - 1)
        // print("target.position = " + target.position);
        // print("people.position = " + person.transform.position);
        //print("distancex = " + distancex);
        // print("distancey = " + distancey);
        // print("distancez = " + distancez);
        if (distancex < 2.0f && distancez < 2.0f && distancey < 2.0f)     //如果人物和目标在x和z之间的差值都在10以内，且y在0.3以内，保证在同一楼层，则调用OnTargetReached()函数。
        {
           // print("has reached!!!!!");
            if (!targetReached) { targetReached = true; OnTargetReached(); }

            //Send a move request, this ensures gravity is applied
            return Vector3.zero;
        }
        Vector3 forward = tr.forward;
        float dot = Vector3.Dot(dir.normalized, forward);
        float sp = speed * Mathf.Max(dot, minMoveScale) * slowdown;

#if ASTARDEBUG
		Debug.DrawLine (vPath[currentWaypointIndex-1] , vPath[currentWaypointIndex],Color.black);
		Debug.DrawLine (GetFeetPosition(),targetPosition,Color.red);
		Debug.DrawRay (targetPosition,Vector3.up, Color.red);
		Debug.DrawRay (GetFeetPosition(),dir,Color.yellow);
		Debug.DrawRay (GetFeetPosition(),forward*sp,Color.cyan);
#endif

        if (Time.deltaTime > 0)
        {
            sp = Mathf.Clamp(sp, 0, targetDist / (Time.deltaTime * 2));
        }
        return forward * sp;
    }

    /** Rotates in the specified direction.
     * Rotates around the Y-axis.
     * \see turningSpeed
     */
    protected virtual void RotateTowards(Vector3 dir)
    {

        if (dir == Vector3.zero) return;

        Quaternion rot = tr.rotation;
        Quaternion toTarget = Quaternion.LookRotation(dir);

        rot = Quaternion.Slerp(rot, toTarget, turningSpeed * Time.deltaTime);
        Vector3 euler = rot.eulerAngles;
        euler.z = 0;
        euler.x = 0;
        rot = Quaternion.Euler(euler);
        //print("rot = " + rot);
        tr.rotation = rot;
    }
    
    /** Calculates target point from the current line segment.
     * \param p Current position
     * \param a Line segment start
     * \param b Line segment end
     * The returned point will lie somewhere on the line segment.
     * \see #forwardLook
     * \todo This function uses .magnitude quite a lot, can it be optimized?
     */
    protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
    {
        a.y = p.y;
        b.y = p.y;

        float magn = (a - b).magnitude;
        if (magn == 0) return a;

        float closest = AstarMath.Clamp01(AstarMath.NearestPointFactor(a, b, p));
        Vector3 point = (b - a) * closest + a;
        float distance = (point - p).magnitude;

        float lookAhead = Mathf.Clamp(forwardLook - distance, 0.0F, forwardLook);

        float offset = lookAhead / magn;
        offset = Mathf.Clamp(offset + closest, 0.0F, 1.0F);
        return (b - a) * offset + a;
    }
    
    public void ExchangeChild() {
        /*foreach (Transform child in myParent.transform)
        {
            child.parent = myParent1.transform;
            print("first:"+child);
            print(child.parent);
        }*/
        Transform[] endPoints = new Transform[myParent.transform.childCount];
        int c = 0;
        foreach (Transform child in myParent.transform)
        {
            endPoints[c] = child;
            c++;
        }
        print("It has "+c);
        for (int i = 0; i < c; i++) {
            endPoints[i].parent = myParent1.transform;
        }
        Transform[] endPoints1 = new Transform[myParent1.transform.childCount];
        int c1 = 0;
        foreach (Transform child in myParent1.transform)
        {
            endPoints1[c1] = child;
            c1++;
        }
        print("It has " + c1);
        for (int i = 0; i < c1; i++)
        {
            endPoints1[i].parent = myParent.transform;
        }
       // targetPoints = myParent.transform;
        /*foreach (Transform child in myParent1.transform)
        {
            child.parent = myParent.transform;
            print("second:" + child);
            print(child.parent);
        }*/
    }
}
