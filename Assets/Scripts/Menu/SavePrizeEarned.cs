using System.Collections;
using System.IO;
using UnityEngine;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

public class SavePrizeEarned : MonoBehaviour
{
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

            string filePath = Path.Combine(Application.persistentDataPath, _nameFile);
            document.Save(filePath);
            document.Close();

            Debug.Log("PDF guardado en: " + filePath);
            StartCoroutine(NotificationOfSaved());
        }
    }

    private IEnumerator NotificationOfSaved()
    {
        _textIsSaved.SetActive(true);
        yield return new WaitForSeconds(2);
        _textIsSaved.SetActive(false);
    }
}