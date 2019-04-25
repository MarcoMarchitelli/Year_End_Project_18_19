using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Evento di unity che porta un float
/// </summary>
[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> { }

/// <summary>
/// Unity Event passing an int
/// </summary>
[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

/// <summary>
/// Evento di unity che porta un Vector2
/// </summary>
[System.Serializable]
public class UnityVector2Event : UnityEvent<Vector2> { }

/// <summary>
/// Evento di unity che porta un Vector3
/// </summary>
[System.Serializable]
public class UnityVector3Event : UnityEvent<Vector3> { }

/// <summary>
/// Evento di unity che porta un bool
/// </summary>
[System.Serializable]
public class UnityBoolEvent : UnityEvent<bool> { }

/// <summary>
/// Unity Event passing a Transform
/// </summary>
[System.Serializable]
public class UnityTransformEvent : UnityEvent<Transform> { }

/// <summary>
/// Unity Event passing a DamageReceiverBehaviour
/// </summary>
[System.Serializable]
public class UnityDamageReceiverEvent : UnityEvent<DamageReceiverBehaviour> { }

/// <summary>
/// Unity Event passing a string
/// </summary>
[System.Serializable]
public class UnityStringEvent : UnityEvent<string> { }

/// <summary>
/// Unity Event passing a Component
/// </summary>
[System.Serializable]
public class UnityComponentEvent : UnityEvent<Component> { }

/// <summary>
/// Unity Event passing a LayerMask
/// </summary>
[System.Serializable]
public class UnityLayerMaskEvent : UnityEvent<LayerMask> { }

/// <summary>
/// Evento di unity void
/// </summary>
[System.Serializable]
public class UnityVoidEvent : UnityEvent { }