using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelRandomizer : MonoBehaviour
{
    public bool generateAtStart = true;
    public Transform itemParent;
    public List<KeyItem> keyItems;
    public List<KeyItem> totems;
    public List<Color> colors;
    // public List<BasicItem> spawnableItems;
    public List<Room> rooms;
    public Transform[] TierOneSpawns;
    public Transform[] TotemSpawns;
    public GameObject player;
    public Transform GoalTreasureItem;

    List<BasicItem> spawnedItems;
    // Start is called before the first frame update
    void Start()
    {
        if (generateAtStart)
        {
            Randomize();
        }
    }

    public void Randomize()
    {
        InGameHUD.instance?.LoadingPanel.SetActive(true);

        // Start Randomization
        List<Room> tempRooms = new List<Room>(rooms);
        List<KeyItem> tempKeys = new List<KeyItem>(keyItems);
        List<Transform> tempTierOneSpawn = new List<Transform>(TierOneSpawns);
        // colors

        // Spawn Room
        Room spawnRoom = tempRooms[Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(spawnRoom);

        player.transform.position = spawnRoom.PlayerPos.position;
        player.transform.rotation = spawnRoom.PlayerPos.rotation;

        spawnRoom.door.gameObject.SetActive(false);

        // Goal Room
        Room goalRoom = tempRooms[Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(goalRoom);

        goalRoom.door.SetMinimapIconColor(Color.red);

        Instantiate(GoalTreasureItem, goalRoom.PlayerPos.position, Quaternion.identity, transform);

        // Room that contains the item for the goal room
        Room firstRoom = tempRooms[Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(firstRoom);

        var item = keyItems[Random.Range(0, keyItems.Count)];
        keyItems.Remove(item);
        goalRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

        Instantiate(item.transform, firstRoom.PlayerPos.position + (Vector3.up * 1f), Quaternion.identity, transform);

        // Second room that contains item
        var secondRoom = tempRooms[Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(secondRoom);

        item = keyItems[Random.Range(0, keyItems.Count)];
        keyItems.Remove(item);

        Instantiate(item.transform, secondRoom.PlayerPos.position + (Vector3.up * 1f), Quaternion.identity, transform);

        var thirdRoom = tempRooms[Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(thirdRoom);

        thirdRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

        // Random items for the first non-goal room
        var spawnPoint = tempTierOneSpawn[Random.Range(0, tempTierOneSpawn.Count)];
        tempTierOneSpawn.Remove(spawnPoint);

        item = keyItems[Random.Range(0, keyItems.Count)];
        keyItems.Remove(item);

        Instantiate(item.transform, spawnPoint.position + (Vector3.up * 1f), Quaternion.identity, transform);
        firstRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

        var curRoom = secondRoom;

        // Other random items
        for (int i = 0; i < 3; i++)
        {
            spawnPoint = tempTierOneSpawn[Random.Range(0, tempTierOneSpawn.Count)];
            tempTierOneSpawn.Remove(spawnPoint);

            item = keyItems[Random.Range(0, keyItems.Count)];
            keyItems.Remove(item);

            curRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

            Instantiate(item.transform, spawnPoint.position + (Vector3.up * 1f), Quaternion.identity, transform);

            if (tempRooms.Count > 0)
            {
                curRoom = tempRooms[Random.Range(0, tempRooms.Count)];
                tempRooms.Remove(curRoom);
            }
        }
        InGameHUD.instance?.LoadingPanel.SetActive(false);
    }

}
