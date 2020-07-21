using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using OCRApi.Models;
using PPOCRSDK_CSharpDLL;

namespace OCRApi.BLL
{
    public class GetImageVlaue
    {
        //System.Windows.Shapes.Path RecognizePath = new System.Windows.Shapes.Path();

        PPOCRObject obj = null;
        List<PPOCRResult> ResultList = null;
        private string LoadedImagePath = "D:\\OCR_Folder\\";
        //private int LoadedImageWidth = 305;
        //private int LoadedImageHeight = 116;
        //BackgroundWorker RecognizeWorker = null;

        #region RecognizeParameter
        class RecognizeParameter
        {
            public string FilePath;
            public int RecognizeMode;
            public bool IsPartialRecognize;
            public RECT RecognizeRect;

            public bool UseRecogAS;
        }
        #endregion

        #region RecognizeResult
        class RecognizeResult
        {
            public PPOCRCode pResultCode;
            public List<PPOCRResult> pResultList;
        }
        #endregion

        public Dictionary<int, OCRResult> getString(string path, int RegRegionMode)
        {
            string value = "test";
            object o_config = "allConfig";
            string OCRImagePath = LoadedImagePath + path;
            Dictionary<int, OCRResult> returnValue = new Dictionary<int, OCRResult>();
            List<RECT> lRecognizeRect = new List<RECT>();

            #region -- config & PPOCRResult init --
            RecognizeSetting config = new RecognizeSetting();
            config.CHConvert = false;
            config.RecogAuto = false;
            config.RecogHEX = false;
            config.RecogSChar = false;
            config.RecogHChar = false;
            config.RecogKChar = false;

            config.RegWndLang = (int)PPI18N.Lang.zhTW;

            config.RegRegionMode = 0;

            obj = new PPOCRObject();

            ResultList = new List<PPOCRResult> { };
            #endregion

            #region -- OCR Button_click function --
            PPI18N.Lang RegWndLang = (PPI18N.Lang)config.RegWndLang;

            PPOCRCode InitCode = obj.PPOCR_Init(RegWndLang);
            #endregion

            //RecognizeSetting config = FrameworkElement.FindResource(o_config) as RecognizeSetting;
            //RecognizeSetting config = new RecognizeSetting();
            bool IsPartialRecognize = false;
            RECT RecognizeRect;
            //RECT RecognizeRect;

            config.RegRegionMode = RegRegionMode;

            //bool DrawRecognizePathAgain = false;
            if (config.RegRegionMode == 1)
            {
                //if (!DrawCanvas.Children.Contains(RecognizePath))
                //{
                //    MessageBox.Show("選擇手刮辨識，請先使用 [滑鼠右鍵] 框選辨識區域");
                //    return;
                //}
                //else
                //{

                #region -- create rect list --
                // 委託人 TeddyYin
                RecognizeRect = new RECT(164, 156, 570, 214);
                lRecognizeRect.Add(RecognizeRect);

                // 名稱 TEST NAME
                RecognizeRect = new RECT(166, 511, 434, 549);
                lRecognizeRect.Add(RecognizeRect);

                // 帳號 123456789
                RecognizeRect = new RECT(533, 510, 762, 549);
                lRecognizeRect.Add(RecognizeRect);
                #endregion

                IsPartialRecognize = true;

                //double XRatio;
                //double YRatio;
                //XRatio = LoadedImageWidth / ImgDisplay.ActualWidth;
                //YRatio = LoadedImageHeight / ImgDisplay.ActualHeight;
                //XRatio = 0.0;
                //YRatio = 0.0;

                //RectangleGeometry tmpRect = (RectangleGeometry)RecognizePath.Data;

                //RecognizeRect = new RECT((int)(tmpRect.Rect.Left * XRatio), (int)(tmpRect.Rect.Top * YRatio),
                //                        (int)(tmpRect.Rect.Right * XRatio), (int)(tmpRect.Rect.Bottom * YRatio));
                RecognizeRect = new RECT(0, 0, 0, 0);

                //}

                //if (DrawCanvas.Children.Contains(RecognizePath))
                //    DrawRecognizePathAgain = true;
            }
            else
            {
                RecognizeRect = new RECT(0, 0, 0, 0);
                lRecognizeRect.Add(RecognizeRect);
            }

            string FilePath = OCRImagePath;

            //DrawCanvas.Children.Clear();
            ResultList.Clear();

            //if (DrawRecognizePathAgain)
            //    DrawCanvas.Children.Add(RecognizePath);

            int pRecognizeMode = GetRecogSetting();

            //if (RecognizeWorker == null)
            //{
            //    RecognizeWorker = new BackgroundWorker();
            //    RecognizeWorker.DoWork += RecognizeWorker_DoWork;
            //    RecognizeWorker.RunWorkerCompleted += RecognizeWorker_RunWorkerCompleted;
            //}

            //RecognizeWorker = new BackgroundWorker();
            //RecognizeWorker.DoWork += RecognizeWorker_DoWork;
            //RecognizeWorker.RunWorkerCompleted += RecognizeWorker_RunWorkerCompleted;

            //if (RecognizeWorker.IsBusy)
            //{
            //    MessageBox.Show("上一次的辨識任務正在進行中");
            //    return;
            //}

            int dictionaryKey = 0;

            foreach (RECT rect in lRecognizeRect)
            {
                IsPartialRecognize = true;

                RecognizeParameter param = new RecognizeParameter();
                param.FilePath = FilePath;
                param.RecognizeMode = pRecognizeMode;
                param.IsPartialRecognize = IsPartialRecognize;
                param.RecognizeRect = rect;
                param.UseRecogAS = true;

                RecognizeResult result = new RecognizeResult();
                result = IdentifyImage(param);
                //RecognizeWorker.RunWorkerAsync(param);

                if (result.pResultCode == PPOCRCode.Success)
                {
                    foreach(PPOCRResult PPOCRtemp in result.pResultList)
                    {
                        PPOCRResult oPPOCRResult = new PPOCRResult();
                        OCRResult oOCRResult = new OCRResult();

                        oPPOCRResult = PPOCRtemp;

                        oOCRResult.top = oPPOCRResult.top;
                        oOCRResult.bottom = oPPOCRResult.bottom;
                        oOCRResult.left = oPPOCRResult.left;
                        oOCRResult.right = oPPOCRResult.right;
                        oOCRResult.RecognizedString = oPPOCRResult.RecognizedString.Trim();
                        oOCRResult.PPCharList = oPPOCRResult.PPCharList;

                        returnValue.Add(dictionaryKey, oOCRResult);

                        dictionaryKey++;
                    }

                    //for (int i = 0; i < result.pResultList.Count(); i++)
                    //{
                    //    PPOCRResult oPPOCRResult = new PPOCRResult();
                    //    OCRResult oOCRResult = new OCRResult();

                    //    oPPOCRResult = result.pResultList[i];

                    //    oOCRResult.top = oPPOCRResult.top;
                    //    oOCRResult.bottom = oPPOCRResult.bottom;
                    //    oOCRResult.left = oPPOCRResult.left;
                    //    oOCRResult.right = oPPOCRResult.right;
                    //    oOCRResult.RecognizedString = oPPOCRResult.RecognizedString;
                    //    oOCRResult.PPCharList = oPPOCRResult.PPCharList;

                    //    returnValue.Add(i, oOCRResult);
                    //}
                }
            }

            return returnValue;
        }

        #region GetRecogSetting
        private int GetRecogSetting()
        {
            RecognizeSetting config = new RecognizeSetting();

            #region -- config & PPOCRResult init --
            config.CHConvert = false;
            config.RecogAuto = false;
            config.RecogHEX = false;
            config.RecogSChar = false;
            config.RecogHChar = false;
            config.RecogKChar = false;

            config.RegWndLang = (int)PPI18N.Lang.zhTW;

            config.RegRegionMode = 0;
            #endregion

            int pRecognizeMode = 0;

            switch (config.RecogLang)
            {
                case 0: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.TRADITIONAL_MODE; break;
                case 1: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.SIMPLIFIED_MODE; break;
                case 2: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.JAPAN_MODE; break;
                case 3: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.KOREA_MODE; break;
                case 4: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.ENGLISH_MODE; break;
                case 5: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.FRANCE_MODE; break;
                case 6: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.GERMAN_MODE; break;
                case 7: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.HOLLAND_MODE; break;

                case 8: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.ITALY_MODE; break;
                case 9: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.SPAIN_MODE; break;
                case 10: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.SWEDEN_MODE; break;
                case 11: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.PORTUGAL_MODE; break;
                case 12: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.NORWAY_MODE; break;
                case 13: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.DENMARK_MODE; break;
                case 14: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.FINLAND_MODE; break;
                case 15: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.TURKY_MODE; break;

                case 16: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.RUSSIA_MODE; break;
                case 17: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.POLAND_MODE; break;
                case 18: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.HUNGARY_MODE; break;
                case 19: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.SLOVENIA_MODE; break;
                case 20: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.ROMANIA_MODE; break;
                case 21: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CZECH_MODE; break;
                case 22: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.SLOVAKIA_MODE; break;
                case 23: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CROATIA_MODE; break;

                case 24: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.GREEK_MODE; break;
                case 25: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.OCRAB_MODE; break;
                case 26: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.INDONESIA_MODE; break;
                case 27: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.ARABIC_MODE; break;
                case 28: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.MICR_MODE; break;
                case 29: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.VIETNAM_MODE; break;
                case 30: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.MALAYSIA_MODE; break;
                case 31: pRecognizeMode = pRecognizeMode | (int)RecognizeMode.THAI_MODE; break;

            }


            if (config.RecogCH)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_SET_CHINESE;

            if (config.RecogENG)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_SET_ENGLISH;

            if (config.RecogNUM)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_SET_DIGITAL;

            if (config.RecogSYM)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_SET_SYMBOL;

            if (config.RecogHEX)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_SET_HEX;

            if (config.RecogSChar)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.USE_SECONDARY_CHARSET;

            if (config.RecogHChar)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.USE_HK_CHARSET;

            if (config.RecogKChar)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.USE_KOR_HZ_CHARSET;

            if (config.CHConvert)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.OUTPUTCODE_CONVERT;

            if (config.RecogOrientation == 0)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.HORIZONTAL_LINE;
            else
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.VERTICATL_LINE;

            if (config.RecogDegree == 1)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_ROTATE_90;
            else if (config.RecogDegree == 2)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_ROTATE_180;
            else if (config.RecogDegree == 3)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.CHAR_ROTATE_270;
            else
            {
                //Not Setting Degree = 0
            }

            if (config.RecogAuto)
                pRecognizeMode = pRecognizeMode | (int)RecognizeMode.AUTO_MODE;

            return pRecognizeMode;
        }
        #endregion

        #region DrawPPOCRResult
        private void DrawPPOCRResult(PPOCRResult Obj, double XRatio, double YRatio)
        {
            if (Obj == null)
                return;

            //MessageBox.Show(Obj.left + " - " + Obj.top + " - " + Obj.right + " - " + Obj.bottom);
            double X = (Obj.left * XRatio);
            double Y = (Obj.top * YRatio);
            double Width = Math.Max(((Obj.right - Obj.left) * XRatio), 0);
            double Height = Math.Max(((Obj.bottom - Obj.top) * YRatio), 0);
            //var rect = new System.Windows.Shapes.Path
            //{
            //    Data = new RectangleGeometry(new Rect(X, Y, Width, Height)),
            //    Stroke = Brushes.Red,
            //    StrokeThickness = 2
            //};
            //DrawCanvas.Children.Add(rect);
        }
        #endregion

        #region IdentifyImage
        private RecognizeResult IdentifyImage(RecognizeParameter param)
        {
            //RecognizeParameter param = (RecognizeParameter)e.Argument;

            List<PPOCRResult> pResultList = new List<PPOCRResult> { };

            bool IsParticalRecognize = param.IsPartialRecognize;
            RECT RecognizeRect = param.RecognizeRect;
            bool UseRecogAS = param.UseRecogAS;

            PPOCRCode ret;
            if (UseRecogAS)
            {
                if (IsParticalRecognize)
                    ret = obj.PPOCR_StartRecognizeByRect_AS(param.FilePath, param.RecognizeMode, RecognizeRect, ref pResultList);
                else
                    ret = obj.PPOCR_StartRecognize_AS(param.FilePath, param.RecognizeMode, ref pResultList); // System.StackOverflowException
            }
            else
            {
                if (IsParticalRecognize)
                    ret = obj.PPOCR_StartRecognizeByRect(param.FilePath, param.RecognizeMode, RecognizeRect, ref pResultList);
                else
                    ret = obj.PPOCR_StartRecognize(param.FilePath, param.RecognizeMode, ref pResultList);
            }
            RecognizeResult result = new RecognizeResult();
            result.pResultCode = ret;
            result.pResultList = pResultList;

            return result;
        }
        #endregion

        #region RecognizeWorker_DoWork
        private void RecognizeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RecognizeParameter param = (RecognizeParameter)e.Argument;

            List<PPOCRResult> pResultList = new List<PPOCRResult> { };

            bool IsParticalRecognize = param.IsPartialRecognize;
            RECT RecognizeRect = param.RecognizeRect;
            bool UseRecogAS = param.UseRecogAS;

            PPOCRCode ret;
            if (UseRecogAS)
            {
                if (IsParticalRecognize)
                    ret = obj.PPOCR_StartRecognizeByRect_AS(param.FilePath, param.RecognizeMode, RecognizeRect, ref pResultList);
                else
                    ret = obj.PPOCR_StartRecognize_AS(param.FilePath, param.RecognizeMode, ref pResultList); // System.StackOverflowException
            }
            else
            {
                if (IsParticalRecognize)
                    ret = obj.PPOCR_StartRecognizeByRect(param.FilePath, param.RecognizeMode, RecognizeRect, ref pResultList);
                else
                    ret = obj.PPOCR_StartRecognize(param.FilePath, param.RecognizeMode, ref pResultList);
            }
            RecognizeResult result = new RecognizeResult();
            result.pResultCode = ret;
            result.pResultList = pResultList;

            e.Result = result;
        }
        #endregion

        #region RecognizeWorker_RunWorkerCompleted
        private void RecognizeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RecognizeResult result = (RecognizeResult)e.Result;

            if (result.pResultCode == PPOCRCode.Success)
            {
                ResultList = result.pResultList;

                //double XRatio;
                //double YRatio;
                //XRatio = ImgDisplay.ActualWidth / LoadedImageWidth;
                //YRatio = ImgDisplay.ActualHeight / LoadedImageHeight;
                //XRatio = 0.0;
                //YRatio = 0.0;

                for (int i = 0; i < ResultList.Count; i++)
                {
                    //DrawPPOCRResult(ResultList[i], XRatio, YRatio);

                    // ResultList[i] 為辨識後的結果
                    //ResultList[i]
                }
            }
            else
            {
                MessageBox.Show("ErrorCode : " + result.pResultCode);
            }
        }
        #endregion

    }
}