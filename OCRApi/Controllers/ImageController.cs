using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OCRApi.BLL;
using OCRApi.Models;

namespace OCRApi.Controllers
{
    public class ImageController : ApiController
    {
        // GET api/values/5
        public string Get(string value, int RegRegionMode)
        {
            GetImageVlaue getImage = new GetImageVlaue();
            Dictionary<int, OCRResult> result = new Dictionary<int, OCRResult>();
            Dictionary<int, OCRResult> resultASC = new Dictionary<int, OCRResult>();
            string returnValue = "辨識失敗";

            try
            {
                result = getImage.getString(value, RegRegionMode);

                if (result.Count > 0)
                {
                    returnValue = "圖片名稱為" + value + "，後續為辨識結果";

                    resultASC = result.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                    foreach (KeyValuePair<int, OCRResult> dic in resultASC)
                    {
                        returnValue += "第" + (dic.Key + 1) + "筆<start>" + dic.Value.RecognizedString + "<end>";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return returnValue;
        }
    }
}
