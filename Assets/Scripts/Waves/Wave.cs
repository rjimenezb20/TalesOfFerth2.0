using System.Collections.Generic;


[System.Serializable] 
public class Wave 
{
    public int waveDayStartTime;
    public int warriorCount;
    public int bruteCount;
    public List<EnemySpawner> spawners;

    public Wave(int waveDayStartTime, int warriorCount, int bruteCount)
    {
        this.waveDayStartTime = waveDayStartTime;
        this.warriorCount = warriorCount;
        this.bruteCount = bruteCount;
    }
}