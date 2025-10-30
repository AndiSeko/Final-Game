
namespace ASUL.Console
{
    [System.Serializable]
    public class Command
    {
        public string primaryCommand;
        public string secondaryCommand;
        public string value;

        public Command(string primaryCommand, string secondaryCommand, string value) 
        {
            this.primaryCommand = primaryCommand;
            this.secondaryCommand = secondaryCommand;
            this.value = value;
        }
    }
}