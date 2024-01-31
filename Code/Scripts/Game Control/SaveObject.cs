using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    public uint saveID;

    [SerializeField] bool savePosisition = true;
    [SerializeField] bool saveRotation = true;
    
    public PlayerData.SaveObjectInfo GetInfo()
    {
        return new PlayerData.SaveObjectInfo
        {
            id = saveID,

            pos_x = transform.position.x,
            pos_y = transform.position.y,
            pos_z = transform.position.z,

            rot_x = transform.rotation.x,
            rot_y = transform.rotation.y,
            rot_z = transform.rotation.z,
            rot_w = transform.rotation.w,
        };
    }

    public void LoadSave(PlayerData.SaveObjectInfo saveObjectInfo)
    {
        Vector3 position = savePosisition ? new Vector3(saveObjectInfo.pos_x, saveObjectInfo.pos_y, saveObjectInfo.pos_z) : transform.position;
        Quaternion rotation = saveRotation ? new Quaternion(saveObjectInfo.rot_x, saveObjectInfo.rot_y, saveObjectInfo.rot_z, saveObjectInfo.rot_w) : transform.rotation;
        transform.SetPositionAndRotation(position, rotation);
    }
}
