using UnityEngine;

public class CollisionWithPlayer : MonoBehaviour
{
    float direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(direction);
        }
    }

    public void Scale (float scaleX)
    {
        direction = scaleX;
    }
}
