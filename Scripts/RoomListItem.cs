using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
	[SerializeField] Text text;
    [SerializeField] Text playerCountText;
    public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		info = _info;
		text.text = _info.Name;
    }

	public void OnClick()
	{
		Launcher.Instance.JoinRoom(info);
	}
}