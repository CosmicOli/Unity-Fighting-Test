using JetBrains.Annotations;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxEnemyBehavior : GenericGravityEntityBehaviour, EntityAIInterface, ContactDamageInterface
{
    // Variables required by ContactDamageInterface
    public GameObject[] players { get; set; }
    public GameObject targetedPlayer { get; set; }
    public PlayerBehaviour playerBehaviour { get; set; }
    public bool stillInContact { get; set; }
    public Collider2D currentCollision { get; set; }

    // Constant passed in through the inspector to define contact damage
    [SerializeField]
    private float ContactDamageValue;
    // Constant required by ContactDamageInterface
    public float ContactDamage { get; set; }


    // Variables required by EntityAIInterface
    public bool currentlyAttacking { get; set; }
    public bool ableToAttack { get; set; }
    public int aggroLevel { get; set; }


    public int DetermineAggroLevel(GameObject entityAggroedOn)
    {
        return 0;
    }

    public void MoveUnaggroed()
    {
        //horizontalAccelerationDirection = -1;
    }

    public void MoveAggroed(int aggroLevel, GameObject entityAggroedOn)
    {

    }

    public void Attack(int aggroLevel, GameObject entityAggroedOn)
    {

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        players = GameObject.FindGameObjectsWithTag("Player");

        float distanceSquared = float.PositiveInfinity;
        foreach (GameObject player in players)
        {
            Vector2 PlayerPosition = player.transform.position;
            Vector2 EnemyPosition = transform.position;

            Vector2 DifferenceInPosition = PlayerPosition - EnemyPosition;
            float newDistanceSquared = DifferenceInPosition.x * DifferenceInPosition.x + DifferenceInPosition.y * DifferenceInPosition.y;

            if (newDistanceSquared < distanceSquared)
            {
                distanceSquared = newDistanceSquared;
                targetedPlayer = player;
            }
        }

        playerBehaviour = targetedPlayer.GetComponent<PlayerBehaviour>();
        ContactDamage = ContactDamageValue;

        ContactDamageBehaviour.FixedUpdate(currentCollision, stillInContact, playerBehaviour, gameObject, targetedPlayer, ContactDamage);
        EntityAIBehaviour.ProgressAI(currentlyAttacking, ableToAttack, aggroLevel, targetedPlayer, DetermineAggroLevel, MoveUnaggroed, MoveAggroed, Attack);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tuple<Collider2D, bool> ReferencedInputs = ContactDamageBehaviour.OnTriggerEnter2D(collision, currentCollision, stillInContact, playerBehaviour, gameObject, targetedPlayer, ContactDamage);
        currentCollision = ReferencedInputs.Item1;
        stillInContact = ReferencedInputs.Item2;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stillInContact = ContactDamageBehaviour.OnTriggerExit2D(collision, stillInContact);
    }
}

