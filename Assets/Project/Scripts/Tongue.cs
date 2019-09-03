using UnityEngine;

public class Tongue : MonoBehaviour
{
    [SerializeField]
    private Toad toad;

    [SerializeField]
    private GameManager game;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Firefly")) return;

        var points = other.GetComponent<Firefly>().Points;
        game.AddPoints(toad.Player, points);

        Destroy(other.gameObject);
    }
}