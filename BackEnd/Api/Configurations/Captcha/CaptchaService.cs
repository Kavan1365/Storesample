using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Api.Configurations.Captcha
{
    public class CaptchaResult
    {
        public string CaptchaCode { get; set; }
        public string CaptchaId { get; set; }
        public string CaptchaImage { get; set; }
        public DateTime Timestamp { get; set; }
    }
    public static class CaptchaList
    {
        public static List<CaptchaResult> Captchas { get; set; }
    }
    
    public interface ICaptchaService
    {
        Task<string> GenerateCaptcha();
        bool ValidateCaptcha(string CaptchaCode, string CaptchaId);
    }
    
    public class CaptchaService : ICaptchaService
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public CaptchaService(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> GenerateCaptcha()
        {
            var CaptchaId = Guid.NewGuid().ToString();
            var filename = @"Captchas\" + CaptchaId + ".png";
            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, filename);

            CaptchaResult captchaResult = new CaptchaResult()
            {
                CaptchaId = CaptchaId,
                CaptchaImage = filename,
                Timestamp = DateTime.Now
            };

            string Letters = "012346789";
            int width = 320, height = 70;
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }
            string captchaCode = sb.ToString();
            captchaResult.CaptchaCode = captchaCode;

            using Bitmap baseMap = new Bitmap(width, height);
            using Graphics graph = Graphics.FromImage(baseMap);
            graph.Clear(GetRandomLightColor());

            DrawCaptchaCode();
            DrawDisorderLine();
            AdjustRippleEffect();

            MemoryStream ms = new MemoryStream();

            using (FileStream fs = new FileStream(resourceFile, FileMode.Create, FileAccess.ReadWrite))
            {
                baseMap.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                fs.Write(bytes, 0, bytes.Length);
            };

            if (CaptchaList.Captchas is null)
                CaptchaList.Captchas = new List<CaptchaResult>();

            CaptchaList.Captchas.Add(captchaResult);
            return captchaResult.CaptchaId;

            static int GetFontSize(int imageWidth, int captchCodeCount)
            {
                var averageSize = imageWidth / captchCodeCount;

                return Convert.ToInt32(averageSize);
            }

            Color GetRandomDeepColor()
            {
                int redlow = 160, greenLow = 100, blueLow = 160;
                return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
            }

            Color GetRandomLightColor()
            {
                int low = 180, high = 255;

                int nRend = rand.Next(high) % (high - low) + low;
                int nGreen = rand.Next(high) % (high - low) + low;
                int nBlue = rand.Next(high) % (high - low) + low;

                return Color.FromArgb(nRend, nGreen, nBlue);
            }

            void DrawCaptchaCode()
            {
                SolidBrush fontBrush = new SolidBrush(Color.Black);
                int fontSize = GetFontSize(width-100, captchaCode.Length);
                Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                for (int i = 0; i < captchaCode.Length; i++)
                {
                    fontBrush.Color = GetRandomDeepColor();

                    int shiftPx = fontSize / 6;

                    float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                    int maxY = height - fontSize;
                    if (maxY < 0) maxY = 0;
                    float y = rand.Next(0, maxY);

                    graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                }
            }

            void DrawDisorderLine()
            {
                Pen linePen = new Pen(new SolidBrush(Color.Black), 3);
                for (int i = 0; i < rand.Next(3, 5); i++)
                {
                    linePen.Color = GetRandomDeepColor();

                    Point startPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                    Point endPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                    //graph.DrawLine(linePen, startPoint, endPoint);

                    Point bezierPoint1 = new Point(rand.Next(0, width), rand.Next(0, height));
                    Point bezierPoint2 = new Point(rand.Next(0, width), rand.Next(0, height));

                    graph.DrawBezier(linePen, startPoint, bezierPoint1, bezierPoint2, endPoint);
                }
            }

            void AdjustRippleEffect()
            {
                short nWave = 6;
                int nWidth = baseMap.Width;
                int nHeight = baseMap.Height;

                Point[,] pt = new Point[nWidth, nHeight];

                for (int x = 0; x < nWidth; ++x)
                {
                    for (int y = 0; y < nHeight; ++y)
                    {
                        var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                        var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                        var newX = x + xo;
                        var newY = y + yo;

                        if (newX > 0 && newX < nWidth)
                        {
                            pt[x, y].X = (int)newX;
                        }
                        else
                        {
                            pt[x, y].X = 0;
                        }


                        if (newY > 0 && newY < nHeight)
                        {
                            pt[x, y].Y = (int)newY;
                        }
                        else
                        {
                            pt[x, y].Y = 0;
                        }
                    }
                }

                Bitmap bSrc = (Bitmap)baseMap.Clone();

                BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                int scanline = bitmapData.Stride;

                IntPtr scan0 = bitmapData.Scan0;
                IntPtr srcScan0 = bmSrc.Scan0;

                unsafe
                {
                    byte* p = (byte*)(void*)scan0;
                    byte* pSrc = (byte*)(void*)srcScan0;

                    int nOffset = bitmapData.Stride - baseMap.Width * 3;

                    for (int y = 0; y < nHeight; ++y)
                    {
                        for (int x = 0; x < nWidth; ++x)
                        {
                            var xOffset = pt[x, y].X;
                            var yOffset = pt[x, y].Y;

                            if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                            {
                                if (pSrc != null)
                                {
                                    p[0] = pSrc[yOffset * scanline + xOffset * 3];
                                    p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
                                    p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                                }
                            }

                            p += 3;
                        }
                        p += nOffset;
                    }
                }

                baseMap.UnlockBits(bitmapData);
                bSrc.UnlockBits(bmSrc);
                bSrc.Dispose();
            }
        }

        public bool ValidateCaptcha(string CaptchaCode, string CaptchaId)
        {
            var captchas = CaptchaList.Captchas.Where(i => i.Timestamp.AddMinutes(1) < DateTime.Now).ToList();

            foreach (var item in captchas)
            {
                var resourceFileitem = Path.Combine(hostingEnvironment.WebRootPath, item.CaptchaImage);
                File.Delete(resourceFileitem);
                CaptchaList.Captchas.Remove(item);
            }


            var captcha = CaptchaList.Captchas.Find(i => i.CaptchaId == CaptchaId);
            if (captcha is null)
                return false;

            bool IsValid = CaptchaCode == captcha.CaptchaCode;
            
            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, captcha.CaptchaImage);
            CaptchaList.Captchas.Remove(captcha);
            File.Delete(resourceFile);

            return IsValid;
        }
    }
}
