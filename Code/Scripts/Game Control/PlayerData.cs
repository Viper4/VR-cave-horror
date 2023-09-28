[System.Serializable]
public class PlayerData
{
    public float pos_x;
    public float pos_y;
    public float pos_z;

    public float rot_x;
    public float rot_y;
    public float rot_z;
    public float rot_w;

    public float time;
    public int sceneIndex;

    public PlayerData Copy()
    {
        PlayerData copy = new PlayerData();
        copy.pos_x = pos_x;
        copy.pos_y = pos_y;
        copy.pos_z = pos_z;

        copy.rot_x = rot_x;
        copy.rot_y = rot_y;
        copy.rot_z = rot_z;
        copy.rot_w = rot_w;

        copy.time = time;
        copy.sceneIndex = sceneIndex;

        return copy;
    }
}