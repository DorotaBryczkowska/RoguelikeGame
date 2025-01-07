using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonRoomsGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    
    [SerializeField]
    private List<GameObject> roomPropsPrefabs;
    
    [SerializeField]
    private int maxPropsPerRoom = 3;
    
    [SerializeField]
    GameObject roomPropsParent;
    
    [SerializeField]
    GameObject enemySpawnerParent;
    
    [SerializeField]
    private List<GameObject> enemyPrefabs;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new();
        floor = CreateRooms(roomsList);

        List<Vector2Int> roomCenters = new();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        var spawnPoint = roomCenters[0];
        var lastRoom = roomCenters.Count-1;
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
        GameObject.FindGameObjectWithTag("Spawner").transform.position = new Vector3(roomCenters[lastRoom].x, roomCenters[lastRoom].y, 0);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(spawnPoint.x, spawnPoint.y, -1);

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        PlaceRoomProps(roomsList, floor, corridors);
        PlaceSpawners(roomsList);
    }

    private void PlaceSpawners(List<BoundsInt> roomsList)
    {
        var lastRoom = roomsList.Count - 1;
        foreach (var room in roomsList)
        {
            if(roomsList.IndexOf(room) == 0 || roomsList.IndexOf(room) == lastRoom)
            {
                continue;
            }
            else
            {
                Vector2 roomCenter = new(Mathf.RoundToInt(room.center.x), Mathf.RoundToInt(room.center.y));
                GameObject spawner = new("EnemySpawner");
                spawner.transform.position = roomCenter;
                EnemySpawner enemySpawner = spawner.AddComponent<EnemySpawner>();
                enemySpawner.transform.parent = enemySpawnerParent.transform;
                enemySpawner.tag = "GeneratedForDungeon";
                enemySpawner.enemyPrefabs = enemyPrefabs;
            }
        }
    }

    private HashSet<Vector2Int> CreateRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && 
                    position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count>0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new();
        var position = currentRoomCenter;
        corridor.Add(position); 
        while (position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position+= Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if(destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private void PlaceRoomProps(List<BoundsInt> roomsList, HashSet<Vector2Int> floor, HashSet<Vector2Int> corridors)
    {
        HashSet<Vector2Int> usedPositions = new();

        foreach (var room in roomsList)
        {
            int propsToPlace = Random.Range(1, maxPropsPerRoom + 1);
            for (int i = 0; i < propsToPlace; i++)
            {
                Vector2Int randomPosition = GetRandomPositionInRoom(room, floor, corridors, usedPositions);
                if (randomPosition != Vector2Int.zero)
                {
                    GameObject propPrefab = roomPropsPrefabs[Random.Range(0, roomPropsPrefabs.Count)];
                    var spawnedProp = Instantiate(propPrefab, new Vector3(randomPosition.x, randomPosition.y, 0), Quaternion.identity);
                    spawnedProp.transform.parent=roomPropsParent.transform;
                    spawnedProp.tag = "GeneratedForDungeon";
                    usedPositions.Add(randomPosition); // Oznacz tę pozycję jako zajętą
                }
            }
        }
    }

    private Vector2Int GetRandomPositionInRoom(BoundsInt room, HashSet<Vector2Int> floor, HashSet<Vector2Int> corridors, HashSet<Vector2Int> usedPositions)
    {
        List<Vector2Int> validPositions = new();
        for (int x = room.xMin + offset; x < room.xMax - offset; x++)
        {
            for (int y = room.yMin + offset; y < room.yMax - offset; y++)
            {
                Vector2Int position = new(x, y);
                if (floor.Contains(position) && !corridors.Contains(position) && !usedPositions.Contains(position) && !IsNearWall(position, floor))
                {
                    validPositions.Add(position);
                }
            }
        }
        if (validPositions.Count > 0)
        {
            return validPositions[Random.Range(0, validPositions.Count)];
        }
        return Vector2Int.zero; // Brak dostępnych pozycji
    }


    private bool IsNearWall(Vector2Int position, HashSet<Vector2Int> floor)
    {
        // Współrzędne sąsiadów (8 kierunków)
        Vector2Int[] neighbors = {
        position + Vector2Int.up,
        position + Vector2Int.down,
        position + Vector2Int.left,
        position + Vector2Int.right,
        position + new Vector2Int(1, 1),
        position + new Vector2Int(-1, 1),
        position + new Vector2Int(1, -1),
        position + new Vector2Int(-1, -1)
    };

        foreach (var neighbor in neighbors)
        {
            if (!floor.Contains(neighbor)) // Jeśli sąsiad nie należy do podłogi, to jest to ściana
            {
                return true;
            }
        }
        return false; // Wszystkie sąsiady należą do podłogi
    }
}
