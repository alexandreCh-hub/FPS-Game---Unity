using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_control : MonoBehaviour
{

    public NavMeshAgent agent;
    private Transform player;
    public LayerMask whatIsPlayer;

    //Gestion des animations
    private Animator anim;

    //Patrouille
    //point de destination
    private Vector3 walkPoint;
    //point valide ou non
    bool walkPointSet = false;
    //rayon dans lequel l'IA va se déplacer
    public float walkPointRange;

    public float runSpeed;

    //Attaque
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //Etat
    //rayon de recherche et d'attaque : Rayon d'attaque < rayon de recherche
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    //initialisation du joueur et de l'IA
    private void Awake(){
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //Verifie si le joueur est dans la zone de chasse
        //centre de recherche : ici la position de l'IA, rayon de recherche et le LayerMask
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //Verifie si le joueur est dans la zone d'attaque
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange) {Patroling();}
        if(playerInSightRange && !playerInAttackRange) {Chasing();}
        if(playerInAttackRange) {Attacking();}
    }

    
    //Gere les deplacements aléatoires de l'IA
    private void Patroling(){
        if(!walkPointSet){
            RandomPoint(transform.position, 20, out walkPoint);
            }else{
            //déplace l'IA
            agent.SetDestination(walkPoint);
            //redfini le point s'il est inaccessible par l'IA
            if(!agent.hasPath){
                walkPointSet = false;
            }
        }
        //Calcule la distance entre l'IA et le point voulu
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Le point est atteint
        if(distanceToWalkPoint.magnitude < 0.1f) walkPointSet = false;
    }
    //Defini le point de destination
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, agent.areaMask))
            {
                result = hit.position;
                walkPointSet = true;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    //Gere quand l'IA poursuit le joueur 
    private void Chasing(){

        agent.SetDestination(player.position);
        transform.LookAt(player.position);
        agent.velocity = agent.transform.forward * runSpeed;
    }

    //Gere quand l'IA attaque le joueur
    private void Attacking(){
        //Immobilise l'IA quand il tire
        agent.SetDestination(transform.position);
        //Oriente l'IA
        transform.LookAt(player);

        //Gestion de l'attaque
        if(!alreadyAttacked){

            //Animation de l'attaque

            //delai entre chaque attaque defini par timeBetweenAttacks
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    //permet de relancer une attaque
    private void ResetAttack(){
        alreadyAttacked = false;
    }
}
