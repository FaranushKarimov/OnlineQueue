using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineQuee.Jobs
{
    public class Job
    {
        private Data.IRepository _Repository;
        private static Models.Setting _Setting;

        public Job()
        {
            _Repository = new Data.Repository();
            _Setting = _Repository.LoadSettings();
        }

        public void Run()
        {
            var timer = DateTime.Now;
            UploadCatigories();

            while (true)
            {
                if(SinceMinute(10, timer))
                {
                    UploadCatigories();
                    timer = DateTime.Now;
                }
                Thread.Sleep(300000); // 5 min
            }

        }

        private bool SinceMinute(int m, DateTime sinceTime)
        {
            TimeSpan ts = DateTime.Now.Subtract(sinceTime);
            if (ts.TotalMinutes >= m)
                return true;
            return false;
        }


        private async void UploadCatigories()
        {
            try
            {
                HttpClientService.Instance.Headers(_Setting);

                var response = await HttpClientService.Instance.Get<Models.GetCatigoriesResponse>("api/reception");
                if (response.Code != 200)
                {
                    Utils.Logger.CoreLogger.Error($"Uploading Catigories failed, code: {response.Code} description: {response.Message}");
                    return;
                }

                var res = _Repository.SaveCategories(response.Categories);
                if (!res)
                {
                    Utils.Logger.CoreLogger.Error("Saving Uploaded Catigories failed");
                    return;
                }

                // var jsonStr = JsonConvert.SerializeObject(response.Categories.ToArray());
                // string script = $"render('{jsonStr}')";
                // WebControl.InvokeScript("eval", script);
            }
            catch (HttpClientService.ApiException ex)
            {
                Utils.Logger.CoreLogger.ErrorException("Uploading Categories failed, auth got exception:", ex);
            }
            catch (Exception ex)
            {
                Utils.Logger.CoreLogger.ErrorException("Uploading Categories failed, got exception:", ex);
            }
        }
        
    }
}
