using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomStepDungeonGenerator : AbstractDungeonGenerator
{

    [SerializeField]
    protected RandomStepData randomStepParameters;

    protected override void StartProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomStep(randomStepParameters, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomStep(RandomStepData parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new();
        for(int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.RandomStep(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path); 
            if(parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
