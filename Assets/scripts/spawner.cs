using UnityEngine;

public class BiomeSpawnerByHeight : MonoBehaviour
{
    public Terrain terrain;
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject sandPrefab;
    private int numObject = 100;

    void Start()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < numObject; i++)
        {
            float Width = terrainData.size.x;
            float Length = terrainData.size.z;

            float x = Random.Range(0, Width);
            float z = Random.Range(0, Length);

            float height = terrain.SampleHeight(
                new Vector3(x, 0, z) + terrain.transform.position) + terrain.transform.position.y;

            GameObject prefab = GetPrefabByHeight(height);
            Vector3 spawnPos = new Vector3(x, height, z) + terrain.transform.position;
            Instantiate(prefab, spawnPos, Quaternion.identity);

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
