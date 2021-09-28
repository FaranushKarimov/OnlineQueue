using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Newtonsoft.Json;
using OnlineQuee.Data;
using OnlineQuee.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineQuee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IRepository _Repository;
        private static Models.Setting _Setting;
        private Jobs.Job _Job;
        private Printer _Printer;
        private int CategoryId = 0;

        [Obsolete]
        public MainWindow()
        {

            // this.Topmost = true;
            // Taskbar.Hide();
            Logger.Init();
            _Repository = new Repository();
            _Printer = new Printer();
            InitializeComponent();

            try
            {
                _Setting = _Repository.LoadSettings();
                var cat = _Repository.GetCatigories();
                if (_Setting == null || !_Setting.Enable)
                {
                    Logger.CoreLogger.Info("_Repository LoadSettings is null");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.CoreLogger.WarnException("DB, Loading Settings error", ex);
            }

            InitWebView();
            _Job = new Jobs.Job();

            var job = new Thread(_Job.Run) { IsBackground = true };
            job.Start();
        }
        [Obsolete]
        private void InitWebView()
        {
            try
            {

                if (_Setting == null)
                {
                    _Setting = _Repository.LoadSettings();
                }
                HttpClientService.Instance.Headers(_Setting);
                WebControl.Visibility = Visibility.Visible;
                WebControl.ScriptNotify += WebControl_ScriptNotify;
                WebControl.NavigateToLocal("/wwwroot/index.html");
                WebControl.NavigationCompleted += async (o, e) =>
                {
                    Logger.CoreLogger.Info("WebView Navigation Completed");
                };
            }
            catch (Exception ex)
            {
                Logger.CoreLogger.ErrorException("Initing Webview failed:", ex);
            }
        }

        private void WebControl_ScriptNotify(object sender, WebViewControlScriptNotifyEventArgs e)
        {
            try
            {
                Logger.CoreLogger.Info($"{e.Value} {e.Uri}");
                var data = JsonConvert.DeserializeObject<Models.WebBrowserDto>(e.Value);
                switch (data.Action)
                {
                    case "get_users":
                        if(int.TryParse(data.Data, out int id))
                        {
                            CategoryId = id;
                            var users = _Repository.GetUsers(id);
                            var jsonStr = JsonConvert.SerializeObject(users);
                            string script = $"showUsers('{jsonStr}')";
                            WebControl.InvokeScript("eval", script);
                        }
                        else
                        {
                            Logger.CoreLogger.Error($"WebControl_ScriptNotify get_catigories got wrong data:{data} " +
                                $"parce failed on {data.Data}");
                        }
                        break;
                    case "get_categories":
                        try
                        {
                            var categories = _Repository.GetCatigories();
                            // categories.RemoveAt(2);
                            var jsonStr = JsonConvert.SerializeObject(categories.ToArray());
                            //var jsonStr = JsonConvert.SerializeObject(categories);
                            var script = $"render('{jsonStr}')";
                            WebControl.InvokeScript("eval", script);
                        }
                        catch (Exception ex)
                        {
                            Logger.CoreLogger.ErrorException("WebControl_ScriptNotify get_catigories failed", ex);
                        } 

                        break; 
                    case "add_queue":
                        Logger.CoreLogger.Info("click", DateTime.UtcNow);
                        int userId = int.Parse(data.Data);
                        AddToQueue(CategoryId, userId, ""); //$"992{data.Phone}");
                        CategoryId = 0;
                        break;
                }
                
            }
            catch (Exception ex)
            {
                Logger.CoreLogger.ErrorException("WebControl_ScriptNotify error:", ex);
            }
        }

        [Obsolete]
        private async void Login_Button_Click(object sender, RoutedEventArgs e)
        {

            var setting = new Models.Setting
            {
                Id = Guid.NewGuid().ToString(),
                Login = EmailTextBox.Text,
                Password = PasswordTextBox.Text,
                URL = URlTextBox.Text,
                Token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9uZXdxLnNhZm9ldi5iZWdldC50ZWNoXC9hcGlcL2xvZ2luIiwiaWF0IjoxNjMyNTc3MDkyLCJleHAiOjE2NjQxMzQ2OTIsIm5iZiI6MTYzMjU3NzA5MiwianRpIjoiemxXb1hEZHhuMjl6ZHdmOCIsInN1YiI6MTUsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.LcV1Sji59p5er1HEu7OV-x4Phs6xT_jmgKKcXYR4Lic",
                CreatedAt = DateTime.Now,
                Enable = true
            };
           
            try
            {
                setting.Token = await CheckLoginAsync(setting);

                if (!_Repository.SaveSettings(setting))
                    LoginTextBlock.Text = "Ошибка при сохранении данных, попробуйте еще раз.";
                InitWebView();
            }
            catch (Exception ex)
            {
                LoginTextBlock.Text = $"Ошибка при сохранении данных: { ex.Message}";
                Logger.CoreLogger.ErrorException("Login error:", ex);
            }
        }

        [Obsolete]
        private async Task<string> CheckLoginAsync(Models.Setting setting)
        {
            if (String.IsNullOrWhiteSpace(setting.Login) || setting.Login.Length < 5)
                throw new Exception("Ввидите майл");

            if (String.IsNullOrWhiteSpace(setting.Password) || setting.Password.Length < 5)
                throw new Exception("Ввидите пароль");

            var content = new StringContent(
                JsonConvert.SerializeObject(new { email = setting.Login, password = setting.Password }), 
                Encoding.UTF8, 
                "application/json");
            //HttpClientService.Instance.Headers(setting);
            var response = await HttpClientService.Instance.PostMultypart<Models.LoginResponse>("api/login", content);
            if (!response.Success)
                throw new Exception("Неверный емайл или пароль");
            return response.Data.Token;
        }


        private async void AddToQueue(int categoryId, int userId, string phone)
        {
            HttpClientService.Instance.Headers(_Setting);
            var content = new StringContent(JsonConvert.SerializeObject(
                new
                {
                    category_id = categoryId,
                    user_id = userId,
                    phone = "992111111111"
                }),
                Encoding.UTF8, "application/json");

            var response = await HttpClientService.Instance.PostMultypart<Models.AddToQueue>("api/reception", content);
            if (response.Code != 200)
            {
                Logger.CoreLogger.Error($"AddToQueue failed, code:{response.Code} message:{response.Message}");
                return;
            }

            // get category by id
            var category = _Repository.GetCatigoryById(categoryId);
            var categoryName = category?.Name;
            // get user by id
            var user = _Repository.GetUserById(userId);
            var userName = user?.FullName;
            //var maxLength = 16;
            //if (userName.Length > maxLength)
            //    userName = userName.Remove(maxLength, userName.Length- maxLength)+"..";

            var jsonStr = JsonConvert.SerializeObject(
                new
                {
                    id = response.Ticket.Id,
                    category_id = categoryId,
                    category = categoryName,
                    user_id = userId,
                    user = userName,
                    number = response.Ticket.Number,
                    created_at = DateTime.Now.ToString("dd'-'MM'-'yyyy HH:mm:ss") //("dd'-'MM'-'yyyy")
                });
            // 29 - length of line
            // 34
            // 25
            var check = $"--------------------------\n" +
                $"НАВБАТИ ЭЛЕКТРОНӢ\n\n" +
                $"Рақами навбат          {response.Ticket.Number}\n" +
                $"Самт              {categoryName}\n" +
                $"Ҳуҷра      {userName}\n" +
                $"Сана          {response.Ticket.CreatedAt.ToString("dd'-'MM'-'yyyy HH:mm:ss")}\n" +
                $"-----------------------\n";

            string script = $"showTicket('{jsonStr}')";
            try
            {
                WebControl.InvokeScript("eval", script);
                _Printer.Print(check, 11);

            }
            catch (Exception ex)
            {
                Logger.CoreLogger.ErrorException($"AddToQueue error to print or to show", ex);
            }
        }

    }
}
