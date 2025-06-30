[System.Serializable]
public class Achievement
{
    public string id;
    public string name;
    public string description;
    public bool isCompleted;

    public Achievement(string id, string name, string description)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.isCompleted = false;
    }
}
