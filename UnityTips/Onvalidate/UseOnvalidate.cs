using UnityEngine;

public class UseOnvalidate : MonoBehaviour
{
    public int TestInt = 0;

    private void FixedUpdate()
    {
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
