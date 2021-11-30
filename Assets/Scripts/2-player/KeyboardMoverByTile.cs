using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile : KeyboardMover
{
    [Tooltip("The tile that will be mined.")] 
    [SerializeField] TileBase prevTile = null;
    [Tooltip("The tile that the mined tile will be turn into.")]
    [SerializeField] TileBase newTile = null;
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;
    [Tooltip("The key used to mine the tile.")]
    [SerializeField] KeyCode keyToMine;

    // Private function to return the tile position in the map.
    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Update()
    {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        // If entered an allowed tile, then change the position.
        if (allowedTiles.Contain(tileOnNewPosition))
        {
            transform.position = newPosition;
        }
        // Added Condition to be able to mine:
        // check if the new tile position does not contain the allowed tiles (cannot be walked into),
        // Also check if the required key is pressed and that the tile we are trying to mine is the correct one (prevTile).
        else if (!allowedTiles.Contain(tileOnNewPosition) && Input.GetKey(keyToMine) && tileOnNewPosition.Equals(prevTile))
        {
            tilemap.SetTile(tilemap.WorldToCell(newPosition), newTile);
        }
        // Trying to enter a tile that is not allowed, does not change position and print error sign.
        else
        {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
    }
}