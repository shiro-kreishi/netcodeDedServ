using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnObjectTransform;

    private void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            CreateObjectServerRpc();
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DeleteObjectServerRpc();
        }

        
        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z -= 1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1f;
        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc(RequireOwnership = true)]
    private void CreateObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        spawnObjectTransform = Instantiate(spawnedObjectPrefab);
        spawnObjectTransform.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = true)]
    private void DeleteObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        if (spawnObjectTransform)
        {
            spawnObjectTransform.GetComponent<NetworkObject>().Despawn();
            Destroy(spawnObjectTransform.gameObject);
        }
    }

    
    [ClientRpc]
    private void TestClientRpc()
    {
        
    }
}