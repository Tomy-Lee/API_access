using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;
using System;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using System.IO;

namespace CheckNumber
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            location.Text = "";
            runner.Text = "";
            emps.Text = "";
            areacode.Text = "";
            var str1 = number.Text.ToString();
            if(str1.Length > 11)
            {
                var msgdialog2 = new MessageDialog("您输入的号码过长，请重新输入").ShowAsync();
            }
            else
            {
                queryAsync(number.Text);
            } 
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            jsoncompany.Text = "";
            jsonprovince.Text = "";
            jsonWD.Text = "";
            jsonSD.Text = "";
            var str2 = number2.Text.ToString();
            if (str2.Length != 9)
            {
                var msgdialog2 = new MessageDialog("您输入的编号格式不正确，请重新输入").ShowAsync();
            }
            else
            {
                queryJson(number2.Text);
            }
        }

        async void queryJson(string tel)
        {
            try
            {
                string url = "http://www.weather.com.cn/data/sk/" + tel +".html";
                HttpClient client = new HttpClient();
                string result = await client.GetStringAsync(url);
                JsonReader reader = new JsonTextReader(new StringReader(result));
                while (reader.Read()){
                    switch ((string)reader.Value)
                     {
                        case "city":
                             reader.Read();
                            jsonprovince.Text += (string)reader.Value;
                            break;
                        case "temp":
                            reader.Read();
                            jsoncompany.Text += (string)reader.Value + "℃";
                            break;
                        case "WD":
                            reader.Read();
                            jsonWD.Text += (string)reader.Value;
                            break;
                        case "WS":
                            reader.Read();
                            jsonWD.Text += (string)reader.Value;
                            break;
                        case "SD":
                            reader.Read();
                            jsonSD.Text += (string)reader.Value;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (HttpRequestException)
            {
                var msgdialog1 = new MessageDialog("号码输入有误，请查证后输入").ShowAsync();
            }
            catch (Exception)
            {
                var msgdialog2 = new MessageDialog("此编号不存在，请查证后输入").ShowAsync();
            }
        }
        async void queryAsync(string tel)
        {
            try
            {
                string url = "http://apis.juhe.cn/mobile/get?phone=" + tel + "&key=7d432216e40447ca4bd9670f0797c5e4&dtype=xml";
                 HttpClient client = new HttpClient();
                 string result = await client.GetStringAsync(url);
                 XmlDocument document = new XmlDocument();
                 document.LoadXml(result);
                 XmlNodeList list = document.GetElementsByTagName("province");
                IXmlNode node1 = list.Item(0);
                list = document.GetElementsByTagName("city");
                IXmlNode node2 = list.Item(0);
                location.Text = node1.InnerText + node2.InnerText;
                list = document.GetElementsByTagName("company");
                node1 = list.Item(0);
                 runner.Text = "中国"+node1.InnerText;
                list = document.GetElementsByTagName("zip");
                node1 = list.Item(0);
                emps.Text = node1.InnerText;
                list = document.GetElementsByTagName("areacode");
                node1 = list.Item(0);
                areacode.Text = node1.InnerText;
            }
            catch (HttpRequestException)   
            {
                var msgdialog1 = new MessageDialog("号码输入有误，请查证后输入").ShowAsync();
            }
            catch (Exception)
            {
                var msgdialog2 = new MessageDialog("您输入的号码不存在，请重新输入").ShowAsync();
            }
        }
    }
}