using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] tiles;
    public List<GameObject> map;
    //public Dictionary<Vector2, GameObject> map;


    int tilesetSizes = 4;
    void Start()
    {
        //map = new Dictionary<Vector2, GameObject>();
        map = new List<GameObject>();
        GenerateMap(10, 1, 4);
        //tilesets
        
    }
    /*
     * size refers to the size of the map.
     * hills refers to how likely curY will jump a unit, the lower, the more likely. A random number will be assigned between 0 and hills.
     * If the number is hills, it will jump up 1, if it is 0, it will jump down 1. Set to a negative number if you want a flat map.
     */
    public void GenerateMap(int size, int tileset, int hills)
    {
        int curX = 0;
        int curY = 0;
        int curZ = 0;
        for (curX = 0; curX < size; curX++)
        {
            for (curZ = 0; curZ < size; curZ++)
            {
                if (curX != 0 && curZ != 0)
                {
                    //curY = (int)((map[new Vector2(curX - 1, curZ)].transform.position.y + map[new Vector2(curX, curZ - 1)].transform.position.y) / 2);
                    curY = (int)(map[map.Count - size].transform.position.y + map[map.Count - 1].transform.position.y) / 2;
                }
                else if (curX != 0)
                {
                    //curY = (int)(map[new Vector2(curX - 1, curZ)].transform.position.y);
                    curY = (int)map[map.Count - size].transform.position.y;
                }
                else if (curZ != 0)
                {
                    //curY = (int)(map[new Vector2(curX, curZ - 1)].transform.position.y);
                    curY = (int)map[map.Count - 1].transform.position.y;
                }
                if (hills >= 0)
                {
                    bool randBool = false;
                    int rand = Random.Range(0, hills + 1);
                    while (!randBool)
                    {
                        if (rand == hills)
                        {
                            curY += 1;
                            randBool = true;
                        }
                        else if (rand == 0 && curY > 0)
                        {
                            curY -= 1;
                            randBool = true;
                        }
                        else if (rand != 0)
                        {
                            randBool = true;
                        }
                        else
                        {
                            rand = Random.Range(0, hills + 1);
                        }
                    }
                }
                int curTileRand = Random.Range(0 + (tileset * tilesetSizes), tilesetSizes + (tileset * tilesetSizes));
                //map[new Vector2(curX, curZ)] = Instantiate(tiles[curTileRand], new Vector3(curX, curY, curZ), transform.rotation);
                map.Add(Instantiate(tiles[curTileRand], new Vector3(curX, curY, curZ), transform.rotation));
                for (int i = 0; i < curY; i++)
                {
                    Instantiate(tiles[curTileRand], new Vector3(curX, i, curZ), transform.rotation);
                }

                map[map.Count - 1].AddComponent<BoxCollider>();
                map[map.Count - 1].GetComponent<BoxCollider>().center = new Vector3((float)-.5, (float).55, (float).5);
                map[map.Count - 1].GetComponent<BoxCollider>().size = new Vector3(1, (float)1.1, 1);
                //map[map.Count - 1].GetComponent<BoxCollider>().isTrigger = true;
                map[map.Count - 1].tag = "GroundObject";


                //GameObject currentGameObject = map[new Vector2(curX, curZ)];
                //currentGameObject.AddComponent<BoxCollider>();
                //currentGameObject.GetComponent<BoxCollider>().center = new Vector3((float)-.5, (float).55, (float).5);
                //currentGameObject.GetComponent<BoxCollider>().size = new Vector3(1, (float)1.1, 1);
                //currentGameObject.tag = "GroundObject";


                //map[map.Count - 1].AddComponent<Rigidbody>();
                //map[map.Count - 1].GetComponent<Rigidbody>().isKinematic = true;
                //map[map.Count - 1] = Instantiate(tiles[curTileRand], new Vector3(curX, curY, curZ), transform.rotation);
            }
        }
    }

    /*
     * to be implemented later to use maps that we want to look a certain way... such as a castle map
     */
    public void MapReader(string FilePath)
    {

    }
    //doesn't currently work correctly
    public void DestroyMap()
    {
        //for(int i = map.Count - 1; i >= 0; i--)
        //{
        //    GameObject g = map[i];
        //    Destroy(g);
        //}
    }
}