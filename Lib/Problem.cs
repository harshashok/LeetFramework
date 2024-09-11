namespace LeetFramework;

public class Problem
{
    public string Name { get; private set; } 
    public string Title { get; private set; }
    public string Dir { get; private set; }

    public Problem(string name, string title, string dir)
    {
        this.Name = name;
        this.Title = title;
        this.Dir = dir;
    }
}