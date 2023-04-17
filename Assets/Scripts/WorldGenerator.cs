using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    public int worldSize;
    int treeAmount;
    int rockAmount;
    GameObject hWorld;
    GameObject hGround;
    GameObject hRock;
    GameObject hTree;
    GameObject car;
    GameObject worldSizeInput;
    GameObject treeAmountInput;
    GameObject rockAmountInput;
    [SerializeField] GameObject terrainTile;
    [SerializeField] GameObject[] treeTile;
    [SerializeField] GameObject[] rockTile;
    [SerializeField] GameObject carTile;
    [SerializeField] GameObject sunTile;

    void Start() 
    {
        worldSizeInput = GameObject.Find("Canvas/World Size/Text");
        treeAmountInput = GameObject.Find("Canvas/Amount of Trees/Text");
        rockAmountInput = GameObject.Find("Canvas/Amount of Rocks/Text");
    }

    public void CreateWorld()
    {
        // Destroy world if it exists
        if(GameObject.Find("World"))
            Destroy(GameObject.Find("World"));

        hWorld = new GameObject("World");
        hGround = new GameObject("Ground");
        hGround.transform.SetParent(hWorld.transform);
        worldSize = int.Parse(worldSizeInput.GetComponent<Text>().text);

        for(int z = 0; z < worldSize; ++z)
        {
            for(int x = 0; x < worldSize; ++x)
            {
                // Generate terrain on x and z multiplicated by the size of the prefab
                var newTerrain = Instantiate(terrainTile, new Vector3(x * 10f + 5f, 0f, z * 10f + 5f), Quaternion.identity);
                newTerrain.transform.SetParent(hGround.transform);
            }
        }

        // Instantiate sun
        var sun = Instantiate(sunTile, new Vector3(worldSize * 10f / 2f, worldSize * 10f / 4f, worldSize * 10f), Quaternion.identity);
        sun.transform.SetParent(hWorld.transform);
        sun.transform.localScale = new Vector3(worldSize / 2f, worldSize / 2f, 1f);

        // Set trees in the hierarchy and in the world
        hTree = new GameObject("Trees");
        hTree.transform.SetParent(hWorld.transform);
        treeAmount = int.Parse(treeAmountInput.GetComponent<Text>().text);
        PlaceObject(treeAmount, treeTile, hTree);

        // Set rocks in the hierarchy and in the world
        hRock = new GameObject("Rocks");
        hRock.transform.SetParent(hWorld.transform);
        rockAmount = int.Parse(rockAmountInput.GetComponent<Text>().text);
        PlaceObject(rockAmount, rockTile, hRock);
    }

    void PlaceObject(int amount, GameObject[] obj, GameObject parent)
    {
        for(int i = 0; i < amount; ++i)
        {
            float x = worldSize * 10f / 2f;
            // Create a road at a 4.5 unit distance from the middle
            while(x > worldSize * 10f / 2f - 4.5f && x < worldSize * 10f / 2f + 4.5f)
                x = Random.Range(0f, worldSize * 10f);

            float z = Random.Range(0f, worldSize * 10f);
            var newObj = Instantiate(obj[Random.Range(0, obj.Length - 1)], new Vector3(x, 0, z), Quaternion.identity);

            // Set object in the hierarchy with different y scale and y rotation
            newObj.transform.SetParent(parent.transform);
            newObj.transform.Rotate(new Vector3(0f, Random.Range(0f, 360f), 0f));
            newObj.transform.localScale = new Vector3(1f,Random.Range(1f, 2f), 1f);
        }
    }

    public void AddCar()
    {
        var car = Instantiate(carTile, new Vector3(worldSize * 10f / 2f, 0f, 2f), Quaternion.identity);
        car.transform.SetParent(hWorld.transform);
    }

    public void StartCar()
    {
        car = GameObject.Find("World/Car(Clone)");
        car.AddComponent<MoveCar>();
    }
    float time = 0f;
    void Update() 
    {
        if(car != null)
        {
            // Destroy the car script when it reach the end of the road
            if(car.transform.position.z >= worldSize * 10f - 2.5f)
                Destroy(car.GetComponent<MoveCar>());    
        }
        if(hTree != null && hTree.transform.childCount > 0)
        {
            // Wind effect for every trees using ping pong
            for(int i = 0; i < hTree.transform.childCount; ++i)
            {
                hTree.transform.GetChild(i).transform.eulerAngles = new Vector3(0f, hTree.transform.GetChild(i).transform.eulerAngles.y, Mathf.PingPong(Time.time, 2) - 1);
            }
        }
    }
}
