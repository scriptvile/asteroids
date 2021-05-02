using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour where T : Component, IPoolable
{
    // Enums
    enum DeficitOption { IncreasePool, CancelRequest }

    // Singleton
    public static Pool<T> i = null;

    // Inspector
    [Header("References")]
    [SerializeField] T prefab;
    [SerializeField] Transform inactiveRoot;
    [SerializeField] Transform spawnRoot;

    [Header("Settings")]
    [SerializeField] DeficitOption deficitOption;
    [SerializeField] int initialAmount;
    [SerializeField] bool returnAllOnGameRestart;

    // Privates
    Queue<T> inactive = new Queue<T>();
    HashSet<T> active = new HashSet<T>();

    // Properties
    public T Prefab { get { return prefab; } }

    void Awake()
    {
        if (i == null) i = this;
        else if (i != this)
        {
            Debug.LogError("Singleton duplicate: " + this.gameObject.name);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AddToPool(initialAmount);
    }

    protected virtual void OnEnable()
    {
        Game.OnStateChanged += OnGameStateChanged;
    }

    protected virtual void OnDisable()
    {
        Game.OnStateChanged -= OnGameStateChanged;
    }

    public void AddToPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T newObj = Instantiate(prefab);
            newObj.transform.SetParent(inactiveRoot);
            newObj.gameObject.SetActive(false);
            inactive.Enqueue(newObj);
        }
    }

    public T Request()
    {
        T ReturnObj()
        {
            T obj = inactive.Dequeue();

            active.Add(obj);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(spawnRoot);
            obj.Spawn();
            return obj;
        }
        if (inactive.Count > 0) return ReturnObj();     // Return any inactive object.

        else                                            // Otherwise create more instances or cancel the request.
        {
            switch (deficitOption)
            {
                case DeficitOption.IncreasePool:
                    AddToPool(1);
                    return ReturnObj();
                case DeficitOption.CancelRequest: return null;
                default:
                    Debug.LogError("Unexpected DecifitOption.");
                    return null;
            }
        }
    }

    public T Request(Vector2 position)
    {
        T obj = Request();
        obj.transform.position = position;
        return obj;
    }

    public HashSet<T> Request(int count)
    {
        HashSet<T> objs = new HashSet<T>();
        for (int i = 0; i < count; i++)
        {
            objs.Add(Request());
        }

        return objs;
    }


    public HashSet<T> Request(int count, Queue<Vector2> positions)
    {
        HashSet<T> objs = new HashSet<T>();
        for (int i = 0; i < count; i++)
        {
            objs.Add(Request(positions.Dequeue()));
        }

        return objs;
    }

    public void ReturnToPool(T obj)
    {
        if (active.Contains(obj)) active.Remove(obj);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(inactiveRoot);
        inactive.Enqueue(obj);
    }

    void OnGameStateChanged(Game.State gameState)
    {
        if (returnAllOnGameRestart)
        {
            if (gameState == Game.State.Preparation) StartCoroutine(ReturnAllToPoolLate());
        }
    }

    IEnumerator ReturnAllToPoolLate()
    {
        yield return new WaitForEndOfFrame();

        List<T> toReturn = new List<T>();
        foreach (T obj in active)                   // Cannot remove elements from hashset when using foreach loop, so move them to the list.
        {
            toReturn.Add(obj);
        }

        for (int i = 0; i < toReturn.Count; i++)
        {
            ReturnToPool(toReturn[i]);
        }
    }

}
