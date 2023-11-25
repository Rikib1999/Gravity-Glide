using UnityEngine;

public class DraggableGravity : MonoBehaviour
{
    Vector3 mousePosOffset;

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePosOffset = transform.root.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (LevelManager.IsPlaying) return;
        transform.root.position = GetMouseWorldPosition() + mousePosOffset;
    }

    private void OnMouseUp()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100.0f, 1 << 6);

        if (!hit) Destroy(gameObject.transform.root.gameObject);
    }
}