using UnityEngine;

public class BiomeSpawnerByHeight : MonoBehaviour
{
    public Terrain terrain;
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject sandPrefab;
    public int numberOfSpawns = 100;

    void Start()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < numberOfSpawns; i++)
        {
            float terrainWidth = terrainData.size.x;
            float terrainLength = terrainData.size.z;

            float x = Random.Range(0, terrainWidth);
            float z = Random.Range(0, terrainLength);

            Vector3 worldPos = new Vector3(x, 0, z) + terrain.transform.position;
            float height = terrain.SampleHeight(worldPos) + terrain.transform.position.y;

            GameObject prefab = GetPrefabByHeight(height);
            if (prefab != null)
            {
                Vector3 spawnPos = new Vector3(x, height, z) + terrain.transform.position;
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }

    GameObject GetPrefabByHeight(float height)
    {
        float Uni_Height = height / 600.0f;
        if (Uni_Height > 0.01f && Uni_Height < 0.05f)
            return sandPrefab; 
        else if (Uni_Height < 0.2)
            return treePrefab; 
        else
            return rockPrefab; 
    }
}
