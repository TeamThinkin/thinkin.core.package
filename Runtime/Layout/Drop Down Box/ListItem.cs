using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItemDto
{
    public object Value { get; set; }
    public string Text { get; set; }
    public float Width { get; set; }
}

public class ListItem : ButtonInteractable
{
    [SerializeField] private Transform Background;
    [SerializeField] private BoxCollider Collider;

    public DropDownBox ParentListControl;
    public ListItemDto Dto { get; private set; }

    private Renderer backgroundRenderer;

    private void Awake()
    {
        isHoverScaleFeedbackEnabled = false;
        backgroundRenderer = Background.GetComponent<Renderer>();
        backgroundRenderer.enabled = false;
    }

    public void SetDto(ListItemDto Dto)
    {
        this.Dto = Dto;
        Label.text = Dto.Text;
        Background.localScale = new Vector3(Dto.Width * 1.03f, Background.localScale.y, Background.localScale.z);

        var size = Collider.size;
        size.x = Dto.Width;
        Collider.size = size;

        Label.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Dto.Width / Label.transform.localScale.x);
    }

    protected override void Pressed()
    {
        base.Pressed();
        ParentListControl.ListItem_Selected(this);
    }

    public override void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        base.OnHoverStart(Sender, RayInfo);
        backgroundRenderer.enabled = true;
    }

    public override void OnHoverEnd(IUIPointer Sender)
    {
        base.OnHoverEnd(Sender);
        backgroundRenderer.enabled = false;
    }
}
