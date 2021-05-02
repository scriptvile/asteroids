using System;

[Serializable]
public class ResultData
{
    // Privates
    int score;
    float time;
    int waveRank;

    // Properties
    public int Score { get { return score; } }
    public float Time { get { return time; } }
    public int WaveRank { get { return waveRank; } }


    // Constructor
    public ResultData(int score, float time, int waveRank)
    {
        this.score = score;
        this.time = time;
        this.waveRank = waveRank;
    }
}