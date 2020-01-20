using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Container : MonoBehaviour
{
    public int itemListSize = 0;
    [HideInInspector]
    public GameObject[] itemList;
    [HideInInspector]
    public float[] itemChances;
    private GameObject itemToDrop = null;

    protected virtual void Awake()
    {
        GetItem();
    }

    private void GetItem()
    {
        if (itemList.Length > 0) // exception for empty list
        {
            float summ = 0;
            foreach (float p in itemChances)
            {
                summ += p;
            }
            if (summ > 0) // exception for 0 chances for all items
            {
                float random = Random.Range(0f, summ);
                int i = 0;
                while (random > 0)
                {
                    random -= itemChances[i];
                    i++;
                }
                itemToDrop = itemList[i - 1];
            }
        }
    }

    public void Open()
    {
        if (itemToDrop != null)
            Instantiate(itemToDrop, transform.position, transform.rotation);
        else
            Debug.Log("Error on container open. Empty drop list");
        Destroy(gameObject);
    }


    public static void Table(Container container) // for inspecrot UI
    {
        GUILayout.BeginHorizontal(); // table headline
        GUILayout.Label("Prefab", GUILayout.Width(120));
        GUILayout.Label("Chance", GUILayout.Width(50));
        GUILayout.Label("%", GUILayout.Width(50));
        GUILayout.EndHorizontal();

        float[] lastChances = container.itemChances;
        GameObject[] lastItemList = container.itemList;
        if (lastChances.Length != container.itemListSize)
        { //if array size changed, make new arrays and move data to them            
            container.itemChances = new float[container.itemListSize];
            container.itemList = new GameObject[container.itemListSize];
            for (int i = 0; i < Mathf.Min(lastChances.Length, container.itemChances.Length); i++)
            {
                container.itemChances[i] = lastChances[i];
                container.itemList[i] = lastItemList[i];
            }
            EditorUtility.SetDirty(container); // to prevent load from prefab on Play    
        }

        float summ = 0; // summ for % output
        foreach (float p in container.itemChances)
        {
            summ += p;
        }
        for (int i = 0; i < container.itemListSize; i++)
        {
            GUILayout.BeginHorizontal();
            {
                GameObject lastItem = container.itemList[i];
                container.itemList[i] = (GameObject)EditorGUILayout.ObjectField(container.itemList[i], typeof(GameObject), GUILayout.Width(120));
                if (lastItem != container.itemList[i])
                    EditorUtility.SetDirty(container);

                string chance = container.itemChances[i].ToString();
                chance = EditorGUILayout.TextField("", chance, GUILayout.Width(50));
                if (chance != container.itemChances[i].ToString()) // if changed
                    EditorUtility.SetDirty(container);
                float.TryParse(chance, out container.itemChances[i]);

                EditorGUILayout.LabelField((container.itemChances[i] * 100f / summ).ToString() + "%");
            }
            GUILayout.EndHorizontal();
        }
    }
}
