using UnityEngine;

public class WebsiteLauncher : MonoBehaviour
{
    public void LaunchWebsite(string url) 
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}
