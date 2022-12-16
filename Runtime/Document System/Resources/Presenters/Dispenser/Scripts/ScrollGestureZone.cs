using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollGestureZone : MonoBehaviour, IHandlePointerEvent
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
    private bool isPancake;
    private IUIPointer activePointer;
    private Plane trackingPlane;


    public MomentumFloat DirectionDeltaMomentum { get; private set; }
    public Vector3 TrackPosition { get; private set; }
    public Vector3 TrackDelta { get; private set; }
    public float ScrollValue { get; private set; }
    public bool IsTrackActive { get; private set; }

    private void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        inactiveMaterial = visualRenderer.sharedMaterial;
        DirectionDeltaMomentum = new MomentumFloat(DecayRate, SmoothingRate);

        initializePancakeMode();
    }

    private void initializePancakeMode()
    {
        isPancake = AppControllerBase.Instance.IsPancake;
        if (!isPancake) return;

        zoneCollider.transform.localScale = zoneCollider.transform.localScale.FlattenZ();
        zoneCollider.isTrigger = false;
        zoneCollider.gameObject.layer = LayerMask.NameToLayer("UI");
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
            IsTrackActive = true;

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
            IsTrackActive = false;

            //Handled in the momemtum value directly with high decay rates past a threshold
            //if (Mathf.Abs(DirectionDeltaMomentum.Value) < 0.01f) 
            //    DirectionDeltaMomentum.Value = 0;
        }
    }


    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo) //Used in pancake mode
    {
        if (IsTrackActive) return;

        IsTrackActive = true;
        TrackDelta = Vector3.zero;
        DirectionDeltaMomentum.Set(0);
        activePointer = Sender;
        trackingPlane = new Plane(zoneCollider.transform.forward, zoneCollider.transform.position);
        TrackPosition = trackingPlane.GetRaycastPoint(activePointer.Ray);

        OnUserInput?.Invoke();
    }

    public void OnTriggerEnd(IUIPointer Sender) //Used in pancake mode
    {
        activePointer = null;
        IsTrackActive = false;
    }

    public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo) //Used in pancake mode
    {
        if (!isPancake) return;
        visualRenderer.enabled = true;
    }

    public void OnHoverEnd(IUIPointer Sender) //Used in pancake mode
    {
        if (!isPancake) return;
        visualRenderer.enabled = false;
    }


    private void FixedUpdate()
    {
        DirectionDeltaMomentum.SmoothingRate = SmoothingRate;
        DirectionDeltaMomentum.DecayRate = DecayRate;

        DirectionDeltaMomentum.Update();
        handleInteractorMovement();
        handleRayMovement();

        ScrollValue = DirectionDeltaMomentum.Value;
    }

    private void handleInteractorMovement()
    {
        if (activeInteractor == null) return;

        TrackPosition = activeInteractor.TransformPoint(localInteractorContactPoint);
        TrackDelta = TrackPosition - lastInteractorPosition;

        DirectionDeltaMomentum.Set(Vector3.Dot(transform.TransformDirection(direction), TrackDelta) * Speed);

        lastInteractorPosition = TrackPosition;
    }

    private void handleRayMovement()
    {
        if (activePointer == null) return;

        var newTrackPosition = trackingPlane.GetRaycastPoint(activePointer.Ray);
        var delta = Vector3.Dot(newTrackPosition - TrackPosition, zoneCollider.transform.right);
        DirectionDeltaMomentum.Set(delta);
        TrackPosition = newTrackPosition;
    }

    #region -- Unused UIPointer events --
    
    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnGripEnd(IUIPointer Sender)
    {
    }
    #endregion
}
