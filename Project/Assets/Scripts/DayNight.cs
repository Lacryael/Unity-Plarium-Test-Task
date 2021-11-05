using UnityEngine;

namespace Assets.Scripts
{
    public class DayNight : MonoBehaviour
    {
        public GameObject sun;
        void Start()
        {
            sun.GetComponent<Light>().color = Color.white;
        }
        private void FixedUpdate()
        {
            transform.Rotate(0, 0.1f, 0);
            sun.GetComponent<Light>().color = Color.Lerp(Color.blue, Color.white, Mathf.Abs(Mathf.Sin(Time.time/20)));
        }
    }
}
