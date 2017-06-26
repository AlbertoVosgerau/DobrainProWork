using System.Collections.Generic;
using UnityEngine;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public class ObjectPooler : MonoBehaviour
    {
        private List<GameObject> container;
        public int defaultSize;
        public GameObject prefab;
        public Transform prefabHolder;
        public List<GameObject> Container
        {
            get { return container; }
        }

        void Start()
        {
            InitObjectPooler();
        }

        List<GameObject> InitObjectPooler()
        {
            container = new List<GameObject>();
            expandContainer(defaultSize);
            return container;
        }

        public GameObject GetObject()
        {
            foreach (GameObject item in container)
            {
                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            expandContainer(defaultSize * 2);
            return GetObject();
        }

        // public void RemoveObject(GameObject target)
        // {
        //     GameObject result = container.Find(x => x.GetInstanceID() == target.GetInstanceID());
        //     result.SetActive(false);
        // }

        protected void expandContainer(int size)
        {
            this.defaultSize = size;
            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab) as GameObject;
                obj.transform.SetParent(prefabHolder);
                obj.SetActive(false);
                container.Add(obj);
            }
        }

        // public void ClearPool()
        // {
        //     foreach (GameObject item in container)
        //     {
        //         container.Remove(item);
        //         Destroy(item);
        //     }
        //     container = null;
        // }
    }
}