using System;
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
            Randomizr();
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
        Room spawnRoom = tempRooms[UnityEngine.Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(spawnRoom);

        player.transform.position = spawnRoom.PlayerPos.position;
        player.transform.rotation = spawnRoom.PlayerPos.rotation;

        spawnRoom.door.gameObject.SetActive(false);

        // Goal Room
        Room goalRoom = tempRooms[UnityEngine.Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(goalRoom);

        goalRoom.door.SetMinimapIconColor(Color.red);

        Instantiate(GoalTreasureItem, goalRoom.PlayerPos.position, Quaternion.identity, transform);

        // Room that contains the item for the goal room
        Room firstRoom = tempRooms[UnityEngine.Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(firstRoom);

        var item = keyItems[UnityEngine.Random.Range(0, keyItems.Count)];
        keyItems.Remove(item);
        goalRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

        Instantiate(item.transform, firstRoom.PlayerPos.position + (Vector3.up * 1f), Quaternion.identity, transform).GetComponent<KeyItem>();
        // item.light




        //
        // Second room that contains item
        var secondRoom = tempRooms[UnityEngine.Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(secondRoom);

        item = keyItems[UnityEngine.Random.Range(0, keyItems.Count)];
        keyItems.Remove(item);

        Instantiate(item.transform, secondRoom.PlayerPos.position + (Vector3.up * 1f), Quaternion.identity, transform);

        var thirdRoom = tempRooms[UnityEngine.Random.Range(0, tempRooms.Count)];
        tempRooms.Remove(thirdRoom);

        thirdRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);




        // Random items for the first non-goal room
        var spawnPoint = tempTierOneSpawn[UnityEngine.Random.Range(0, tempTierOneSpawn.Count)];
        tempTierOneSpawn.Remove(spawnPoint);

        item = keyItems[UnityEngine.Random.Range(0, keyItems.Count)];
        keyItems.Remove(item);

        Instantiate(item.transform, spawnPoint.position + (Vector3.up * 1f), Quaternion.identity, transform);
        firstRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

        var curRoom = secondRoom;

        // Other random items
        for (int i = 0; i < 3; i++)
        {
            spawnPoint = tempTierOneSpawn[UnityEngine.Random.Range(0, tempTierOneSpawn.Count)];
            tempTierOneSpawn.Remove(spawnPoint);

            item = keyItems[UnityEngine.Random.Range(0, keyItems.Count)];
            keyItems.Remove(item);

            curRoom.door.SetKeyItem(item.Kind, item.Icon, Color.white);

            Instantiate(item.transform, spawnPoint.position + (Vector3.up * 1f), Quaternion.identity, transform);

            if (tempRooms.Count > 0)
            {
                curRoom = tempRooms[UnityEngine.Random.Range(0, tempRooms.Count)];
                tempRooms.Remove(curRoom);
            }
        }
        InGameHUD.instance?.LoadingPanel.SetActive(false);
    }

    public void Randomizr()
    {
        InGameHUD.instance?.LoadingPanel.SetActive(true);

        //Randomize to all heck
        List<Room> tempRooms = new List<Room>(rooms).OrderBy(i => Guid.NewGuid().ToString()).ToList();
        List<KeyItem> tempKeys = new List<KeyItem>(keyItems).OrderBy(i => Guid.NewGuid().ToString()).ToList();
        List<Transform> tempTierOneSpawn = new List<Transform>(TierOneSpawns).OrderBy(i => Guid.NewGuid().ToString()).ToList();
        List<Color> tempColors = new List<Color>(colors).OrderBy(i => Guid.NewGuid().ToString()).ToList();
        Room currRoom = null;
        for (int r = 0; r < tempRooms.Count; r++)
        {
            var room = tempRooms[r];
            print("Room " + r + " at " + room.transform.position);
            if (r == 0)
            {
                player.transform.position = room.PlayerPos.position;
                player.transform.rotation = room.PlayerPos.rotation;
                room.door?.gameObject.SetActive(false);
            }
            else
            {
                for (int k = 1; k < tempKeys.Count + 1; k++)
                {
                    // print("Key/Color " + k);
                    int index = r > 3 ? r-- : r;
                    var key = tempKeys[k--];
                    // var color = tempColors[k];
                    var color = Color.white;
                    var ikey = Instantiate(key, Vector3.zero, Quaternion.identity, transform).GetComponent<KeyItem>();
                    if (r > 0 || r < 4)
                    {
                        if (r == 1) room.door.SetMinimapIconColor(Color.red); // This be the room
                        ikey.transform.position = room.PlayerPos.position + (Vector3.up * 1f);
                        if (r == 2) { currRoom = room; }
                    }
                    else
                    {
                        currRoom = room;
                        // ikey.transform.position = tempRooms[r--].PlayerPos.position + (Vector3.up * 1f);
                    }
                    ikey.renderer.color = color;
                    // tempRooms[index].door.SetKeyItem(tempKeys[k--].Kind, tempKeys[k--].Icon, color);
                    // tempRooms[index].door.glow.color = color;
                    // tempRooms[index].door.keyIcon.color = color;
                }
            }
        }
        InGameHUD.instance?.LoadingPanel.SetActive(false);
    }
}
