using UnityEngine;

public class Breakeble : MonoBehaviour
{
    [SerializeField] private GameObject pieces;

    public void SpawnPieces()
    {
        pieces.gameObject.SetActive(true);
        gameObject.gameObject.SetActive(false);
    }
}
