using UnityEngine;

public class Detector : Item
{
    [Header("Detector")]
    [SerializeField] private float distance;
    [SerializeField] private float beepInterval = 0.5f;

    AudioSource audioSource;
    Collider[] colliders = new Collider[20];

    bool isInHands = false;
    public void HandsTake(bool isTake) { isInHands = isTake; }

    private float timer = 0f;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isInHands)
            SeekStuff();
    }

    void SeekStuff()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders);
        bool found = false;

        for (int i = 0; i < count; i++)
        {
            Collider col = colliders[i];

            // Пропускаем самого себя!
            if (col.gameObject == gameObject)
                continue;

            // Проверяем, есть ли Item или Puzzle
            if (col.GetComponentInParent<Item>() != null || col.GetComponent<Puzzle>() != null)
            {
                // Дополнительная проверка: это не сам детектор
                Item item = col.GetComponentInParent<Item>();
                if (item != null && item.gameObject == gameObject)
                    continue;

                found = true;
                break;
            }
        }

        if (found)
        {
            timer += Time.deltaTime;

            if (timer >= beepInterval)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}