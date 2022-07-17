using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EncounterArea : MonoBehaviour
{
    [SerializeField] private Encounter encounter;
    [SerializeField] private Transform playerBattlePosition;
    [SerializeField] private Transform[] enemyBattlePositions;
    [SerializeField] private Vector3 enemySpawnOffset;

    private bool hasTriggered = false;

    private void TriggerBattle(Player player)
    {
        SetupPlayer(player);

        var enemies = SpawnAndPositionEnemies();
        StartCoroutine(BattleSystem.instance.StartBattle(enemies));

        hasTriggered = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasTriggered) { return; }

        var player = collision.GetComponent<Player>();
        TriggerBattle(player);
    }

    private void SetupPlayer(Player player)
    {
        player.DisableCollisionAndMovement();
        StartCoroutine(LerpMovement.LerpToPosition(player.transform, playerBattlePosition.position, 2f));
    }

    private Enemy[] SpawnAndPositionEnemies()
    {
        Enemy[] enemies = new Enemy[encounter.enemies.Length];

        for (int i = 0; i < encounter.enemies.Length; i++)
        {
            Vector3 offsetedPos = enemyBattlePositions[i].position + enemySpawnOffset;

            var enemy = Instantiate(encounter.enemies[i], offsetedPos, Quaternion.identity);
            enemies[i] = enemy;

            StartCoroutine(LerpMovement.LerpToPosition(enemy.transform, enemyBattlePositions[i].position, 2));
        }

        return enemies;
    }
}
