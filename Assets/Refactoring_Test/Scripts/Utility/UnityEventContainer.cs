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
/// Evento di unity void
/// </summary>
[System.Serializable]
public class UnityVoidEvent : UnityEvent { }