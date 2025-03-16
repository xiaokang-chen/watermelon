using DG.Tweening;
using UnityEngine;

public class Watermelon : MonoBehaviour
{
    public int level; // 表示西瓜的大小级别
    public bool hasMerged = false; // 新增标志，表示西瓜是否已经合并
    public float blastScale = 1.0f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    // void Update() {}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Watermelon collidedWatermelon = collision.gameObject.GetComponent<Watermelon>();
        Watermelon thisWatermelon = gameObject.GetComponent<Watermelon>();

        Debug.Log("collision" + collision.gameObject.name + " gameObject " + gameObject.name);

        if (collision.gameObject.name.Equals("Bottom") || null != collidedWatermelon)
        {
            WatermelonGameController.Instance.CreateNowWatermelon(gameObject);
        }

        WatermelonGameController.Instance.PlayCollisionAudioEffect();

        // 确保两个西瓜都存在，并且等级相同，并且至少有一个西瓜还没有合并
        if (collidedWatermelon != null && thisWatermelon != null &&
            thisWatermelon.level == collidedWatermelon.level &&
            (!thisWatermelon.hasMerged || !collidedWatermelon.hasMerged))
        {
            // 将两个西瓜标记为已合并
            thisWatermelon.hasMerged = true;
            collidedWatermelon.hasMerged = true;

            if (level + 1 < WatermelonObjectPoolMgr.Instance.MaxWatermelonPrefabsCount)
            {
                if (0 < WatermelonGameController.Instance.Combo)
                {
                    var comboObj = WatermelonObjectPoolMgr.Instance.Get(WatermelonObjectType.Combo);
                    comboObj.transform.localPosition = new Vector3();
                    comboObj.GetComponent<Animator>().Play("combo_01");
                    DOVirtual.DelayedCall(1.0f, () =>
                    {
                        WatermelonObjectPoolMgr.Instance.Put(comboObj);
                    });
                }
                WatermelonObjectPoolMgr.Instance.Put(collision.gameObject);
                //var size = Get2DObjectSize(gameObject);
                var blastObj = WatermelonObjectPoolMgr.Instance.Get(WatermelonObjectType.Blast);
                blastObj.transform.localScale = new Vector3(blastScale, blastScale, 1.0f);
                blastObj.transform.localPosition = collision.gameObject.transform.localPosition;
                WatermelonGameController.Instance.PlayBlastAudioEffect();
                DOVirtual.DelayedCall(1.0f, () =>
                {
                    WatermelonObjectPoolMgr.Instance.Put(blastObj);
                });
                WatermelonObjectPoolMgr.Instance.Put(gameObject);

                WatermelonGameController.Instance.Combo++;

                // 创建一个新的更大的西瓜
                WatermelonGameController.Instance.Score += 2 * (level + 1) * 50;
                WatermelonGameController.Instance.CreateWatermelon(thisWatermelon.level + 1, transform.position);
            }
        }
    }

    void FixedUpdate()
    {
        float minAngularVelocity = 0.1f;  // 定义一个最小角速度阈值
        if (Mathf.Abs(rb.angularVelocity) < minAngularVelocity)
        {
            rb.angularVelocity = minAngularVelocity;  // 施加一个小的角速度
        }
    }

    /*Vector2 Get2DObjectSize(GameObject obj)
    {
        // 获取对象的 SpriteRenderer 组件
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // 获取精灵的大小（单位是世界坐标）
            return spriteRenderer.bounds.size;
        }

        // 返回一个默认值
        return Vector2.one;
    }*/
}
