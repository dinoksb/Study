using UnityEngine;

public class UseOnvalidate : MonoBehaviour
{
    private void FixedUpdate()
    {
        public int TestInt = 0;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if(TestInt == 3)
            {
                Debug.LogAssertion("Cannot use this value");
                TestInt = 0;
            }
            else
            {
                Debug.Log("Value Changed");
            }
        }
        #endif
    }
}
