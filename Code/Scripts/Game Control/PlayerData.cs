[System.Serializable]
public class PlayerData
{
    [System.Serializable]
    public struct SaveObjectInfo
    {
        public uint id;

        public float pos_x;
        public float pos_y;
        public float pos_z;

        public float rot_x;
        public float rot_y;
        public float rot_z;
        public float rot_w;
    }

    public SaveObjectInfo[] saveObjectInfos;

    public float time;
    public bool inCave;
    public bool inShelter;
    public int sceneIndex;

    public PlayerData Copy()
    {
        PlayerData copy = new PlayerData();
        /*copy.keys = keys;
        copy.values = values;*/
        copy.saveObjectInfos = saveObjectInfos;

        copy.time = time;
        copy.inCave = inCave;
        copy.inShelter = inShelter;
        copy.sceneIndex = sceneIndex;

        return copy;
    }
}