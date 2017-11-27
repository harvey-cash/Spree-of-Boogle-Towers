using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    
    public Vector3 StartPosition() {
        GameObject playerMarker = GetComponentsInChildren<PlayerController>()[0].gameObject;
        Vector3 startPosition = playerMarker.transform.localPosition;
        Destroy(playerMarker);
        return startPosition;
    }

    private LevelGrid levelGrid;
    private Puppet[] puppetList;
    public Puppet[] GetPuppets() { return puppetList; }

    public GameObject floor, directLight;
    public bool BeDark() {
        if (directLight.GetComponent<Light>().intensity <= 0) { return true; }
        else { return false; }
    }

    private GameController gameController;
    public GameController Game {
        set { gameController = value; }
    }

    // Use this for initialization
    void Start () {        
        levelGrid = new LevelGrid(floor, GetChildren());
        puppetList = InitialisePuppets();        
	}

    public Vector3 BoardCenter() {
        return floor.transform.position;
    }


    /* Initialise all Puppets in the level
     */
     private Puppet[] InitialisePuppets() {
        Puppet[] puppets = GetComponentsInChildren<Puppet>();
        for (int i = 0; i < puppets.Length; i++) {
            puppets[i].SetLevel(this);
        }
        return puppets;
    }

    /* Return a GameObject[] of all the child gameObjects
     */
    private GameObject[] GetChildren() {
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < childTransforms.Length; i++) {
            if (childTransforms[i].gameObject.GetComponent<LevelObject>() != null) {
                childTransforms[i].GetComponent<LevelObject>().SetLevelController(this);
                children.Add(childTransforms[i].gameObject);
            }            
        }
        return children.ToArray();
    }

    /* Return true if given co-ords are free
     */
    public ACTION MoveTo(Vector3 position, PlayerController player) {
        int x = (int)Mathf.Floor(position.x);
        int z = (int)Mathf.Floor(position.z);
        return levelGrid.MoveTo(x, z, player);
    }

    /* Press the object at the given position,
     * if it even exists.
     */
    public ACTION Press(Vector3 position, PlayerController player) {
        int x = (int)Mathf.Floor(position.x);
        int z = (int)Mathf.Floor(position.z);

        // If the press results in defeating the level, progress to next
        ACTION pressed = levelGrid.Press(x, z, player);
        if (pressed == ACTION.WIN) {
            gameController.LoadLevel();
        }

        return pressed; 
    }


    /* PUPPETS CAN ONLY DO SO MUCH!
     */
    public ACTION MoveTo(Vector3 position, Puppet puppet) {
        int x = (int)Mathf.Floor(position.x);
        int z = (int)Mathf.Floor(position.z);        
        return levelGrid.MoveTo(x, z, puppet);
    }
    public ACTION Press(Vector3 position, Puppet puppet) {
        int x = (int)Mathf.Floor(position.x);
        int z = (int)Mathf.Floor(position.z);
        ACTION pressed = levelGrid.Press(x, z, puppet);
        return pressed;
    }


    /* Will player / puppet collide with a player / puppet?
     */
    public bool PuppetCollision(Vector3 position) {
        int indexX = (int)Mathf.Floor(position.x);
        int indexZ = (int)Mathf.Floor(position.z);

        for (int i = 0; i < puppetList.Length; i++) {
            int puppetX = (int)Mathf.Floor(puppetList[i].transform.position.x);
            int puppetZ = (int)Mathf.Floor(puppetList[i].transform.position.z);

            // if grid indicies will be the same, we'd collide!
            if ((indexX == puppetX) && (indexZ == puppetZ)) {
                return true;
            }
        }
        return false;
    }
    public bool PlayerCollision(Vector3 position) {
        int ourX = (int)Mathf.Floor(position.x);
        int ourZ = (int)Mathf.Floor(position.z);

        Vector3 playerPos = gameController.GetPlayer().transform.position;
        int playerX = (int)Mathf.Floor(playerPos.x);
        int playerZ = (int)Mathf.Floor(playerPos.z);

        return ((ourX == playerX) && (ourZ == playerZ));
    }
}
