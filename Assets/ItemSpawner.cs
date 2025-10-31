using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 0.5f;
    public List<Item> items = new List<Item>();
    public Transform parent;
    public Transform[] lanes;
    public Transform spawnPoint;
    public float destroyZ = -50f;

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1, 1f);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);
        Vector3 lanePos = GetRandomLocation();

        spawnedItem.itemPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
        spawnedItem.targetPosition = lanePos;

        items.Add(spawnedItem);
    }

    private Vector3 GetRandomLocation()
    {
        Transform randomLane = lanes[Random.Range(0, lanes.Length)];
        return new Vector3(randomLane.position.x, randomLane.position.y, 0);
    }

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            if (item == null)
            {
                items.RemoveAt(i);
                continue;
            }
            ItemMover(item);

            if (item.itemPosition.z < destroyZ)
            {
                items.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }

        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        children = children.Where(x => x != null && x.GetComponent<Item>() != null).ToList();
        children = children.OrderByDescending(x => x.GetComponent<Item>().itemPosition.z).ToList();
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    private void ItemMover(Item item)
    {
        item.itemPosition.z = item.itemPosition.z - speed;

        float totalDistance = spawnPoint.position.z - destroyZ;
        float traveledDistance = spawnPoint.position.z - item.itemPosition.z;
        float progress = traveledDistance / totalDistance;

        item.itemPosition.x = spawnPoint.position.x + (item.targetPosition.x - spawnPoint.position.x) * progress;
        item.itemPosition.y = spawnPoint.position.y + (item.targetPosition.y - spawnPoint.position.y) * progress;
    }
}