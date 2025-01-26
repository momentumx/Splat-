using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItemScript : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	public static byte[] setItems;
	Transform target;
	// these take up 8 bytes so there is no reason NOT  to have 4 if you are going to have 1
	byte index;
	public bool unlocked, set;
	// Use this for initialization
	void Start()
	{
		setItems = new byte[5];
		index = byte.Parse(System.Text.RegularExpressions.Regex.Match(transform.name, @"\d+").Value);
		unlocked = Unlocked();
		if (!unlocked)
		{
			Color color = GetComponent<UnityEngine.UI.Image>().color;
			color.a = .2f;
			GetComponent<UnityEngine.UI.Image>().color = color;
		}
		else
		{
			sbyte i = -1; while (++i != 5)
				if (index == GameController.indexes[i])
				{
					set = true;
					ChangeTarget(GameObject.Find("ItemPick" + i).transform);
					return;
				}
		}
	}

	public void ChangeTarget(Transform _target)
	{
		target = _target;
		transform.position = target.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (unlocked)// unlocked playerprefs
			transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DraggableItemScript nearDraggable = null;
		foreach (DraggableItemScript draggable in FindObjectsOfType<DraggableItemScript>())
		{
			if (draggable != this && Vector2.Distance(draggable.transform.position, transform.position) < 40f)
			{
				nearDraggable = draggable;
				break;
			}
		}
		Transform oldTarget = target;
		if (nearDraggable && nearDraggable.unlocked)
		{
			set ^= nearDraggable.set;
			nearDraggable.set ^= set;
			set ^= nearDraggable.set;

			target = nearDraggable.target;
			nearDraggable.ChangeTarget(oldTarget);
			if (set)
			{
				setItems[int.Parse(System.Text.RegularExpressions.Regex.Match(target.name, @"\d+").Value)] = index;
			}

			if (nearDraggable.set)
			{
				setItems[int.Parse(System.Text.RegularExpressions.Regex.Match(nearDraggable.target.name, @"\d+").Value)] = index;
			}
		}
		transform.position = target.position;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		transform.SetAsLastSibling();
	}

	public virtual bool Unlocked()
	{
		return index <= GameController.stars / 6 + 1;
	}

	public void ChangeUnlocked(bool _unlocked)
	{
		unlocked = _unlocked;
		if (!unlocked)
		{
			Color color = GetComponent<UnityEngine.UI.Image>().color;
			color.a = .2f;
			GetComponent<UnityEngine.UI.Image>().color = color;
		}
	}

	public void CheckUnlocked()
	{
		unlocked = Unlocked();
		if (!unlocked)
		{
			Color color = GetComponent<UnityEngine.UI.Image>().color;
			color.a = .2f;
			GetComponent<UnityEngine.UI.Image>().color = color;
		}
	}
}