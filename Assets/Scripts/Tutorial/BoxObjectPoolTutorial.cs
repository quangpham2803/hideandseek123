using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObjectPoolTutorial : MonoBehaviour
{
    public static BoxObjectPoolTutorial current;

    [Tooltip("Assign the box prefab.")]
    public InditorTutorial pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    List<InditorTutorial> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<InditorTutorial>();

        for (int i = 0; i < pooledAmount; i++)
        {
            InditorTutorial box = Instantiate(pooledObject);
            box.transform.SetParent(transform, false);
            box.Activate(false);
            pooledObjects.Add(box);
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
            InditorTutorial box = Instantiate(pooledObject);
            box.transform.SetParent(transform, false);
            box.Activate(false);
            pooledObjects.Add(box);
            return box;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (InditorTutorial box in pooledObjects)
        {
            box.Activate(false);
        }
    }
}
