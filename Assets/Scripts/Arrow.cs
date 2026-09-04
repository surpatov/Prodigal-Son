using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    private float lifeTimer;

    void Start()
    {
        lifeTimer = lifetime;
    }
    void Update()
    {
        if (lifeTimer > 0)
        {
            transform.Rotate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
            lifeTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
