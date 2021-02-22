using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public ImageSynthesis synth;
    public GameObject[] prefabs;
    public int minObjects = 10;
    public int maxObjects = 50;
    public int trainingImages;
    public int valImages;
    public bool grayscale = false;
    public bool save = false;

    private ShapePool pool;
    private int frameCount = 0;

    // Start is called before the first frame update
    void Start() {
        pool = ShapePool.Create(prefabs);
    }

    // Update is called once per frame
    void Update() {
        if (frameCount < trainingImages + valImages) {
            if (frameCount % 30 == 0) {
                GenerateRandom();
                Debug.Log($"FrameCount: {frameCount}");
            }
            frameCount++;
            if (save) {
                if (frameCount < trainingImages) {
                    string filename = $"image_{frameCount.ToString().PadLeft(5, '0')}";
                    synth.Save(filename, 512, 512, "captures/train", 2);
                }
                else if (frameCount < trainingImages + valImages) {
                    int valFrameCount = frameCount - trainingImages;
                    string filename = $"image_{valFrameCount.ToString().PadLeft(5, '0')}";
                    synth.Save(filename, 512, 512, "captures/val", 2);
                }

            }
        }
    }

    void GenerateRandom() {
        pool.ReclaimAll();
        int objectsThisTime = Random.Range(minObjects, maxObjects);
        for (int i = 0; i < objectsThisTime; i++) {
            // Pick out a prefab
            int prefabIndx = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[prefabIndx];

            // Position
            float newPosX, newPosY, newPosZ;
            newPosX = Random.Range(-15.0f, 15.0f);
            newPosY = Random.Range(-0.5f, 0.5f);
            newPosZ = Random.Range(-15.0f, 15.0f);
            Vector3 newPos = new Vector3(newPosX, newPosY, newPosZ);

            // Rotation
            float newRotX, newRotY, newRotZ;
            newRotX = Random.Range(-20.0f, 20.0f);
            newRotY = Random.Range(-180.0f, 180.0f);
            newRotZ = Random.Range(-20.0f, 20.0f);

            var newRot = Quaternion.Euler(new Vector3(newRotX, newRotY, newRotZ));

            var shape = pool.Get((ShapeLabel)prefabIndx);
            var newObj = shape.obj;
            newObj.transform.position = newPos;
            newObj.transform.rotation = newRot;

            // Scale
            float sx = Random.Range(4.0f, 8.0f);
            Vector3 newScale = new Vector3(sx, sx, sx);
            newObj.transform.localScale = newScale;
            
           // Color
            float newR, newG, newB;
            newR = Random.Range(0.0f, 1.0f);
            newG = Random.Range(0.0f, 1.0f);
            newB = Random.Range(0.0f, 1.0f);
            var newColor = new Color(newR, newG, newB);

            Debug.Log($"newObj: {newObj}");
            // if (newObj.label != "Remy") {
            //newObj.GetComponent<Renderer>().material.color = newColor;
            //}          

        }
        synth.OnSceneChange(grayscale);
    }
}
