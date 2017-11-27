using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour {

    private const int LEVEL_COUNT = 9;
    private int currentLevel = 1;

    public List<GameObject> objectsInScene;
    public Rigidbody cameraObject;

    private PlayerController player;
    public PlayerController GetPlayer() { return player; }

    private LevelController level;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < objectsInScene.Count; i++) {
            Destroy(objectsInScene[i]);
        }

        player = (Instantiate(Resources.Load("Player")) as GameObject).GetComponent<PlayerController>();
        player.Game = this;
        player.CameraObject = cameraObject;
        player.CameraOffset = player.transform.position - cameraObject.transform.position;

        currentLevel--;
        LoadLevel();

        StartCoroutine(PlayMusic());
    }


    private IEnumerator PlayMusic() {
        while(true) {
            AudioClip clip = Resources.Load("Music/OsmosisLoud") as AudioClip;
            AudioSource.PlayClipAtPoint(clip, transform.position, 0.2f);
            yield return new WaitForSeconds(89);
        }        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        //RELOAD
        if (Input.GetKeyDown(KeyCode.Space)) {
            currentLevel--;
            LoadLevel();
        }
    }

    /* Load the next level
     */
    public void LoadLevel() {
        currentLevel++;
        if (currentLevel <= LEVEL_COUNT) {
            string levelName = "MENU";
            Debug.Log("Loading " + levelName);

            if (level != null) { Destroy(level.gameObject); }
            level = (Instantiate(Resources.Load(levelName)) as GameObject).GetComponent<LevelController>();
            level.Game = this;
            player.Level = level;
            player.transform.position = level.StartPosition();
            player.SetTargetPos(level.BoardCenter());

            player.Initialise();
            player.Delluminate();

            //RenderSettings.skybox = Instantiate(Resources.Load("Materials/Backgrounds/" + levelName)) as Material;
            /*
            if (level.BeDark()) {
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                RenderSettings.ambientLight = Color.black;
                player.Illuminate();
            } else {
                RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                DynamicGI.UpdateEnvironment();
                player.Delluminate();
            }
            */
        }
        else {
            Debug.Log("Win!");
            currentLevel = 1;

            RenderSettings.skybox = Instantiate(Resources.Load("Materials/Backgrounds/Level1")) as Material;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            DynamicGI.UpdateEnvironment();
            player.Delluminate();

            Transform[] allObjects = level.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < allObjects.Length; i++) {
                allObjects[i].gameObject.AddComponent<Rigidbody>().useGravity = true;
                Physics.gravity = Vector3.up * 6;
            }
        }
             
    }
}
