public class DirectorySizeCommand : ICommand
    {
    private string _dir = string.Empty;
    public long result { get; private set; }

    public DirectorySizeCommand(string dir)
    {
        _dir = dir;
    }

    public void Execute()
    {
        if (Directory.Exists(_dir))
        {
            string[] files = Directory.GetFiles(_dir);

            result = files.Select(f => new FileInfo(f).Length).Sum();
        }
        else
        {
            throw new Exception("Directory does not exist");
        }
    }
}

public class FindFilesCommand
{
    private string _dir = string.Empty;
    private string _mask = string.Empty;
    public List<string> result { get; private set; }

    public FindFilesCommand(string dir, string mask)
    {
        _dir = dir;
        _mask = mask;
    }

    public void Execute() {
        if (Directory.Exists(_dir))
        {
            result = Directory.GetFiles(_dir,_mask).ToList();
        }
        else
        {
            throw new Exception("Directory does not exist");
        }
    }
}