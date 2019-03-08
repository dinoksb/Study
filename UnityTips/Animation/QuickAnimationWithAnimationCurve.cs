using UnityEngine;
 
public class QuickAnimationWithAnimationCurve : MonoBehaviour {
 
    public enum EAnimatedProperty {
        LocalPositionX,
        LocalPositionY,
        LocalPositionZ,
        LocalEulerX,
        LocalEulerY,
        LocalEulerZ,
        ScaleX,
        ScaleY,
        ScaleZ,
        ScaleUniform
    }
    public EAnimatedProperty Affect;
    public AnimationCurve Curve;
    public float Speed;
 
    private float m_fTime;
    private Vector3
        m_vTempPosition,
        m_vTempEuler,
        m_vTempScale;
    private Transform m_ThisTransform;
 
    void Start () {
        m_ThisTransform = transform;
    }
 
    void Update () {
        m_fTime += Time.deltaTime * Speed;
        m_vTempPosition = m_ThisTransform.localPosition;
        m_vTempEuler = m_ThisTransform.localEulerAngles;
        m_vTempScale = m_ThisTransform.localScale;
        switch (Affect) {
        case EAnimatedProperty.LocalPositionX:
            m_vTempPosition.x = Curve.Evaluate(m_fTime);
            m_ThisTransform.localPosition = m_vTempPosition;
            break;
        case EAnimatedProperty.LocalPositionY:
            m_vTempPosition.y = Curve.Evaluate(m_fTime);
            m_ThisTransform.localPosition = m_vTempPosition;
            break;
        case EAnimatedProperty.LocalPositionZ:
            m_vTempPosition.z = Curve.Evaluate(m_fTime);
            m_ThisTransform.localPosition = m_vTempPosition;
            break;
        case EAnimatedProperty.LocalEulerX:
            m_vTempEuler.x = Curve.Evaluate(m_fTime);
            m_ThisTransform.localEulerAngles = m_vTempEuler;
            break;
        case EAnimatedProperty.LocalEulerY:
            m_vTempEuler.y = Curve.Evaluate(m_fTime);
            m_ThisTransform.localEulerAngles = m_vTempEuler;
            break;
        case EAnimatedProperty.LocalEulerZ:
            m_vTempEuler.z = Curve.Evaluate(m_fTime);
            m_ThisTransform.localEulerAngles = m_vTempEuler;
            break;
        case EAnimatedProperty.ScaleX:
            m_vTempScale.x = Curve.Evaluate(m_fTime);
            m_ThisTransform.localScale = m_vTempScale;
            break;
        case EAnimatedProperty.ScaleY:
            m_vTempScale.y = Curve.Evaluate(m_fTime);
            m_ThisTransform.localScale = m_vTempScale;
            break;
        case EAnimatedProperty.ScaleZ:
            m_vTempScale.z = Curve.Evaluate(m_fTime);
            m_ThisTransform.localScale = m_vTempScale;
            break;
        case EAnimatedProperty.ScaleUniform:
            m_ThisTransform.localScale = Vector3.one * Curve.Evaluate(m_fTime);
            break;
        }
    }
}