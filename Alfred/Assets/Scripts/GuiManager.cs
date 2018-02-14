using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
    public Canvas LoadingCanvas;
    public Canvas LiveDataCanvas;
    public Canvas ErrorCanvas;
    public Canvas PopUpCanvas;
    public GameObject TimeLineObject;
    public GameObject RoomNameUnderline;
    public Text ErrorMessageText;
    public StringReference ErrorMessage;

	public void ShowLoadingCanvas()
    {
        LiveDataCanvas.enabled = false;
        ErrorCanvas.enabled = false;
        PopUpCanvas.enabled = false;
        TimeLineObject.SetActive(false);
        RoomNameUnderline.SetActive(false);
        LoadingCanvas.enabled = true;
    }

    public void ShowDataCanvas()
    {
        LoadingCanvas.enabled = false;
        ErrorCanvas.enabled = false;
        PopUpCanvas.enabled = false;
        TimeLineObject.SetActive(true);
        RoomNameUnderline.SetActive(true);
        LiveDataCanvas.enabled = true;

    }

    public void ShowErrorCanvas()
    {
        LiveDataCanvas.enabled = false;
        LoadingCanvas.enabled = false;
        PopUpCanvas.enabled = false;
        TimeLineObject.SetActive(false);
        RoomNameUnderline.SetActive(false);
        ErrorMessageText.text = ErrorMessage.Value.Equals("") ? "An error ocurred..." : ErrorMessage.Value;
        ErrorCanvas.enabled = true;
    }

    public void ShowPopUpCanvas()
    {
        LiveDataCanvas.enabled = false;
        ErrorCanvas.enabled = false;
        LoadingCanvas.enabled = false;
        TimeLineObject.SetActive(false);
        RoomNameUnderline.SetActive(false);
        PopUpCanvas.enabled = true;

    }
}
