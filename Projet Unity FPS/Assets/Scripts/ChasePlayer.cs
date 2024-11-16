using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{

    GameObject player;
    NavMeshAgent agent;
    //defini le jouer et l'ia
    void Start()
    {
     player = GameObject.FindWithTag("Player");
     agent = this.GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {
        //permet de suivre le joueur en temp reel
        agent.SetDestination((player.transform.position));
    }
}
