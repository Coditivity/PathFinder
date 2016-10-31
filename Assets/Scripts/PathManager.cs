using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager:MonoBehaviour {
    
    public Grid.Layer_WeightPair[] layerWeights;
    Queue jobQueue;
    bool isFindingPath;
    private static PathManager pathManager = null;
    
    public static PathManager instance
    {
        get
        {
            return pathManager;
        }        
    }
    
    void Awake()
    {
        PathFinder.Init();
        jobQueue = new Queue();
        if (pathManager == null)
        {
            pathManager = (PathManager)FindObjectOfType<PathManager>();
            if (pathManager == null)
            {
                Debug.LogError("No PathFinder instance found. Please attach a PathFinder script to any gameobject");
            }
        }
    }

	public static void AddForPathFinding(PathFindJob job)
    {
        job.grid.Reset();
        instance.jobQueue.Enqueue(job);
    }

    public static void StartPathFinding()
    {
        if (!instance.isFindingPath)
        {
            instance.isFindingPath = true;
            instance.StartCoroutine(FindPath());
        }
    }

    static IEnumerator FindPath()
    {
        while(instance.jobQueue.Count>0)
        {
            PathFindJob job = (PathFindJob)instance.jobQueue.Dequeue();
            PathFinder.SetAndStart(job);
            
            if(instance.jobQueue.Count==0)
            {
                instance.isFindingPath = false;
            }
            yield return null;            
        }
        
    }

    

    
}
