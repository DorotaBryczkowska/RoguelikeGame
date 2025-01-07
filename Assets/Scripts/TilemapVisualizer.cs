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

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if(WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTopTiles[UnityEngine.Random.Range(0, wallTopTiles.Length)];
        }
        else if(WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRightTiles[UnityEngine.Random.Range(0, wallSideRightTiles.Length)];
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeftTiles[UnityEngine.Random.Range(0, wallSideLeftTiles.Length)];
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottomTiles[UnityEngine.Random.Range(0, wallBottomTiles.Length)];
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
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

        if(WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDL;
        }else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDR;
        } else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDL;
        } else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDR;
        } else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUR;
        } else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUL;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottomTiles[UnityEngine.Random.Range(0, wallBottomTiles.Length)];
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }
}
