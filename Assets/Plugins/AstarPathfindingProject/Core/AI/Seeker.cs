/* 修改寻路路线的材质，也就是显示在游戏中的路线纹理
 * 
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System.Diagnostics;
using System;

[AddComponentMenu("Pathfinding/Seeker")]
/** Handles path calls for a single unit.
 * \ingroup relevant
 * This is a component which is meant to be attached to a single unit (AI, Robot, Player, whatever) to handle it's pathfinding calls.
 * It also handles post-processing of paths using modifiers.
 * \see \ref calling-pathfinding
 */
public class Seeker : MonoBehaviour
{

    //====== SETTINGS ======

    /* Recalculate last queried path when a graph changes. \see AstarPath.OnGraphsUpdated */
    //public bool recalcOnGraphChange = true;
    public static bool Exist_target = true;     //判断是否存在target
    public bool drawGizmos = true;
    public bool detailedGizmos = false;
    private Vector3[] linePoints;
    private List<Vector3> points = new List<Vector3>();
    public Material tax;
    private LineRenderer lineRenderer;
    private int index = 0;
    public static string ChangePoints = "";        //生成的点转化为string
    private bool flag_Reach = true;      //判断是否是第一次生成的路线
    private int flash;         //用于路线闪烁
    private Transform people;
    private int Distance;       //线段的长度
    
    /** Saves nearest nodes for previous path to enable faster Get Nearest Node calls.
     * This variable basically does not a affect anything at the moment. So it is hidden in the inspector */
    [HideInInspector]
    public bool saveGetNearestHints = true;

    public StartEndModifier startEndModifier = new StartEndModifier();

    [HideInInspector]
    public TagMask traversableTags = new TagMask(-1, -1);

    [HideInInspector]
    /** Penalties for each tag.
     * Tag 0 which is the default tag, will have added a penalty of tagPenalties[0].
     * These should only be positive values since the A* algorithm cannot handle negative penalties.
     * \note This array should always have a length of 32.
     * \see Pathfinding.Path.tagPenalties
     */
    public int[] tagPenalties = new int[32];
    public  Vector3[] Points;

    //====== SETTINGS ======

    //public delegate Path PathReturn (Path p);

    /** Callback for when a path is completed. Movement scripts should register to this delegate.\n
     * A temporary callback can also be set when calling StartPath, but that delegate will only be called for that path */
    public OnPathDelegate pathCallback;

    /** Called before pathfinding is started */
    public OnPathDelegate preProcessPath;

    /** For anything which requires the original nodes (Node[]) (before modifiers) to work */
    public OnPathDelegate postProcessOriginalPath;

    /** Anything which only modifies the positions (Vector3[]) */
    public OnPathDelegate postProcessPath;

    //public GetNextTargetDelegate getNextTarget;

    //DEBUG
    //public Path lastCompletedPath;
    [System.NonSerialized]
    public static List<Vector3> lastCompletedVectorPath;
    [System.NonSerialized]
    public List<GraphNode> lastCompletedNodePath;

    //END DEBUG

    /** The current path */
    [System.NonSerialized]
    protected Path path;

    /** Previous path. Used to draw gizmos */
    private Path prevPath;

    /** Returns #path */
    public Path GetCurrentPath()
    {
        return path;
    }

    private GraphNode startHint;
    private GraphNode endHint;

    private OnPathDelegate onPathDelegate;
    private OnPathDelegate onPartialPathDelegate;

    /** Temporary callback only called for the current path. This value is set by the StartPath functions */
    private OnPathDelegate tmpPathCallback;

    /** The path ID of the last path queried */
    protected uint lastPathID = 0;

#if PhotonImplementation
	public Seeker () {
		Awake ();
	}
#endif

    /** Initializes a few variables
	 */
    public void Awake()
    {
        onPathDelegate = OnPathComplete;
        onPartialPathDelegate = OnPartialPathComplete;
        startEndModifier.Awake(this);
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        //设置材质  
        lineRenderer.material = tax;
        //设置颜色  
        //lineRenderer.SetColors(Color.red, Color.yellow);
        //设置宽度  
         lineRenderer.SetWidth(0.2f, 0.2f);
         Points = new Vector3[1000];
        index = 0;
        flash = 0;
        people = GameObject.Find("people").transform;
        Distance = 5;
    }

    /** Cleans up some variables.
     * Releases any eventually claimed paths.
     * Calls OnDestroy on the #startEndModifier.
     * 
     * \see ReleaseClaimedPath
     * \see startEndModifier
     */
    public void OnDestroy()
    {
        ReleaseClaimedPath();
        startEndModifier.OnDestroy(this);
    }

    /** Releases an eventual claimed path.
     * The seeker keeps the latest path claimed so it can draw gizmos.
     * In some cases this might not be desireable and you want it released.
     * In that case, you can call this method to release it (not that path gizmos will then not be drawn).
     * 
     * If you didn't understand anything from the description above, you probably don't need to use this method.
     */
    public void ReleaseClaimedPath()
    {
        if (prevPath != null)
        {
            prevPath.ReleaseSilent(this);
            prevPath = null;
        }
    }

    private List<IPathModifier> modifiers = new List<IPathModifier>();

    public void RegisterModifier(IPathModifier mod)
    {
        if (modifiers == null)
        {
            modifiers = new List<IPathModifier>(1);
        }

        modifiers.Add(mod);
    }

    public void DeregisterModifier(IPathModifier mod)
    {
        if (modifiers == null)
        {
            return;
        }
        modifiers.Remove(mod);
    }

    public enum ModifierPass
    {
        PreProcess,
        PostProcessOriginal,
        PostProcess
    }

    /** Post Processes the path.
     * This will run any modifiers attached to this GameObject on the path.
     * This is identical to calling RunModifiers(ModifierPass.PostProcess, path)
     * \see RunModifiers
     * \since Added in 3.2
     */
    public void PostProcess(Path p)
    {
        RunModifiers(ModifierPass.PostProcess, p);
    }

    /** Runs modifiers on path \a p */
    public void RunModifiers(ModifierPass pass, Path p)
    {

        //Sort the modifiers based on priority (bubble sort (slow but since it's a small list, it works good))
        bool changed = true;
        while (changed)
        {
            changed = false;
            for (int i = 0; i < modifiers.Count - 1; i++)
            {
                if (modifiers[i].Priority < modifiers[i + 1].Priority)
                {
                    IPathModifier tmp = modifiers[i];
                    modifiers[i] = modifiers[i + 1];
                    modifiers[i + 1] = tmp;
                    changed = true;
                }
            }
        }

        //Call eventual delegates
        switch (pass)
        {
            case ModifierPass.PreProcess:
                if (preProcessPath != null) preProcessPath(p);
                break;
            case ModifierPass.PostProcessOriginal:
                if (postProcessOriginalPath != null) postProcessOriginalPath(p);
                break;
            case ModifierPass.PostProcess:
                if (postProcessPath != null) postProcessPath(p);
                break;
        }

        //No modifiers, then exit here
        if (modifiers.Count == 0) return;

        ModifierData prevOutput = ModifierData.All;
        IPathModifier prevMod = modifiers[0];

        //Loop through all modifiers and apply post processing
        for (int i = 0; i < modifiers.Count; i++)
        {
            //Cast to MonoModifier, i.e modifiers attached as scripts to the game object
            MonoModifier mMod = modifiers[i] as MonoModifier;

            //Ignore modifiers which are not enabled
            if (mMod != null && !mMod.enabled) continue;

            switch (pass)
            {
                case ModifierPass.PreProcess:
                    modifiers[i].PreProcess(p);
                    break;
                case ModifierPass.PostProcessOriginal:
                    modifiers[i].ApplyOriginal(p);
                    break;
                case ModifierPass.PostProcess:

                    //Convert the path if necessary to match the required input for the modifier
                    ModifierData newInput = ModifierConverter.Convert(p, prevOutput, modifiers[i].input);

                    if (newInput != ModifierData.None)
                    {
                        modifiers[i].Apply(p, newInput);
                        prevOutput = modifiers[i].output;
                    }
                    else
                    {

                        UnityEngine.Debug.Log("Error converting " + (i > 0 ? prevMod.GetType().Name : "original") + "'s output to " + (modifiers[i].GetType().Name) + "'s input.\nTry rearranging the modifier priorities on the Seeker.");

                        prevOutput = ModifierData.None;
                    }

                    prevMod = modifiers[i];
                    break;
            }

            if (prevOutput == ModifierData.None)
            {
                break;
            }

        }
    }

    /** Is the current path done calculating.
     * Returns true if the current #path has been returned or if the #path is null.
     * 
     * \note Do not confuse this with Pathfinding.Path.IsDone. They do mostly return the same value, but not always.
     * 
     * \since Added in 3.0.8
     * \version Behaviour changed in 3.2
     * */
    public bool IsDone()
    {
        return path == null || path.GetState() >= PathState.Returned;
    }

    /** Called when a path has completed.
     * This should have been implemented as optional parameter values, but that didn't seem to work very well with delegates (the values weren't the default ones)
     * \see OnPathComplete(Path,bool,bool) */
    public void OnPathComplete(Path p)
    {
        OnPathComplete(p, true, true);
    }

    /** Called when a path has completed.
     * Will post process it and return it by calling #tmpPathCallback and #pathCallback */
    public void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
    {

        AstarProfiler.StartProfile("Seeker OnPathComplete");


        if (p != null && p != path && sendCallbacks)
        {
            return;
        }


        if (this == null || p == null || p != path)
            return;

        if (!path.error && runModifiers)
        {
            AstarProfiler.StartProfile("Seeker Modifiers");
            //This will send the path for post processing to modifiers attached to this Seeker
            RunModifiers(ModifierPass.PostProcessOriginal, path);

            //This will send the path for post processing to modifiers attached to this Seeker
            RunModifiers(ModifierPass.PostProcess, path);
            AstarProfiler.EndProfile();
        }

        if (sendCallbacks)
        {

            p.Claim(this);

            AstarProfiler.StartProfile("Seeker Callbacks");

            lastCompletedNodePath = p.path;
            lastCompletedVectorPath = p.vectorPath;

            //This will send the path to the callback (if any) specified when calling StartPath
            if (tmpPathCallback != null)
            {
                tmpPathCallback(p);
            }

            //This will send the path to any script which has registered to the callback
            if (pathCallback != null)
            {
                pathCallback(p);
            }

            //Recycle the previous path
            if (prevPath != null)
            {
                prevPath.ReleaseSilent(this);
            }

            prevPath = p;

            //If not drawing gizmos, then storing prevPath is quite unecessary
            //So clear it and set prevPath to null
            if (!drawGizmos) ReleaseClaimedPath();

            AstarProfiler.EndProfile();
        }

        AstarProfiler.EndProfile();
    }

    /** Called for each path in a MultiTargetPath. Only post processes the path, does not return it.
      * \astarpro */
    public void OnPartialPathComplete(Path p)
    {
        OnPathComplete(p, true, false);
    }

    /** Called once for a MultiTargetPath. Only returns the path, does not post process.
      * \astarpro */
    public void OnMultiPathComplete(Path p)
    {
        OnPathComplete(p, false, true);
    }

    /*public void OnEnable () {
        //AstarPath.OnGraphsUpdated += CheckPathValidity;
    }
	
    public void OnDisable () {
        //AstarPath.OnGraphsUpdated -= CheckPathValidity;
    }*/

    /*public void CheckPathValidity (AstarPath active) {
		
        /*if (!recalcOnGraphChange) {
            return;
        }
		
		
		
        //Debug.Log ("Checking Path Validity");
        //Debug.Break ();
        if (lastCompletedPath != null && !lastCompletedPath.error) {
            //Debug.Log ("Checking Path Validity");
            StartPath (transform.position,lastCompletedPath.endPoint);
			
            /*if (!lastCompletedPath.path[0].IsWalkable (lastCompletedPath)) {
                StartPath (transform.position,lastCompletedPath.endPoint);
                return;
            }
				
            for (int i=0;i<lastCompletedPath.path.Length-1;i++) {
				
                if (!lastCompletedPath.path[i].ContainsConnection (lastCompletedPath.path[i+1],lastCompletedPath)) {
                    StartPath (transform.position,lastCompletedPath.endPoint);
                    return;
                }
                Debug.DrawLine (lastCompletedPath.path[i].position,lastCompletedPath.path[i+1].position,Color.cyan);
            }*
        }*
    }*/

    //The frame the last call was made from this Seeker
    //private int lastPathCall = -1000;

    /** Returns a new path instance. The path will be taken from the path pool if path recycling is turned on.\n
     * This path can be sent to #StartPath(Path,OnPathDelegate,int) with no change, but if no change is required #StartPath(Vector3,Vector3,OnPathDelegate) does just that.
     * \code Seeker seeker = GetComponent (typeof(Seeker)) as Seeker;
     * Path p = seeker.GetNewPath (transform.position, transform.position+transform.forward*100);
     * p.nnConstraint = NNConstraint.Default; \endcode */
    public ABPath GetNewPath(Vector3 start, Vector3 end)
    {
        //Construct a path with start and end points
        ABPath p = ABPath.Construct(start, end, null);

        return p;
    }

    /** Call this function to start calculating a path.
     * \param start		The start point of the path
     * \param end		The end point of the path
     */
    public Path StartPath(Vector3 start, Vector3 end)
    {
        return StartPath(start, end, null, -1);
    }

    /** Call this function to start calculating a path.
     * \param start		The start point of the path
     * \param end		The end point of the path
     * \param callback	The function to call when the path has been calculated
     * 
     * \a callback will be called when the path has completed.
     * \a Callback will not be called if the path is canceled (e.g when a new path is requested before the previous one has completed) */
    public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
    {
        return StartPath(start, end, callback, -1);
    }

    /** Call this function to start calculating a path.
     * \param start		The start point of the path
     * \param end		The end point of the path
     * \param callback	The function to call when the path has been calculated
     * \param graphMask	Mask used to specify which graphs should be searched for close nodes. See Pathfinding.NNConstraint.graphMask.
     * 
     * \a callback will be called when the path has completed.
     * \a Callback will not be called if the path is canceled (e.g when a new path is requested before the previous one has completed) */
    public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, int graphMask)
    {
        Path p = GetNewPath(start, end);
        return StartPath(p, callback, graphMask);
    }

    /** Call this function to start calculating a path.
     * \param p			The path to start calculating
     * \param callback	The function to call when the path has been calculated
     * \param graphMask	Mask used to specify which graphs should be searched for close nodes. See Pathfinding.NNConstraint.graphMask. \astarproParam
     * 
     * \a callback will be called when the path has completed.
     * \a Callback will not be called if the path is canceled (e.g when a new path is requested before the previous one has completed) */
    public Path StartPath(Path p, OnPathDelegate callback = null, int graphMask = -1)
    {
        p.enabledTags = traversableTags.tagsChange;
        p.tagPenalties = tagPenalties;

#if !AstarFree && FALSE
		//In case a multi target path has been specified, call special logic
		if (p.GetType () == typeof (MultiTargetPath)) {
			return StartMultiTargetPath (p as MultiTargetPath,callback);
		}
#endif
        //Cancel a previously requested path is it has not been processed yet and also make sure that it has not been recycled and used somewhere else
        if (path != null && path.GetState() <= PathState.Processing && lastPathID == path.pathID)
        {
            path.Error();
            path.LogError("Canceled path because a new one was requested.\n" +
                "This happens when a new path is requested from the seeker when one was already being calculated.\n" +
                "For example if a unit got a new order, you might request a new path directly instead of waiting for the now" +
                " invalid path to be calculated. Which is probably what you want.\n" +
                "If you are getting this a lot, you might want to consider how you are scheduling path requests.");
            //No callback should be sent for the canceled path
        }

        path = p;
        path.callback += onPathDelegate;
        path.nnConstraint.graphMask = graphMask;

        tmpPathCallback = callback;

        //Set the Get Nearest Node hints if they have not already been set
        /*if (path.startHint == null)
            path.startHint = startHint;
			
        if (path.endHint == null) 
            path.endHint = endHint;
        */

        //Save the path id so we can make sure that if we cancel a path (see above) it should not have been recycled yet.
        lastPathID = path.pathID;

        //Delay the path call by one frame if it was sent the same frame as the previous call
        /*if (lastPathCall == Time.frameCount) {
            StartCoroutine (DelayPathStart (path));
            return path;
        }*/

        //lastPathCall = Time.frameCount;

        //Pre process the path
        RunModifiers(ModifierPass.PreProcess, path);

        //Send the request to the pathfinder
        AstarPath.StartPath(path);

        return path;
    }

    /** Starts a Multi Target Path from one start point to multiple end points. A Multi Target Path will search for all the end points in one search and will return all paths if \a pathsForAll is true, or only the shortest one if \a pathsForAll is false.\n
     * \param start			The start point of the path
     * \param endPoints		The end points of the path
     * \param pathsForAll	Indicates whether or not a path to all end points should be searched for or only to the closest one
     * \param callback		The function to call when the path has been calculated
     * \param graphMask		Mask used to specify which graphs should be searched for close nodes. See Pathfinding.NNConstraint.graphMask. \astarproParam
     * 
     * \a callback and #pathCallback will be called when the path has completed. \a Callback will not be called if the path is canceled (e.g when a new path is requested before the previous one has completed)
     * \astarpro 
     * \see Pathfinding.MultiTargetPath
     * \see \ref MultiTargetPathExample.cs "Example of how to use multi-target-paths"
     */
    public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback = null, int graphMask = -1)
    {
        MultiTargetPath p = MultiTargetPath.Construct(start, endPoints, null, null);
        p.pathsForAll = pathsForAll;
        return StartMultiTargetPath(p, callback, graphMask);
    }

    /** Starts a Multi Target Path from multiple start points to a single target point. A Multi Target Path will search from all start points to the target point in one search and will return all paths if \a pathsForAll is true, or only the shortest one if \a pathsForAll is false.\n
     * \param startPoints	The start points of the path
     * \param end			The end point of the path
     * \param pathsForAll	Indicates whether or not a path from all start points should be searched for or only to the closest one
     * \param callback		The function to call when the path has been calculated
     * \param graphMask		Mask used to specify which graphs should be searched for close nodes. See Pathfinding.NNConstraint.graphMask. \astarproParam
     * 
     * \a callback and #pathCallback will be called when the path has completed. \a Callback will not be called if the path is canceled (e.g when a new path is requested before the previous one has completed)
     * \astarpro 
     * \see Pathfinding.MultiTargetPath
     * \see \ref MultiTargetPathExample.cs "Example of how to use multi-target-paths"
     */
    public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback = null, int graphMask = -1)
    {
        MultiTargetPath p = MultiTargetPath.Construct(startPoints, end, null, null);
        p.pathsForAll = pathsForAll;
        return StartMultiTargetPath(p, callback, graphMask);
    }

    /** Starts a Multi Target Path. Takes a MultiTargetPath and wires everything up for it to send callbacks to the seeker for post-processing.\n
     * \param p				The path to start calculating
     * \param callback		The function to call when the path has been calculated
     * \param graphMask	Mask used to specify which graphs should be searched for close nodes. See Pathfinding.NNConstraint.graphMask. \astarproParam
     * 
     * \a callback and #pathCallback will be called when the path has completed. \a Callback will not be called if the path is canceled (e.g when a new path is requested before the previous one has completed)
     * \astarpro
     * \see Pathfinding.MultiTargetPath
     * \see \ref MultiTargetPathExample.cs "Example of how to use multi-target-paths"
     */
    /* public void DestroyLine()
     {

         int arrayLength = lastCompletedVectorPath.Count;
         if (arrayLength > 0)
         {
             lineRenderer.Destroy(lastCompletedVectorPath[arrayLength - 1]);
             pointPos.RemoveAt(arrayLength - 1);
             lineRenderer.SetVertexCount(--lineSeg);
         }
     }*/
    public MultiTargetPath StartMultiTargetPath(MultiTargetPath p, OnPathDelegate callback = null, int graphMask = -1)
    {

        //Cancel a previously requested path is it has not been processed yet and also make sure that it has not been recycled and used somewhere else
        if (path != null && path.GetState() <= PathState.Processing && lastPathID == path.pathID)
        {
            path.ForceLogError("Canceled path because a new one was requested");
            //No callback should be sent for the canceled path
        }

        OnPathDelegate[] callbacks = new OnPathDelegate[p.targetPoints.Length];
        for (int i = 0; i < callbacks.Length; i++)
        {
            callbacks[i] = onPartialPathDelegate;
        }

        p.callbacks = callbacks;
        p.callback += OnMultiPathComplete;
        p.nnConstraint.graphMask = graphMask;

        path = p;

        tmpPathCallback = callback;

        //Save the path id so we can make sure that if we cancel a path (see above) it should not have been recycled yet.
        lastPathID = path.pathID;

        //Delay the path call by one frame if it was sent the same frame as the previous call
        /*if (lastPathCall == Time.frameCount) {
            StartCoroutine (DelayPathStart (path));
            return p;
        }
		
        lastPathCall = Time.frameCount;*/

        //Pre process the path
        RunModifiers(ModifierPass.PreProcess, path);

        //Send the request to the pathfinder
        AstarPath.StartPath(path);

        return p;
    }

    public IEnumerator DelayPathStart(Path p)
    {
        yield return null;
        //lastPathCall = Time.frameCount;

        RunModifiers(ModifierPass.PreProcess, p);

        AstarPath.StartPath(p);
    }

#if !PhotonImplementation
    private int i;
    void Update()              //在该函数中进行画线
    {
        if (AIPath.ReachInSeeker) {
            flag_Reach = true;
            AIPath.ReachInSeeker = false;
        }
        if (lastCompletedNodePath == null || !drawGizmos)
        {
            return;
        }

        if (detailedGizmos)
        {
            Gizmos.color = new Color(0.7F, 0.5F, 0.1F, 0.5F);

            if (lastCompletedNodePath != null)
            {
                for (i = 0; i < lastCompletedNodePath.Count - 1; i++)
                {
                    Gizmos.DrawLine((Vector3)lastCompletedNodePath[i].position, 
                        (Vector3)lastCompletedNodePath[i + 1].position);
                }
            }
        }
        if (Exist_target == false)
        {
            //gameObject.GetComponent<LineRenderer>().active = false;
            //gameObject.RemoveComponent<LineRenderer>();
            lineRenderer = GetComponent<LineRenderer>();                //增加画线函数LineRenderer
            lineRenderer.SetVertexCount(0);
        }
        /* for (i = 0; i < lastCompletedVectorPath.Count - 1; i++)
         {
             points.Add(lastCompletedVectorPath[i]);
         }*/
        int Line_Num = 0;
        int k, j;
        double len;
        int num;
        double ax,ay,az;
        Vector3 vec;
        int length = 0;
        Vector3 po_people;
        //Vector3 poo = new Vector3(251.07f, 17.27324f, 177.3149f);
        if (lastCompletedVectorPath != null && Exist_target)
        {
            
            // print("lastCompletedVectorPath.Count = " + lastCompletedVectorPath.Count);
            // print("Is_target = " + Exist_target);
            if (gameObject != GameObject.Find("Move_People"))
            {
                lineRenderer = GetComponent<LineRenderer>();                //增加画线函数LineRenderer
                flash++;
                if (flash > 20)
                    flash = 0;
                if (flash < 10)
                {
                    lineRenderer.SetVertexCount(0);
                    lineRenderer.material = tax;
                    return;
                }
                else
                {
                    // flash = true;
                    //lineRenderer.SetVertexCount(0);
                    //lineRenderer.material = tax;
                    lineRenderer.SetVertexCount(lastCompletedVectorPath.Count);
                    //lastCompletedVectorPath[3] = lastCompletedVectorPath[2];
                    //lastCompletedVectorPath[2] = poo;
                    for (k = 0; k < lastCompletedVectorPath.Count; k++)
                    {
                        //Debug.DrawLine(transform.position, points[i], Color.green);
                        //Gizmos.DrawLine(lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1]);
                        if (k == 0)
                        {
                            po_people = people.position;
                            po_people.y -= 0.1f;
                            lastCompletedVectorPath[k] = po_people;
                        }
                        lineRenderer.SetPosition(k, lastCompletedVectorPath[k]);
                        // print("k = " + k);
                        // print("Points[k] = " + Points[k]);
                        //lineRenderer.SetPosition(i, Points[i]);  //把所有点添加到positions里
                    }
                }
               // else
               // {
                   /* for (i = 1; i < lastCompletedVectorPath.Count; i++)
                    {
                        len = Math.Sqrt(Math.Pow(lastCompletedVectorPath[i].x - lastCompletedVectorPath[i - 1].x, 2.0) + Math.Pow(lastCompletedVectorPath[i].z - lastCompletedVectorPath[i - 1].z, 2.0) + Math.Pow(lastCompletedVectorPath[i].y - lastCompletedVectorPath[i - 1].y, 2.0));
                        num = (int)len;
                        ax = (lastCompletedVectorPath[i].x - lastCompletedVectorPath[i - 1].x) / len;
                        ay = (lastCompletedVectorPath[i].y - lastCompletedVectorPath[i - 1].y) / len;
                        az = (lastCompletedVectorPath[i].z - lastCompletedVectorPath[i - 1].z) / len;
                        //print("len = " + len);
                        for (j = 1; j < len; j++)
                        {
                            vec.x = float.Parse((lastCompletedVectorPath[i - 1].x + ax * j).ToString());
                            vec.y = float.Parse((lastCompletedVectorPath[i - 1].y + ay * j).ToString());
                            vec.z = float.Parse((lastCompletedVectorPath[i - 1].z + az * j).ToString());
                            //Picture.transform.position = Vector3.MoveTowards(Picture.transform.position, vec, Time.deltaTime * 10);
                            //Picture.transform.position = vec;
                            //Picture.SimpleMove(vec);
                            Points[length++] = vec;
                            if (j % Distance == 0)
                            {
                                //lineRenderer[Line_Num] = GameObject.Find("people").GetComponent<LineRenderer>();                //获得画线函数LineRenderer
                                lineRenderer.SetVertexCount(Distance);
                                for (k = j - Distance; k < j; k++)
                                {
                                    //Debug.DrawLine(transform.position, points[i], Color.green);
                                    //Gizmos.DrawLine(lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1]);
                                    lineRenderer.SetPosition(Line_Num, Points[k]);
                                    Line_Num++;
                                    //lineRenderer.SetPosition(i, Points[i]);  //把所有点添加到positions里
                                }
                                Line_Num = 0;
                                //Line_Num++;
                            }
                        }

                    }*/
                //lastCompletedVectorPath.Count++;
               
                //lineRenderer.SetPosition(k+1, poo);
                   // flash = true;
                
                
                    /*if (flash)
                    {
                        flash = false;
                        lineRenderer.SetVertexCount(0);
                        return;
                    }
                    else
                    {
                       /* lineRenderer.SetVertexCount(length);
                        for (k = 0; k < length; k++)
                        {
                            //Debug.DrawLine(transform.position, points[i], Color.green);
                            //Gizmos.DrawLine(lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1]);
                            lineRenderer.SetPosition(k, Points[k]);
                            // print("k = " + k);
                            // print("Points[k] = " + Points[k]);
                            //lineRenderer.SetPosition(i, Points[i]);  //把所有点添加到positions里
                        }
                        length = 0;
                        flash = true;*/
                        /*lineRenderer.SetVertexCount(lastCompletedVectorPath.Count);         //开一个数量恰好合适的数组
                         for (i = 0; i < lastCompletedVectorPath.Count; i++)
                         {
                            //Debug.DrawLine(transform.position, points[i], Color.green);
                            //Gizmos.DrawLine(lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1]);
                            //print("lastCompletedVectorPath[i] = " + lastCompletedVectorPath[i]);
                             if (i == 0)
                             {
                                 po_people = people.position;
                                 po_people.y -= 0.6f;
                                 lastCompletedVectorPath[i] = po_people;
                             }
                           lineRenderer.SetPosition(i, lastCompletedVectorPath[i]);  //把所有点添加到positions里
                          // print("lastCompletedVectorPath = " + lastCompletedVectorPath[i]);
                        }
                         flash = true;
                    }*/
                    
                    /*Vector3 poo;
                    poo = gameObject.transform.position;
                    poo.y -= 0.9f;
                    poo.x -= 1.0f;
                    Picture.transform.position = poo;*/
                }
            }
            if (flag_Reach)
            {
                for (i = 0; i < lastCompletedVectorPath.Count; i++)
                {
                    ChangePoints = ChangePoints + lastCompletedVectorPath[i];
                }
                print("ChangePoints =" + ChangePoints);
                flag_Reach = false;
                //  Vector3 po;
                // ControlChange 
                // po = ControlChange.People_Position;
            }



            //lineRenderer.SetVertexCount(0);
            //points.Clear();
        }
        //lastCompletedVectorPath.Clear();
        /*Gizmos.color = new Color(0, 1F, 0, 1F);
        i = 0;
        if (lastCompletedVectorPath != null)
        {
            lineRenderer.SetVertexCount(lastCompletedVectorPath.Count);
            while (i < lastCompletedVectorPath.Count - 1)
            {
                lineRenderer.SetPosition(i, lastCompletedVectorPath[i]);
                //index++;
                i++;
            }
        }
        //Destroy(lineRenderer);
        if (lastCompletedVectorPath != null)
        {
            /*lineRenderer.SetVertexCount(lastCompletedVectorPath.Count);
            while (i < lastCompletedVectorPath.Count)
            {
                lineRenderer.SetPosition(index, lastCompletedVectorPath[index]);
                index++;
                i++;
            }
            i = 0;
            for (i = 0; i < lastCompletedVectorPath.Count - 1; i++)
            {
                /*lineRenderer.SetPosition(i, lastCompletedVectorPath[i]);
                index++;
                linePoints = new Vector3[2] { lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1] };
                VectorLine line = new VectorLine("Line", linePoints, null, 3.0f);
                line.Draw();
                Gizmos.DrawLine(lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1]);

            }
        }*/
    }
    /* void Update()
     {
         if (lastCompletedNodePath == null || !drawGizmos)
         {
             return;
         }
        
         if (detailedGizmos)
         {
             Gizmos.color = new Color(0.7F, 0.5F, 0.1F, 0.5F);

             if (lastCompletedNodePath != null)
             {
                 for (i = 0; i < lastCompletedNodePath.Count - 1; i++)
                 {
                     Gizmos.DrawLine((Vector3)lastCompletedNodePath[i].position, (Vector3)lastCompletedNodePath[i + 1].position);
                 }
             }
         }
         i = 0;
         if (lastCompletedVectorPath != null)
         {
             lineRenderer = GetComponent<LineRenderer>();
             lineRenderer.SetVertexCount(lastCompletedVectorPath.Count);
             while (i < lastCompletedVectorPath.Count - 1)
             {
                 lineRenderer.SetPosition(i, lastCompletedVectorPath[i]);
                 //index++;
                 i++;
             }
         }

     }*/
    /* for (int i = 0; i < lastCompletedVectorPath.Count - 1; i++)
     {
         Vector2[] linePoints = {new Vector2(lastCompletedVectorPath[i].x, lastCompletedVectorPath[i].y),   // 第一个点在屏幕最左边
      new Vector2(lastCompletedVectorPath[i + 1].x, lastCompletedVectorPath[i + 1].y)};
         linePoints = new Vector3[2] { lastCompletedVectorPath[i], lastCompletedVectorPath[i + 1] };
         VectorLine line = new VectorLine("Line", linePoints, tax, 20.0f);
         line.Draw();
         Gizmos.DrawLine (lastCompletedVectorPath[i],lastCompletedVectorPath[i+1]);
     }*/



#endif




