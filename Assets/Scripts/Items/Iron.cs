using UnityEngine;

public class Iron : Item
{
    [SerializeField] private MeshRenderer child_mesh;
    [SerializeField] private MeshRenderer own_mesh;
    [SerializeField] private Inventory inventory;
    [SerializeField] private float speed;

    [SerializeField] private Vector3 rootPos;
    [SerializeField] private Quaternion rootRot;

    [SerializeField] private ParticleSystem smoke;

    private bool isPressing = false; // Добавляем флаг состояния

    protected override void Update()
    {
        base.Update();

        // Контроль партиклов в зависимости от состояния
        if (isPressing && Input.GetMouseButton(0))
        {
            if (!smoke.isPlaying)
                smoke.Play();
        }
        else
        {
            if (smoke.isPlaying)
                smoke.Stop();
        }
    }

    public void Press(Note note, Inventory inv)
    {
        inventory = inv;
        isPressing = true; // Устанавливаем флаг

        if (note.TimeLeft > 0)
        {
            rootPos = note.transform.position;
            rootRot = note.transform.rotation;
            note.Pressing();
            RootPosition(note);
        }
        else
        {
            isPressing = false;
            smoke.Stop();
        }
    }

    private void RootPosition(Note _note)
    {
        inventory.canRoot = false;

        GetComponent<Animator>().SetBool("isPressing", true);
        own_mesh.enabled = false;
        child_mesh.enabled = true;

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        var step = speed * 2 * Time.deltaTime;
        var rotStep = speed * 200 * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, rootPos, step);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rootRot, rotStep);
    }
}