using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButtonObjectPoolTutorial : MonoBehaviour
{
    public static ArrowButtonObjectPoolTutorial current;

    [Tooltip("Assign the arrow prefab.")]
    public InditorTutorial pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    public List<InditorTutorial> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<InditorTutorial>();
        for (int i = 0; i < pooledAmount; i++)
        {
            InditorTutorial arrow = Instantiate(pooledObject);
            arrow.transform.SetParent(transform, false);
            arrow.Activate(false);
            pooledObjects.Add(arrow);
        }
    }

    /// <summary>
    /// Gets pooled objects from the pool.
    /// </summary>
    /// <returns></returns>
    public InditorTutorial GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            InditorTutorial arrow = Instantiate(pooledObject);
            arrow.transform.SetParent(transform, false);
            arrow.Activate(false);
            pooledObjects.Add(arrow);
            return arrow;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (InditorTutorial arrow in pooledObjects)
        {
            arrow.Activate(false);
        }
    }
}
