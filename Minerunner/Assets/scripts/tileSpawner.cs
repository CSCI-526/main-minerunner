using UnityEngine;
using System.Collections.Generic;

public class FloatingTileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public bool enableBobbing = true;
    public float amplitude = 0.25f;
    public float frequency = 1f;

    public Vector3[] tilePositions = new Vector3[]
    {
        new Vector3(0f, 0f, 0f),
        new Vector3(2f, 0.6f, 1.2f),
        new Vector3(-2.2f, 0.4f, 2.5f),
        new Vector3(3.5f, 1.2f, -1f),
        new Vector3(-3.5f, 0.8f, -2f),
        new Vector3(1.2f, 0.3f, -3.5f),
        new Vector3(3.2f, 1.4f, 2.5f),
        new Vector3(-4f, 0.5f, 5.5f),
        new Vector3(2.2f, 1.1f, 4.7f),
        new Vector3(-2.8f, 0.9f, -3f),
        new Vector3(4.2f, 1.3f, 1f),
        new Vector3(-1.5f, 1.1f, 3.1f),
        new Vector3(3.2f, 0.5f, -4.1f),
        new Vector3(-4.0f, 1.3f, 4.8f),
        new Vector3(1.6f, 1.7f, -1.4f),
        new Vector3(-2.7f, 0.2f, 3.9f),
        new Vector3(4.4f, 0.8f, -4.8f),
        new Vector3(-3.3f, 1.5f, -4.4f),
        new Vector3(2.8f, 0.9f, 4.3f),
        new Vector3(1.2f, 1.2f, -5.1f),
        new Vector3(-2.1f, 1.4f, -1.3f),
        new Vector3(3.7f, 0.6f, 1.9f),
        new Vector3(-4.5f, 0.7f, 2.2f),
        new Vector3(1.4f, 1.6f, 3.1f),
        new Vector3(2.3f, 0.3f, -3.8f),
        new Vector3(-3.6f, 1.0f, 1.1f),
    };

    class FloatingBobData
    {
        public Transform transform;
        public Vector3 startPos;
        public float offset;

        public FloatingBobData(Transform t)
        {
            transform = t;
            startPos = t.position;
            offset = Random.Range(0f, 2 * Mathf.PI);
        }
    }

    List<FloatingBobData> bobbingTiles = new();

    void Start()
    {
        foreach (Vector3 pos in tilePositions)
        {
            GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);

            if (enableBobbing)
                bobbingTiles.Add(new FloatingBobData(tile.transform));
        }
    }

    void Update()
    {
        if (!enableBobbing) return;

        float time = Time.time;
        foreach (FloatingBobData bob in bobbingTiles)
        {
            float y = Mathf.Sin(time * frequency + bob.offset) * amplitude;
            bob.transform.position = bob.startPos + new Vector3(0f, y, 0f);
        }
    }
}
