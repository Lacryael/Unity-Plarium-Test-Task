using UnityEngine;

namespace Assets.Scripts
{
    public class ButtonCheck : MonoBehaviour
    {
        public bool isClicked; 
        public void TaskOnClick()
        {
            isClicked = true;
        }
    }
}
