using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct Pool
    {
        public Queue<ParticleSystem> ParticlePool;
        public ParticleSystem ObjectPrefab;
        public int PoolSize;
    }
    public Pool[] Pools = null;

    //creating gameobjects queues
    private void Awake()
    {

        for (int j = 0; j < Pools.Length; j++)
        {
            Pools[j].ParticlePool = new Queue<ParticleSystem>();
            for (int i = 0; i < Pools[j].PoolSize; i++)
            {
                ParticleSystem obj = Instantiate(Pools[j].ObjectPrefab);
                obj.gameObject.SetActive(false);

                Pools[j].ParticlePool.Enqueue(obj);
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

        ParticleSystem obj = Pools[objectType].ParticlePool.Dequeue();
        obj.gameObject.SetActive(true);
        Pools[objectType].ParticlePool.Enqueue(obj);
        return obj;

    }

}