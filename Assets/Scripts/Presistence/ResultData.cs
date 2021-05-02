using System;

[Serializable]
public class ResultData
{
    int score;
    float time;
    int waveRank;

    public int Score { get { return score; } }
    public float Time { get { return time; } }
    public int WaveRank { get { return waveRank; } }

    public ResultData(int score, float time, int waveRank)
    {
        this.score = score;
        this.time = time;
        this.waveRank = waveRank;
    }
}