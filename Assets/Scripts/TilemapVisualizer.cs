using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase wallFull, wallInnerCornerDL, wallInnerCornerDR, wallDiagonalCornerDR,
        wallDiagonalCornerDL, wallDiagonalCornerUR, wallDiagonalCornerUL;
    
    [SerializeField]
    public TileBase[] floorTiles, wallTopTiles, wallSideLeftTiles, wallSideRightTiles, wallBottomTiles;

    [SerializeField]
    private GameObject wallCollidersPrefab;

    private GameObject wallColliders;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTiles[0]);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)], position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        var wallColliders = GameObject.FindGameObjectWithTag("WallColliders");
        if (wallColliders != null)
        {
            wallColliders.tag = "Untagged";
            DestroyImmediate(wallColliders);
        }

        ClearProps();
    }

    public void ClearProps()
    {
        var propsToDestroy = GameObject.FindGameObjectsWithTag("GeneratedForDungeon");
        foreach (var prop in propsToDestroy)
        {
            DestroyImmediate(prop);
        }
    }

    internal void PaintSingleNormalWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if(WallTypesInfo.wallTop.Contains(typeAsInt))
        {
            tile = wallTopTiles[UnityEngine.Random.Range(0, wallTopTiles.Length)];
        }
        else if(WallTypesInfo.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRightTiles[UnityEngine.Random.Range(0, wallSideRightTiles.Length)];
        }
        else if (WallTypesInfo.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeftTiles[UnityEngine.Random.Range(0, wallSideLeftTiles.Length)];
        }
        else if (WallTypesInfo.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottomTiles[UnityEngine.Random.Range(0, wallBottomTiles.Length)];
        }
        else if (WallTypesInfo.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);

            if (wallColliders == null)
                wallColliders = Instantiate(wallCollidersPrefab);

            var collider = wallColliders.AddComponent<BoxCollider2D>();
            collider.offset = position;
        }
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if(WallTypesInfo.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDL;
        }else if (WallTypesInfo.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDR;
        } else if (WallTypesInfo.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDL;
        } else if (WallTypesInfo.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDR;
        } else if (WallTypesInfo.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUR;
        } else if (WallTypesInfo.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUL;
        }
        else if (WallTypesInfo.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesInfo.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottomTiles[UnityEngine.Random.Range(0, wallBottomTiles.Length)];
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }
}
