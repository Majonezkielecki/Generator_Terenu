using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain;
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject sandPrefab;

    [Header("properties")]
    public int GeneralObjects = 100;
    [Header("Forest")]
    public float forestRadius = 100f;
    public int forestObjects = 100;
    public Vector2 forestPoz;


    private TerrainData terrainData;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        terrainData = terrain.terrainData;

        float Width = terrainData.size.x;
        float Length = terrainData.size.z;
        Vector2 Poz;
        for (int i = 0; i < GeneralObjects; i++){
            Poz = new(Random.Range(0, Width), Random.Range(0, Length));

            GenerateObject(Poz, false);
        }
        //Forest Geneator
        Vector2 forestCenter = new(Width * forestPoz.x, Length * forestPoz.y);

        for (int i = 0; i < forestObjects; i++)
        {
            Vector2 offset = Random.insideUnitCircle * forestRadius;
            Poz = new(Mathf.Clamp(forestCenter.x + offset.x, 0, Width), 
                      Mathf.Clamp(forestCenter.y + offset.y, 0, Length));

            GenerateObject(Poz, true);
        }
    }

    int GenerateObject(Vector2 poz,bool forest)
    {
        float height = terrain.SampleHeight(new Vector3(poz.x, 0, poz.y) 
            + terrain.transform.position) + terrain.transform.position.y;

        GameObject prefab = GetPrefabByHeight(height,forest);
        if (prefab == null) return 0;

        Vector3 spawnPos = new Vector3(poz.x, height, poz.y) + terrain.transform.position;
        Instantiate(prefab, spawnPos, Quaternion.identity, this.transform);

        return 1;
    }

    GameObject GetPrefabByHeight(float height, bool forest)
    {
        float Uni_Height = height / 600.0f;
        if ( Uni_Height <= 0.05f && Uni_Height > 0.01f && !forest)
            return sandPrefab; 
        else if (Uni_Height <= 0.2 && Uni_Height > 0.05f)
            return treePrefab; 
        else if (Uni_Height <= 0.55 && Uni_Height > 0.2 && !forest)
            return rockPrefab; 

        return null;
    }
}
