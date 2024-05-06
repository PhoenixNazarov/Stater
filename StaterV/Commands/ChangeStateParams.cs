namespace StaterV.Commands
{
    class ChangeStateParams : CommandParams
    {
        public Attributes.StateAttributes NewAttributes { get; set; }
        public Project.ProjectManager Project { get; set; }
    }
}
