using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField]
    private CharacterData data;
    public CharacterType type { get; private set; }
    public CharacterLevel level { get; private set; }
    private CharacterStates state;
    private Animator animator;
    private int attack;
    public float health;
    [SerializeField]
    private GameObject bullet;
    private GameObject target;
    NavMeshAgent agent;
    void Start()
    {
        state = CharacterStates.Idle;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = data.health;
        attack = data.attack;
        type = data.type;
        level = data.level;
        
        gameData.enemies.Add(this.gameObject);
    }
    private void OnEnable()
    {
        EventManager.startGame += StartFight;
        EventManager.characterDeath += ChangeTarget;
        EventManager.attackByPlayer += Damage;
    }
    private void OnDisable()
    {
        EventManager.startGame -= StartFight;
        EventManager.characterDeath -= ChangeTarget;
        EventManager.attackByPlayer -= Damage;
    }
   
    IEnumerator OnStartGame()
    {
        while (health > 0)
        {
            switch (state)
            {
                case CharacterStates.Idle:
                    animator.SetBool("Running", false);
                    animator.SetBool("Attack", false);
                    break;
                case CharacterStates.Running:
                    animator.SetBool("Running", true);
                    animator.SetBool("Attack", false);
                    break;
                case CharacterStates.Attack:
                    animator.SetBool("Running", false);
                    animator.SetBool("Attack", true);
                    break;
            }


            if (gameData.players.Count == 0)
            {
                animator.SetBool("Running", false);
                animator.SetBool("Attack", false);
                agent.Stop();
            }
            yield return new WaitForEndOfFrame();
        }
        gameData.enemies.Remove(this.gameObject);
        EventManager.characterDeath?.Invoke();
        gameData.money += 50;
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
    void StartFight()
    {
        StartCoroutine(OnStartGame());
        if (!target && gameData.players.Count > 0)
        {
            target = gameData.players[0];

        }
        for (int i = 0; i < gameData.players.Count; i++)
        {
            if (Vector3.Distance(transform.position, gameData.players[i].transform.position) < Vector3.Distance(transform.position, target.transform.position))
            {
                target = gameData.players[i];
            }
        }
        if (type == CharacterType.Melee)
            StartCoroutine(StartMove());
        else
        {
            StartCoroutine(StartFightRanged());
        }
    }
    GameObject oldTarget;
    void ChangeTarget()
    {
        state = CharacterStates.Idle;
        StopCoroutine(StartMove());
        StopCoroutine(StartFightRanged());
        oldTarget = target;

        if (gameData.players.Count > 0)
        {
            target = gameData.players[0];

            for (int i = 0; i < gameData.players.Count; i++)
            {

                if (Vector3.Distance(transform.position, gameData.players[i].transform.position) < Vector3.Distance(transform.position, target.transform.position) && gameData.players[i] != oldTarget)
                {
                    target = gameData.players[i];
                }
            }

            if (type == CharacterType.Melee)
                StartCoroutine(StartMove());
            else
            {
                StartCoroutine(StartFightRanged());
            }
        }

    }
    IEnumerator StartMove()
    {
        
        while (true)
        {
            if (target)
            {
                agent.SetDestination(target.transform.position);

                if (Vector3.Distance(transform.position, target.transform.position) <= 2f)
                {
                    state = CharacterStates.Attack;
                    transform.LookAt(target.transform.position);
                    oldTarget = target;
                    yield return new WaitForSeconds(1f);
                    if (target&& oldTarget==target)
                    {
                        EventManager.attackByEnemy?.Invoke(target, attack);
                    }
                }
                else
                {
                    state = CharacterStates.Running;
                }

            }

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator StartFightRanged()
    {

        while (true)
        {
            if (target)
            {
                transform.LookAt(target.transform.position);
                oldTarget = target;
                yield return new WaitForSeconds(1f);
                if (target && oldTarget == target)
                {
                    transform.LookAt(target.transform.position);
                    GameObject obj = Instantiate(bullet, transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = null;
                    obj.GetComponent<BulletScript>().SetTarget("Player", attack, target);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }


    void Damage(GameObject obj, int damage)
    {
        if (this.gameObject==obj)
        {
            health -= damage;
        }

    }
}
