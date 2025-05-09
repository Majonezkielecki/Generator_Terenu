using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Terrain terrain;
    public int octaves = 2;
    public float scale = 10f;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public float Multi_Height = 0.8f;
    public Vector2 offset;

    [Header("Rzeka")]
    public float riverShift = 0f;
    public float riverRadians = 0f;
    public int riverWidth = 3;
    public float riverDepth = 0.05f;
    public float riverAmplitude = 50f;
    public float riverFrequency = 0.03f;
    private float[,] heights;
    void Start()
    {
        TerrainData terrain_data = terrain.terrainData;
        int width = terrain_data.heightmapResolution;
        int height = terrain_data.heightmapResolution;
        heights = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heights[y, x] = GenerateFractalNoise(x, y, width, height) * Multi_Height;
            }
        }

        terrain_data.SetHeights(0, 0, heights);

        //rzeka
        GenerateRiver();
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    float GenerateFractalNoise(int x, int y, int width, int height)
    {
        float amp = 1, freq = 1, h1 = 0;
        float x1, y1, dx1, dy1;
        for (int i = 0; i < octaves; i++)
        {
            x1 = (x + offset.x) / width * scale * freq;
            y1 = (y + offset.y) / height * scale * freq;

            dx1 = (x + offset.x) / width * (scale / 5) * freq;
            dy1 = (y + offset.y) / height * (scale / 5) * freq;

            h1 += (Mathf.PerlinNoise(x1, y1) * amp) + 
                  (Mathf.PerlinNoise(dx1, dy1) * 0.3f);

            amp *= persistence;
            freq *= lacunarity;
        }

        float normalized = Mathf.Clamp01(h1 / 1.875f);

        return Mathf.Pow(normalized, 3.7f);
    }

    void GenerateRiver()
    {
        int heightRes = heights.GetLength(0);
        int widthRes = heights.GetLength(1);

        Vector2 Mapcenter = new Vector2(widthRes / 2f, heightRes / 2f) + 
                            new Vector2(-Mathf.Sin(riverRadians), Mathf.Cos(riverRadians)) *
                            riverShift;
        Vector2 dir = new Vector2(Mathf.Cos(riverRadians), Mathf.Sin(riverRadians)).normalized;

        int maxLength = Mathf.CeilToInt(Mathf.Sqrt(widthRes * widthRes + heightRes * heightRes));

        for (int i = -maxLength; i <= maxLength; i++)
        {
            float offset = Mathf.Sin(i * riverFrequency) * riverAmplitude;
            Vector2 pos = Mapcenter + dir * i + new Vector2(-dir.y, dir.x) * offset;

            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);

            for (int dx = -riverWidth; dx <= riverWidth; dx++)
            {
                for (int dy = -riverWidth; dy <= riverWidth; dy++)
                {
                    int nx = Mathf.Clamp(x + dx, 0, widthRes - 1);
                    int ny = Mathf.Clamp(y + dy, 0, heightRes - 1);

                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    if (dist <= riverWidth)
                    {
                        float t = dist / riverWidth;
                        float falloff = Mathf.Pow(1f - t, 2f);

                        heights[nx, ny] -= riverDepth * falloff;
                        heights[nx, ny] = Mathf.Clamp01(heights[nx, ny]);
                    }
                }
            }
        }
    }

}
