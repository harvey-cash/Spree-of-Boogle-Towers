using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Level Grid is responsible for holding a representation
 * of the objects in the current level
 */
public class LevelGrid {
    
    private GameObject[,] grid;
    public GameObject[,] GetGrid() { return grid; }

    /* Construct a new LevelGrid, and populate with the objects in the level
     */
    public LevelGrid(GameObject floor, GameObject[] objects) {
        int x = Mathf.RoundToInt(floor.transform.localScale.x);
        int z = Mathf.RoundToInt(floor.transform.localScale.z);
        grid = new GameObject[x, z];
            
        PopulateGrid(objects);
    }


    /* Populate the grid with the given list of objects
     */
    public void PopulateGrid(GameObject[] objects) {
        for (int i = 0; i < objects.Length; i++) {
            int indexX = (int)Mathf.Floor(objects[i].transform.position.x);
            int indexZ = (int)Mathf.Floor(objects[i].transform.position.z);
            grid[indexX, indexZ] = objects[i];
        }
    }


    /* Return ACTION.MOVE if space is free, otherwise consult the
     * occupying object
     */
    public ACTION MoveTo(int x, int z, PlayerController player) {
        bool withinX = (x >= 0 && x < grid.GetLength(0));
        bool withinZ = (z >= 0 && z < grid.GetLength(1));

        if (withinX && withinZ) {
            if (grid[x, z] == null) {
                return ACTION.MOVE;
            }
            else {
                return grid[x, z].GetComponent<LevelObject>().MoveReaction(player);
            }
            
        } else {
            return ACTION.NOPE;
        }
    }
    /* Overloaded MoveTo for Puppets
     */
    public ACTION MoveTo(int x, int z, Puppet puppet) {
        bool withinX = (x >= 0 && x < grid.GetLength(0));
        bool withinZ = (z >= 0 && z < grid.GetLength(1));

        if (withinX && withinZ) {
            if (grid[x, z] == null) {
                return ACTION.MOVE;
            } else {
                return grid[x, z].GetComponent<LevelObject>().MoveReaction(puppet);
            }

        } else {
            return ACTION.NOPE;
        }
    }


    /* Return the press action of whatever's at these co-ordinates
     */
    public ACTION Press(int x, int z, PlayerController player) {
        if (grid[x, z] == null) {
            return ACTION.MOVE;
        } else {
            return grid[x, z].GetComponent<LevelObject>().PressedReaction(player);
        }
    }
    /* Overloaded Press for Puppets
     */
    public ACTION Press(int x, int z, Puppet puppet) {
        if (grid[x, z] == null) {
            return ACTION.MOVE;
        } else {
            return grid[x, z].GetComponent<LevelObject>().PressedReaction(puppet);
        }
    }


    /* Add the given GameObject to the grid, provided the grid
     * space is free.
     */
    public bool AddObjectToGrid(GameObject objectToAdd) {
        int indexX = (int)Mathf.Floor(objectToAdd.transform.position.x);
        int indexZ = (int)Mathf.Floor(objectToAdd.transform.position.z);

        if (grid[indexX, indexZ] != null) {
            // That grid space is already populated
            return false;
        } else {
            grid[indexX, indexZ] = objectToAdd;
            return true;
        }
    }
	
}
