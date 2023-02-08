using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TokenScan.Models;
using System.Diagnostics;
using TokenScan.App_Start;

namespace TokenScan.Controllers
{
    public class HomeController : Controller
    {
        //[KiemTraQuyen(role = "admin")]
        [KiemTraQuyen]
        public ActionResult DashBoard(string listAdd)
        {
            if (listAdd != null)
            {
                var data = GetToken(listAdd);              
                ViewBag.List = data;
                ViewBag.errorMessage = TempData["errorMessage"];
                ViewBag.checkAdmin = Common.common.CheckUserAdmin();

            }
            return View();
        }

        public List<DataShow> GetToken(string listAdd)
        {
            //list dataShow 
            List<DataShow> dataShow = new List<DataShow>();
            try
            {
                //chia address list ra tung address
                List<string> singleAdd = listAdd.Split('\n').ToList();
                foreach (var add in singleAdd)
                {
                    
                    if (add.Length > 0)
                    {
                        var data = new DataShow();
                        data.wallet = add;
                        string url = String.Format("url" + add);
                        WebRequest requestOject = WebRequest.Create(url);
                        requestOject.Method = "GET";
                        HttpWebResponse response = null;
                        response = (HttpWebResponse)requestOject.GetResponse();
                        string strResult = null;
                        using (Stream stream = response.GetResponseStream())
                        {
                            StreamReader sr = new StreamReader(stream);
                            strResult = sr.ReadToEnd();
                            var model = JsonConvert.DeserializeObject<List<DataResult>>(strResult);
                            data.dataResult = model;
                            sr.Close();
                        }
                        dataShow.Add(data);
                    }
                    else
                    {
                        break;
                    }

                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("400"))
                {
                    TempData["errorMessage"] = "Invalid account";

                }
                else if (ex.Message.Contains("429"))
                {
                    TempData["errorMessage"] = "Too Many Requests";

                }
                else if (ex.Message.Contains("500"))
                {
                    TempData["errorMessage"] = "Server Error";

                }
                else
                {
                    TempData["errorMessage"] = ex.Message.ToString();

                }
            }                   
            return dataShow;
        }
    }




    
}

