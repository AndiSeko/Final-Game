
namespace ASUL.Console
{
    [System.Serializable]
    public class Command
    {
        public string primaryParameter;
        public string secondaryParameter;
        public string value;

        public Command(string primary)
        {
            primaryParameter = primary;
            secondaryParameter = "";
            value = "";
        }

        public Command(string primary, string secondary)
        {
            primaryParameter = primary;
            secondaryParameter = secondary;
            value = "";
        }

        public Command(string primaryParameter, string secondaryParameter, string value) 
        {
            this.primaryParameter = primaryParameter;
            this.secondaryParameter = secondaryParameter;
            this.value = value;
        }
    }
}