using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<ParticleSystem> pool;
        public ParticleSystem ObjectPrefab;
        public int PoolSize;
    }
    public Pool[] Pools = null;

    //creating gameobjects queues
    private void Awake()
    {

        for (int j = 0; j < Pools.Length; j++)
        {
            Pools[j].pool = new Queue<ParticleSystem>();
            for (int i = 0; i < Pools[j].PoolSize; i++)
            {
                ParticleSystem obj = Instantiate(Pools[j].ObjectPrefab);
                obj.gameObject.SetActive(false);

                Pools[j].pool.Enqueue(obj);
            }
        }
        
    }
    //returning a gameobject from queue
    public ParticleSystem GetObject(int objectType)
    {
        if (objectType >= Pools.Length)
        {
            return null;
        }

        ParticleSystem obj = Pools[objectType].pool.Dequeue();
        obj.gameObject.SetActive(true);
        Pools[objectType].pool.Enqueue(obj);
        return obj;

    }

}