using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

namespace CliffLeeCL
{
    /// <summary>
    /// Spawn game objects in spawn areas according to the spawn rate.
    /// </summary>
    public class ObjectSpawner : MonoBehaviour
    {
        /// <summary>
        /// Control whether the spawn process should pause.
        /// </summary>
        public bool isSpawnPaused = false;
        /// <summary>
        /// Game objects that aim to be spawned. 
        /// </summary>
        [Tooltip("Generate a random object if there are more than one prefab.")]
        public GameObject[] objectPrefabs;

        /// <summary>
        /// Define how the spawnAreas is used.
        /// </summary>
        /// <para>SpawnMethod.Random will pick the spawn area randomly in spawnAreas.</para>
        /// <para>SpawnMethod.RoundRobin will pick the spawn area according to the index(cycle).</para>
        /// <seealso cref="spawnMethod"/>
        /// <seealso cref="spawnAreas"/>
        public enum SpawnMethod
        {
            Random,
            RoundRobin
        };
        [Header("Spawn point")]
        /// <summary>
        /// Define how the spawnAreas is used.
        /// </summary>
        /// <seealso cref="SpawnMethod"/>
        public SpawnMethod spawnMethod;
        /// <summary>
        /// Control whether the spawn areas are assigned automatically or by hand.
        /// </summary>
        /// <seealso cref="spawnAreas"/>
        public bool canAutoFindSpawnArea = true;
        /// <summary>
        /// The variable is used to spawn game objects at.
        /// </summary>
        /// <seealso cref="canAutoFindSpawnArea"/>
        public List<SpawnArea> spawnAreas;
        

        /// <summary>
        /// The variable decide every spawn wave's game object number.
        /// </summary>
        /// <para>The minimun is inclusive, and the maximum is exclusive.</para>
        [Header("Spawn number")]
        [Tooltip("Min[inclusive], Max[exclusive]")]
        public IntervalInt spawnNumber = new IntervalInt(3, 8);
        /// <summary>
        /// Decide whether the spawn number is limited to spawn number limit.
        /// </summary>
        /// <seealso cref="spawnNumberLimit"/>
        public bool hasNumberLimit = true;
        /// <summary>
        /// Define how many game objects can the spawner spawn.
        /// </summary>
        /// <seealso cref="hasNumberLimit"/>
        public int spawnNumberLimit = 30;

        /// <summary>
        /// Spawner will start to spawn after first spawn time.
        /// </summary>
        [Header("Spawn rate")]
        public float firstSpawnTime = 5f;
        /// <summary>
        /// After first spawn of spawner, spawner will spawn game objects after spawn rate.
        /// </summary>
        /// <para>The minimun is inclusive, and the maximum is inclusive.</para>
        [Tooltip("Min[inclusive], Max[inclusive]")]
        public IntervalFloat spawnRate = new IntervalFloat(3, 5);

        /// <summary>
        /// The variable is used to group all spawned game objects by this spawner.
        /// </summary>
        Transform objectHolder;
        /// <summary>
        /// Current spawned count.
        /// </summary>
        int spawnCount = 0;

        /// <summary>
        /// Start is called once on the frame when a script is enabled.
        /// </summary>
        void Start()
        {
            if (canAutoFindSpawnArea)
                FindSpawnAreas();
            Assert.IsTrue(spawnAreas.Count > 0, "There is no SpawnAreas!");
            Assert.IsTrue(objectPrefabs.Length > 0, "There is no object to be spawned!");

            spawnCount = 0;
            StopCoroutine("SpawnObject");
            StartCoroutine("SpawnObject");
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled () or inactive.
        /// </summary>
        void OnDisable()
        {
            
        }

        /// <summary>
        /// Event listener that listen to EventManager's onGameOver event.
        /// </summary>
        /// <seealso cref="EventManager.onGameOver"/>
        void OnGameOver()
        {
            StopCoroutine("SpawnObject");
        }

        /// <summary>
        /// Find GameObjects with tag "SpawnArea", and used as spawn areas.
        /// </summary>
        void FindSpawnAreas()
        {
            spawnAreas.Clear();
            SpawnArea[] area = FindObjectsOfType<SpawnArea>();
            for(int i = 0; i < area.Length; i++)
                spawnAreas.Add(area[i]);
            Assert.IsTrue(spawnAreas.Count > 0, "There is no \"SpawnArea\"!");
        }

        /// <summary>
        /// Coroutine that will spawn object after first spawn time, then spawn after spawn rate time.
        /// </summary>
        /// <returns>Interface that all coroutines use.</returns>
        /// <seealso cref="firstSpawnTime"/>
        /// <seealso cref="spawnRate"/>
        IEnumerator SpawnObject()
        {
            if (!objectHolder)
                objectHolder = new GameObject("ObjectHolder").transform;

            yield return new WaitForSeconds(firstSpawnTime);

            while (!isSpawnPaused)
            {
                Spawn();
                yield return new WaitForSeconds(Random.Range(spawnRate.minimun, spawnRate.maximun));
            }
        }

        /// <summary>
        /// Spawn random type random number game objects in random spawn areas.
        /// </summary>
        /// <seealso cref="objectPrefabs"/>
        /// <seealso cref="spawnNumber"/>
        /// <seealso cref="spawnAreas"/>
        void Spawn()
        {
            int spawnNum = Random.Range(spawnNumber.minimun, spawnNumber.maximun);

            for (int i = 0; i < spawnNum; ++i)
            {
                if (hasNumberLimit && spawnCount >= spawnNumberLimit) // Exceed spawn number limit.
                {
                    isSpawnPaused = true;
                    break;
                }
                else
                {
                    GameObject Instance;
                    int typeIndex = Random.Range(0, objectPrefabs.Length);
                    int areaIndex;

                    switch (spawnMethod)
                    {
                        case SpawnMethod.Random:
                            areaIndex = Random.Range(0, spawnAreas.Count);
                            break;
                        case SpawnMethod.RoundRobin:
                            areaIndex = spawnCount % spawnAreas.Count;
                            break;
                        default:
                            areaIndex = 0;
                            break;
                    }

                    Vector3 spawnPoint = spawnAreas[areaIndex].GetSpawnPoint(true);

                    if (spawnPoint != Vector3.zero) {
                        Instance = (GameObject)Instantiate(objectPrefabs[typeIndex], spawnPoint, Quaternion.identity);
                        Instance.transform.SetParent(objectHolder);
                        ++spawnCount;
                    }
                }
            }
        }
    }
}


