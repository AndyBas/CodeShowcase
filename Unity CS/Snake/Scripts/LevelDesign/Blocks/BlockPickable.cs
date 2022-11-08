using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPickable : Block
{
    [SerializeField] private List<Transform> _pickablesPrefabs = new List<Transform>();
    [SerializeField] private int _nbToInstantiate = 1;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePickableItem();
    }

    private void GeneratePickableItem()
    {
        int lRandIndex = Random.Range(0, _pickablesPrefabs.Count);
        float lRandX = Random.Range(-Block.HALF_WIDTH_HEIGHT, Block.HALF_WIDTH_HEIGHT);
        float lZ = 0f; 

        int lIndex = 0;
        Transform lFood;

        for (lIndex = 0; lIndex < _nbToInstantiate; ++lIndex)
        {
            lZ = transform.position.z + Block.WIDTH_HEIGHT * ((float)lIndex / _nbToInstantiate);

            lFood = Instantiate(_pickablesPrefabs[lRandIndex]);
            lFood.position = new Vector3(lRandX, 0, lZ);
        }
    }
}
