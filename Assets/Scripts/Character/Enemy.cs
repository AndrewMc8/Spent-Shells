using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected int attackCount;

    [SerializeField] Transform player;
    [SerializeField] protected float maxCombatDistance;
    [SerializeField] protected float minCombatDistance;
    [SerializeField] protected float speed = 4.0f;

    [SerializeField] protected float maxAttackRange;

    [SerializeField] protected float attackTime = 2.0f;
    [SerializeField] protected float attackTimer;

    [SerializeField] protected float damage = 10;

    [SerializeField] TriggerEvent triggered_event;


    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        base.update(Time.deltaTime);

        if(!dead)
        {
            Vector3 dir = (player.position - transform.position);

            animator.SetBool("combatDistance", dir.magnitude <= maxCombatDistance);

            {
                var lookPos = player.position - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);
            }

            float movement = 0;
            if (dir.magnitude <= maxCombatDistance)
            {
                if(dir.magnitude > minCombatDistance)
                {
                    Vector3 target = transform.forward * Mathf.Min(speed * Time.deltaTime, dir.magnitude);
                    movement = target.magnitude;
                    transform.position += target;
                }                
            }

            animator.SetFloat("speed", movement);

            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;

            if(attackTimer <= 0 && Vector3.Distance(transform.position, player.position) < maxAttackRange)
            {
                print("attacked");
                animator.SetInteger("attackMethod", Random.Range(1, attackCount));
                animator.SetTrigger("attack");

                player.gameObject.GetComponent<Health>().Damage(new Assets.Scripts.Items.Weapons.Damage(Assets.Scripts.Items.Weapons.DamageType.BLUNT, damage, 0));

                attackTimer = attackTime;
            }
        }
    }

    public override void Die()
    {
        Destroy(health);
        animator.SetTrigger("death");

        if (triggered_event)
            triggered_event.Trigger();
    }
}
