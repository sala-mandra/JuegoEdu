using System.Collections;
using System.IO;
using UnityEngine;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using UnityEngine.UI;

public class SavePrizeEarned : MonoBehaviour
{
    [SerializeField] private GameObject _panelDownload;
    [SerializeField] private GameObject _panelGameOver;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonExit;
    [SerializeField] private Texture2D _imagePrize;
    [SerializeField] private GameObject _textIsSaved;

    private string _nameFile = "prize.pdf";

    public void SaveAsPDF()
    {
        byte[] pngData = _imagePrize.EncodeToPNG();

        using (MemoryStream ms = new MemoryStream(pngData))
        {
            XImage xImage = XImage.FromStream(() => ms);

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();

            page.Width = xImage.PixelWidth * 72 / xImage.HorizontalResolution;
            page.Height = xImage.PixelHeight * 72 / xImage.VerticalResolution;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);

            //string filePath = Path.Combine(GetDownloadsPath(), _nameFile);
            string filePath = Path.Combine(Application.persistentDataPath, _nameFile);
            document.Save(filePath);
            document.Close();

            Debug.Log("PDF guardado en: " + filePath);
            StartCoroutine(NotificationOfSaved());
        }
    }
    
    private string GetDownloadsPath()
    {
        using (var environment = new AndroidJavaClass("android.os.Environment"))
        {
            var downloadsDir = environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", environment.GetStatic<string>("DIRECTORY_DOWNLOADS"));
            return downloadsDir.Call<string>("getAbsolutePath");
        }
    }

    private IEnumerator NotificationOfSaved()
    {
        _buttonRestart.onClick.AddListener(LevelsController.Instance.RestartGame);
        _buttonExit.onClick.AddListener(LevelsController.Instance.ExitGame);
        _textIsSaved.SetActive(true);
        yield return new WaitForSeconds(2);
        _textIsSaved.SetActive(false);
        _panelDownload.SetActive(false);
        _panelGameOver.SetActive(true);
    }
}