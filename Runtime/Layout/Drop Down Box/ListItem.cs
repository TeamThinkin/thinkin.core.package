using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItemDto
{
    public object Value { get; set; }
    public string Text { get; set; }
}

public class ListItem : ButtonInteractable
{
    public DropDownBox ParentListControl;
    public ListItemDto Dto { get; private set; }

    public void SetDto(ListItemDto Dto)
    {
        this.Dto = Dto;
        Label.text = Dto.Text;
    }

    protected override void Pressed()
    {
        base.Pressed();
        ParentListControl.ListItem_Selected(this);
    }
}
