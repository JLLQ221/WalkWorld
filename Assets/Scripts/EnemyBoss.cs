using System.Collections;
using UnityEngine;

public class EnemyBoss : Enemy
{
    public Collider2D collisionAttack;

    private float distancePlayer;

    // Update is called once per frame
    void Update()
    {
        if (player == null) { return; }
        if (freeMove) { return; }
        speed = Guard();
        rgb2D.linearVelocityX = speed;
        animationEnemy.SetFloat("Speed", Mathf.Abs(speed));
    }

    private float Guard()
    {
        // Obtenemos la dirección del jugador y la nuestra, obtenemos el vector dirección que va de nosotros al jugador
        Vector3 direction = player.transform.position - transform.position;
        distancePlayer = Mathf.Abs(direction.x);
        speed = 0f;

        if (distancePlayer > 0.3f && distancePlayer < 1.3f)
        {
            // Lo que hacemos es calcular la distancia, posición, del jugador y del enemigo entonces
            // si el jugador esta atras del enemigo su posición es negativa por tanto el enemigo mirara a la
            // derecha de lo contrario mirara a la izquierda
            if (!attack)
            {
                transform.localScale = new Vector3((direction.x > 0.0f) ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
                this.speed = 1.3f * direction.x;
                directionWatch = 2;
                if (speed != 0 && Time.time > lastStep + 0.4f)
                {
                    lastStep = Time.time;
                    audio.PlayOneShot(enemyInfo.GetSound(EnemySoundType.Step));
                }
            }
        }
        else if (distancePlayer < 0.3f)
        {
            if (!attack && Mathf.Sign(direction.x) == Mathf.Sign(transform.localScale.x))
            {
                Attack();
            }
        }
        else
        {
            if (!attack && distancePlayer > 1.3f)
            {
                Watch();
            }
        }

        return speed;
    }

    public void EndAttack()
    {
        EnableAttackCollider();
        attack = false;
        animationEnemy.SetBool("Attack", false);
        collisionAttack.enabled = false;
    }

    public void EnableAttackCollider()
    {
        collisionAttack.enabled = true;
        collisionAttack.GetComponent<CollisionWithPlayer>().Scale(transform.localScale.x);
        StartCoroutine(OffAttackCollider());
    }

    IEnumerator OffAttackCollider()
    {
        yield return new WaitForSeconds(0.01f);
        collisionAttack.enabled = false;
    }
}
