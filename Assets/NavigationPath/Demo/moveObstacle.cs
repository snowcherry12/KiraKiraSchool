using UnityEngine;

public class moveObstacle : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Rotate(new Vector3(0f, 1f, 0f) * speed * Time.deltaTime);
    }
}
