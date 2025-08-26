using System.Collections;
using System.IO;
using UnityEngine;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavePrizeEarned : MonoBehaviour
{
    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    [SerializeField] private GameObject _panelDownload;
    [SerializeField] private GameObject _panelGameOver;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonExit;
    [SerializeField] private Texture2D _imagePrize;
    [SerializeField] private GameObject _textIsSaved;

    private string _nameFile = "MisakPiurek_RegaloEspiral.pdf";

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

            string filePath = Path.Combine(Application.persistentDataPath, _nameFile);
            document.Save(filePath);
            document.Close();

            Debug.Log("PDF guardado en: " + filePath);
            StartCoroutine(NotificationOfSaved());
        }
    }

    private IEnumerator NotificationOfSaved()
    {
        _buttonRestart.onClick.AddListener(RestartGame);
        _buttonExit.onClick.AddListener(ExitGame);
        _textIsSaved.SetActive(true);
        yield return new WaitForSeconds(2);
        _textIsSaved.SetActive(false);
        _panelDownload.SetActive(false);
        _panelGameOver.SetActive(true);
    }
    
    private void RestartGame()
    {
        _soLevelSpiral.Level = 0;
        _soLevelSpiral.LevelsComplete.Clear();
        SceneManager.LoadScene("MenuAndLevel1");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}