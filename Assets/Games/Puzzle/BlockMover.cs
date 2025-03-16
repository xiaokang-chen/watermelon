using UnityEngine;

public class BlockMover : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(5, 4); // 默认网格大小为5x4
    public Vector2 blockSize = new Vector2(1, 1); // 默认滑块大小为1x1
    public Vector2 offset = new Vector2(0.5f, 0.5f); // 网格的偏移量

    private Vector2 initialMousePosition;
    private Vector2 initialBlockPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialBlockPosition = transform.position;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && initialMousePosition != Vector2.zero)
        {
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distanceMoved = currentMousePosition - initialMousePosition;

            Vector2 newPosition = initialBlockPosition + distanceMoved;
            newPosition = ClampPositionToGrid(newPosition + offset) - offset; // 应用偏移，然后在最后减去偏移量以还原实际位置

            if (!IsPositionOccupied(newPosition))
            {
                transform.position = newPosition;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            initialMousePosition = Vector2.zero;
        }
    }

    Vector2 ClampPositionToGrid(Vector2 position)
    {
        float maxX = gridSize.x - blockSize.x;
        float maxY = gridSize.y - blockSize.y;

        float clampedX = Mathf.Clamp(position.x, 0, maxX);
        float clampedY = Mathf.Clamp(position.y, 0, maxY);

        float closestX = FindClosestGridPosition(clampedX, gridSize.x, blockSize.x);
        float closestY = FindClosestGridPosition(clampedY, gridSize.y, blockSize.y);

        return new Vector2(closestX, closestY);
    }

    float FindClosestGridPosition(float position, float gridSize, float blockSize)
    {
        float gridPosition = Mathf.Round((position / (gridSize - blockSize)) * (gridSize - 1));
        return Mathf.Clamp(gridPosition, 0, gridSize - 1);
    }

    bool IsPositionOccupied(Vector2 position)
    {
        Collider2D collider = Physics2D.OverlapBox(position, blockSize, 0);
        return collider != null && collider.gameObject != gameObject;
    }
}