using ASUL.Console;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Command[] Commands;

    private void Start()
    {
        string commands = "sEt vAlUe 1; set sLeG 2; set; set 0 0";

        Commands = CommandReader.ReadCommands(commands);
    }
}
