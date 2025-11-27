using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBasic : Enemy
{
    public GameObject player;
    public GameObject bulletPrefat;

    private float distancePlayer;
    private bool attack = false;

    private Coroutine coroutineWatch;
    private bool continuoWatch = true;

    // Update is called once per frame
    void Update()
    {
        if (player == null) { return; }
        if (freeMove) { return; }
        speed = GuardPause(0.8f, 1.2f);
        rgb2D.linearVelocityX = speed;
        animationEnemy.SetFloat("Speed", Mathf.Abs(speed));
    }

    private float GuardPause(float minVision, float maxVision)
    {
        // Obtenemos la dirección del jugador y la nuestra, obtenemos el vector dirección que va de nosotros al jugador
        Vector3 direction = player.transform.position - transform.position;
        distancePlayer = Mathf.Abs(direction.x);
        speed = 0f;

        if (distancePlayer > minVision && distancePlayer < maxVision)
        {
            // Lo que hacemos es calcular la distancia, posición, del jugador y del enemigo entonces
            // si el jugador esta atras del enemigo su posición es negativa por tanto el enemigo mirara a la
            // derecha de lo contrario mirara a la izquierda
            transform.localScale = new Vector3((direction.x > 0.0f) ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
            speed = 1.3f * direction.x;
            continuoWatch = false;
        }
        else if (distancePlayer < minVision)
        {
            if (!attack && Mathf.Sign(direction.x) == Mathf.Sign(transform.localScale.x))
            {
                Attack();
                continuoWatch = false;
            }
            else
            {
                transform.localScale = new Vector3((direction.x > 0.0f) ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            if (!continuoWatch && coroutineWatch.IsUnityNull() && distancePlayer > maxVision && !attack)
            {
                coroutineWatch = StartCoroutine(ContinuWatchCorutine());
            }

            if (distancePlayer > maxVision && continuoWatch)
            {
                Watch();
            }
        }

        return speed;
    }

    IEnumerator ContinuWatchCorutine()
    {
        yield return new WaitForSeconds(1.5f);
        continuoWatch = true;
        coroutineWatch = null;
    }

    private void Attack()
    {
        attack = true;
        animationEnemy.SetBool("Attack", attack);
    }

    public void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector2.right;
        else direction = Vector2.left;

        Vector2 scale;
        scale = new Vector2(Mathf.Sign(transform.localScale.x) * 0.4f, 0.4f);
        GameObject bullet = Instantiate(bulletPrefat, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletEnemy>().Scale(scale);
    }
    public void EndAttack()
    {
        animationEnemy.SetBool("Attack", false);
        attack = false;
    }
}
