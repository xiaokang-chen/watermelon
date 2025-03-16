using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonObjectPoolMgr : MonoBehaviour
{
    public GameObject[] watermelonPrefabs; // 包含所有级别的西瓜预制件
    public GameObject blastAnimPrefab;
    public GameObject comboAnimPrefab;

    private readonly float cleanupInterval = 5;
    private readonly float maxIdleTime = 20;
    private readonly int maxPool = 3;

    private readonly Dictionary<WatermelonObjectType, List<GameObject>> objPoolsDict = new();

    public int MaxWatermelonPrefabsCount { get { return watermelonPrefabs.Length; } }

    public static WatermelonObjectPoolMgr Instance;

    public GameObject EndCoin;
    public ParticleSystem EndCoinParticle;

    private void Awake()
    {
       Instance = this;
       StartCoroutine(CleanupCoroutine());
    }



    // Start is called before the first frame update
    // void Start() { }

    public GameObject Get(WatermelonObjectType objectType) {

        if(objectType == WatermelonObjectType.Max){
            EndCoin.SetActive(true);
            EndCoinParticle.Play();
            // close it after 2 second
            StartCoroutine(CloseEndCoin(2));
        }

        List<GameObject> pool;
        if (objPoolsDict.ContainsKey(objectType))
        {
            pool = objPoolsDict[objectType];
        }
        else
        {
            pool = new();
            objPoolsDict.Add(objectType, pool);
        }     

        if (0 < pool.Count)
        {
            GameObject obj = pool[0];
            pool.RemoveAt(0);
            if (WatermelonObjectType.WatermelonBegin <= objectType && objectType < WatermelonObjectType.WatermelonEnd) {
                obj.GetComponent<Watermelon>().hasMerged = false;
            }
            obj.SetActive(true);
            //obj.transform.SetParent(gameObject.transform);
            return obj;
        }
        GameObject prefab;
        if (WatermelonObjectType.Blast == objectType)
        {
            prefab = blastAnimPrefab;
        }
        else if (WatermelonObjectType.Combo == objectType)
        {
            prefab = comboAnimPrefab;
        }
        else {
            prefab = watermelonPrefabs[(int)objectType];
        }

        if (null == prefab) return null;

        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        if (!newObj.TryGetComponent<WatermelonObject>(out _))
        {
            WatermelonObject cls = newObj.AddComponent<WatermelonObject>();
            cls.objectType = objectType;
        }
        return newObj;
    }

    private IEnumerator CloseEndCoin(float seconds){
        yield return new WaitForSeconds(seconds);
        EndCoin.SetActive(false);
    }

    public void Put(GameObject obj)
    {
        obj.SetActive(false);
        //obj.transform.SetParent(null); // 移除父节点

        var cls = obj.GetComponent<WatermelonObject>();
        var objectType = cls.objectType;
        cls.lastActiveTimestamp = Time.time; 
        objPoolsDict[objectType].Add(obj);
    }

    // Update is called once per frame
    // void Update() {}

    private IEnumerator CleanupCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cleanupInterval);

            float currentTime = Time.time;
            var tmpDict = objPoolsDict;
            foreach(var e in tmpDict)
            {
                var list = e.Value;
                var len = list.Count;
                if (len <= maxPool) { continue; }
                for (var i = 0; i < len; i++)
                {
                    var o = list[i];
                    if (o.activeSelf) continue;
                    var cls = o.GetComponent<WatermelonObject>();
                    if (cls.lastActiveTimestamp + maxIdleTime < currentTime) {
                        e.Value.RemoveAt(i);
                        i--;
                        len--;
                        Destroy(o);
                    }
                    if (len <= maxPool)
                    {
                        break;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        objPoolsDict.Clear();
    }
}
