using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class EnemyAI : NetworkBehaviour
{
    public Transform player; // Player's transform
    public float detectionRange = 10f; // The range within which the enemy detects and chases the player

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Ensure player is found on start
        if (IsServer)
        {
            FindPlayer();
        }
    }

    void Update()
    {
        if (!IsServer) return; // Only run on the server

        // Ensure player is assigned
        if (player == null)
        {
            FindPlayer();
            return; // Exit update until player is found
        }

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // If within detection range, move towards the player
        if (distanceToPlayer <= detectionRange)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.ResetPath(); // Stop moving if out of range
        }

        // Synchronize enemy position to clients
        UpdateClientPositionClientRpc(transform.position);
    }

    [ClientRpc]
    private void UpdateClientPositionClientRpc(Vector3 newPosition)
    {
        if (!IsServer)
        {
            transform.position = newPosition;
        }
    }

    private void FindPlayer()
    {
        // Example of finding the player in a multiplayer context
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            player = players[0].transform; // Pick the first player found
        }
    }
}
