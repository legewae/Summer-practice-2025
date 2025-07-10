using Xunit;
using System.IO;
using CommandRunner;
using Microsoft.VisualStudio.TestPlatform.TestHost;
public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute(); // Проверяем, что не возникает исключений  
        Directory.Delete(testDir, true);
        Assert.Equal(10, command.result);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute(); // Должен найти 1 файл  
        Directory.Delete(testDir, true);
        Assert.Single(command.result);
    }

    [Fact]
    public void CommandRunner_TestCommands()
    {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        CommandRunner.Program.Main();

        string[] results = stringWriter.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        Assert.Contains("10", results);
        Assert.Contains(Path.Combine(Path.GetTempPath(), "TestDir2", "test1.txt"), results);
    }
}
