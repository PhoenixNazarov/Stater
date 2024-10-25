namespace Stater.Models;

public interface IProjectManager
{
    Project CreateProject(string name);
    Project LoadProject(string path);
    void SaveProject(Project project, string path);
}
