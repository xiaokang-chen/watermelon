using UnityEngine;

public class ApplyForceInRange : MonoBehaviour
{
    public float forceMagnitude = 10f; // 力的大小
    public float radius = 5f; // 作用范围的半径
    public Vector2 forceDirection = Vector2.up; // 力的方向
    public LayerMask layerMask; // 用于检测的图层

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) // 按下空格键（或替换成你的按钮事件）
        // {
        //     ApplyForce();
        // }
    }

    public void ApplyForce()
    {
        // 查找范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 对每个找到的物体施加力
                rb.AddForce(forceDirection.normalized * forceMagnitude, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 绘制作用范围的Gizmos（用于在编辑器中可视化范围）
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}