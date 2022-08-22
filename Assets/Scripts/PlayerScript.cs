using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private CharacterData data;
    Character character = new Character();
    public CharacterType type { get; private set; }
    public CharacterLevel level { get; private set; }
    private CharacterStates state;
    private Animator animator;
    private int attack;
    private float health;
    [SerializeField]
    private GameObject bullet;
    private string charachterType;
    private GameObject triggeredCharacter;
    [HideInInspector]
    public GameObject lastGrid;
    private GameObject target;
    NavMeshAgent agent;

    private void Start()
    {
        state = CharacterStates.Idle;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        triggeredCharacter = null;
        charachterType = type.ToString() + level.ToString();
        health = data.health;
        attack = data.attack;
        type = data.type;
        level = data.level;
        //gameObject.tag = charachterType;
        if (SceneManager.GetActiveScene().name != "Level 1")
        {
           
            gameData.players.Add(this.gameObject);
        }

    }



    private void OnEnable()
    {
        EventManager.goFightScene += GoFightScene;
        EventManager.startGame += StartFight;
        EventManager.characterDeath += ChangeTarget;
        EventManager.attackByEnemy += Damage;
    }
    private void OnDisable()
    {
        EventManager.goFightScene -= GoFightScene;
        EventManager.startGame -= StartFight;
        EventManager.characterDeath -= ChangeTarget;
        EventManager.attackByEnemy -= Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().name == "Level 1")
        {
            if (other.GetComponent<PlayerScript>())
            {
                if (other.GetComponent<PlayerScript>().type == type && other.GetComponent<PlayerScript>().level == level)
                {
                    triggeredCharacter = other.gameObject;
                }
            }

            else if (other.tag == "Grid")
            {
                if (!other.GetComponent<GridScript>().isFull)
                {
                    lastGrid = other.gameObject;
                    EventManager.setGrid?.Invoke(other.gameObject);
                }

            }
        }
        
    }
    private void OnMouseUp()
    {

        if (triggeredCharacter && level != CharacterLevel.Level3)
        {
            EventManager.doMerge?.Invoke(type, level, triggeredCharacter.transform.position, triggeredCharacter.GetComponent<PlayerScript>().lastGrid);
            Destroy(triggeredCharacter);
            Destroy(this.gameObject);

        }
        else
        {
            GetComponent<CapsuleCollider>().isTrigger = false;
            Vector3 pos = new Vector3(0, transform.position.y + 1f, 0);
            transform.position = lastGrid.transform.position + pos;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerScript>())
        {
            if (other.GetComponent<PlayerScript>().type == type && other.GetComponent<PlayerScript>().level == level)
            {
                triggeredCharacter = null;
            }

        }

        if (other.tag == "Grid")
        {

        }
    }
    IEnumerator OnStartGame()
    {
        while (health > 0)
        {
            switch(state)
            {
                case CharacterStates.Idle:
                    animator.SetBool("Running",false);
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


            if (gameData.enemies.Count == 0)
            {
                animator.SetBool("Running", false);
                animator.SetBool("Attack", false);
                agent.Stop();
            }
            yield return new WaitForEndOfFrame();
        }
        gameData.players.Remove(this.gameObject);
        EventManager.characterDeath?.Invoke();
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    void StartFight()
    {
        StartCoroutine(OnStartGame());
        if (!target && gameData.enemies.Count > 0)
        {
            target = gameData.enemies[0];
           
        }
        for (int i = 0; i < gameData.enemies.Count; i++)
        {
            if (Vector3.Distance(transform.position, gameData.enemies[i].transform.position) < Vector3.Distance(transform.position, target.transform.position))
            {
                target = gameData.enemies[i];
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

        if (gameData.enemies.Count > 0)
        {
            target = gameData.enemies[0];

            for (int i = 0; i < gameData.enemies.Count; i++)
            {

                if (Vector3.Distance(transform.position, gameData.enemies[i].transform.position) < Vector3.Distance(transform.position, target.transform.position) && gameData.enemies[i] != oldTarget)
                {
                    target = gameData.enemies[i];
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
                    if (target && oldTarget == target)
                    {
                        EventManager.attackByPlayer?.Invoke(target,attack);
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
    GameObject obj;
    IEnumerator StartFightRanged()
    {

        while (true)
        {
            if (target)
            {
                transform.LookAt(target.transform.position);
                oldTarget = target;
                yield return new WaitForSeconds(1f);
                if (target && oldTarget == target&& obj==null)
                {
                    transform.LookAt(target.transform.position);
                    obj= Instantiate(bullet,transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.parent = null;
                    obj.GetComponent<BulletScript>().SetTarget("Enemy",attack,target);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    void Damage(GameObject obj, int damage)
    {

        if (this.gameObject == obj)
        {
            health -= damage;
        }

    }

    void GoFightScene()
    {
        character.type = type;
        character.level = level;
        character.position = transform.position;

        gameData.characters.Add(character);
    }


}
