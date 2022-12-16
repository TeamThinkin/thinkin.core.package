using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollGestureZone : MonoBehaviour
{
    public event System.Action OnUserInput;

    [SerializeField] private Renderer visualRenderer;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private float SmoothingRate = 0.1f;
    [SerializeField] private float DecayRate = 0.05f;
    [SerializeField] private float Speed = 1;

    public float DebugRayScalar = 100;

    private Material inactiveMaterial;
    private Collider activeCollider;
    private Transform activeInteractor;
    private Vector3 lastInteractorPosition;
    private Collider zoneCollider;
    private Vector3 localInteractorContactPoint; //Local to the interactor

    public MomentumFloat DirectionDeltaMomentum { get; private set; }
    public Vector3 TrackPosition { get; private set; }
    public Vector3 TrackDelta { get; private set; }
    public float ScrollValue { get; private set; }
    public bool IsTrackActive => activeInteractor != null;

    private void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        inactiveMaterial = visualRenderer.sharedMaterial;
        DirectionDeltaMomentum = new MomentumFloat(DecayRate, SmoothingRate);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (IsTrackActive) return;

        var interactor = other.transform;
        if(interactor != null)
        {
            var contactPoint = zoneCollider.ClosestPoint(other.transform.position);
            
            visualRenderer.sharedMaterial = activeMaterial;
            activeCollider = other;
            activeInteractor = interactor.transform;
            TrackPosition = contactPoint;
            localInteractorContactPoint = activeInteractor.InverseTransformPoint(contactPoint);
            TrackDelta = Vector3.zero;
            DirectionDeltaMomentum.Set(0);
            lastInteractorPosition = TrackPosition;

            OnUserInput?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == activeCollider)
        {
            visualRenderer.sharedMaterial = inactiveMaterial;
            activeCollider = null;
            activeInteractor = null;

            if (Mathf.Abs(DirectionDeltaMomentum.Value) < 0.01f) 
                DirectionDeltaMomentum.Value = 0;
        }
    }

    private void FixedUpdate()
    {
        DirectionDeltaMomentum.SmoothingRate = SmoothingRate;
        DirectionDeltaMomentum.DecayRate = DecayRate;

        DirectionDeltaMomentum.Update();
        if (activeInteractor != null)
        {
            TrackPosition = activeInteractor.TransformPoint(localInteractorContactPoint);
            TrackDelta = TrackPosition - lastInteractorPosition;

            DirectionDeltaMomentum.Set(Vector3.Dot(transform.TransformDirection(direction), TrackDelta) * Speed);

            lastInteractorPosition = TrackPosition;
        }

        ScrollValue = DirectionDeltaMomentum.Value;
    }
}
