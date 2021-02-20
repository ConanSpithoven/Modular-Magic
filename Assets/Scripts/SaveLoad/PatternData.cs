[System.Serializable]
public class PatternData
{
    public string[] formula1;
    public string[] formula2;
    public string[] formula3;

    public PatternData(string[] formula1, string[] formula2, string[] formula3)
    {
        this.formula1 = formula1;
        this.formula2 = formula2;
        this.formula3 = formula3;
    }
}
