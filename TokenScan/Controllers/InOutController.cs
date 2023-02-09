using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TokenScan.Models;
using TokenScan.App_Start;
using PagedList;
using PagedList.Mvc;
using Coinbase.Commerce;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Coinbase.Commerce.Models;


namespace TokenScan.Controllers
{
    
    public class InOutController : Controller
    {
        // GET: InOut

        #region userAction
        public ActionResult SignUp()
        {
            return View();
        }
        // Sign In view start
        public ActionResult SignIn()
        {
            return View();
        }
        // Sign In with parameter
        [HttpPost]
        public ActionResult SignIn(string email, string pass)
        {
            try
            {
                var s_pass = Encryption.GetSHA1.Hash(pass);
                using (TSCANEntities db = new TSCANEntities())
                {
                    var data = db.UserManagers.Where(s => s.email.Equals(email) && s.pass.Equals(s_pass)).ToList();
                    if (data.Count() > 0)
                    {
                        //Save session server
                        Session["user"] = email;
                        CookieHelper.Create("saU", email, DateTime.Now.AddDays(3));
                        CookieHelper.Create("saP", s_pass, DateTime.Now.AddDays(3));

                        return RedirectToAction("DashBoard", "Home");
                    }
                }
            }
            catch
            {

            }
            ViewBag.errMsg = "Wrong Email or Password, Please try again";
            return View();
        }
        // Sign Out 
        public ActionResult SignOut()
        {
            Session.Remove("user");
            CookieHelper.Remove("saU");
            CookieHelper.Remove("saP");
            return RedirectToAction("SignIn");
        }
        // add new User to database
        [HttpPost]
        public string AddUser(string email, string pass)
        {
            UserManager newUser = new UserManager();
            try
            {
                string msgTrue;
                using (TSCANEntities db = new TSCANEntities())
                {
                    var checkDup = db.UserManagers.Where(s => s.email.Equals(email)).ToList();
                    if (checkDup.Count() > 0)
                    {
                        msgTrue = "This email address already exists";
                    }
                    else
                    {
                        newUser.email = email;
                        newUser.pass = Encryption.GetSHA1.Hash(pass);
                        newUser.role = "user";
                        newUser.premiumDate = DateTime.Now.AddDays(-1).ToString();
                        db.UserManagers.Add(newUser);
                        db.SaveChanges();
                        msgTrue = "You have successfully registered";
                    }

                }
                return msgTrue;
            }
            catch (Exception ex)
            {

                string msg = ex.Message.ToString();
                return msg;
            }
        }
        // forgot email for User
        public string ForgotMail(string email)
        {
            string mess = "";
            string passNew = Common.common.CreatePassword(8);
            try
            {
                var db = new TSCANEntities();
                var data = db.UserManagers.Count(m => m.email == email);
                if (data >= 0)
                {
                    var dataUpdate = db.UserManagers.Find(email);
                    dataUpdate.pass = Encryption.GetSHA1.Hash(passNew);
                    db.SaveChanges();
                    string send = Common.common.SendMail(email, passNew);
                    mess = "Password Reset Email Has Been Sent";


                }
                else
                {
                    mess = "Can't Found your email, Please check again";
                }
            }
            catch (Exception ex)
            {

                mess = ex.Message.ToString();
            }
            
            return mess;
        }
        #endregion


        #region userManager 
        [KiemTraQuyen(role = "admin")]
        public ActionResult UserManager(string search, int? i)
        {
            var db = new TSCANEntities();
            List<UserManager> listUser = db.UserManagers.ToList();
            ViewBag.msg = TempData["msgDel"];
            return View(db.UserManagers.Where(x => x.email.StartsWith(search) || search == null).ToList().ToPagedList(i ?? 1, 3));
        }
        // Admin Edit user
        [KiemTraQuyen(role = "admin")]
        public string EditUser(string email , string pass, string role, string date)
        {
            string errMsg = "";
            var checkMe = Common.common.CheckUserLogin(email);
            try
            {
                if (!checkMe)
                {
                    using (var db = new TSCANEntities())
                    {
                        var data = db.UserManagers.Find(email);
                        data.pass = Encryption.GetSHA1.Hash(pass);
                        data.role = role;
                        data.premiumDate = date;
                        db.SaveChanges();
                        errMsg = "Edit complete";
                    }

                }
                else
                {
                    errMsg = "Can't edit your account";
                }
            }
            catch (Exception ex)
            {

                errMsg = ex.Message.ToString();
            }

            return errMsg;
        }

        //Admin Delete User
        [KiemTraQuyen(role = "admin")]
        public ActionResult DeleteUser(string email)
        {
            var checkMe = Common.common.CheckUserLogin(email);
            try
            {
                if (!checkMe)
                {
                    using (var db = new TSCANEntities())
                    {
                        var data = db.UserManagers.Find(email);
                        db.UserManagers.Remove(data);
                        db.SaveChanges();

                    }
                    TempData["msgDel"] = "User Deleted";
                }
                else
                {
                    TempData["msgDel"] = "Can't delete your account";
                }
            }
            catch (Exception ex)
            {

                TempData["msgDel"] = ex.Message.ToString();
            }

            return RedirectToAction("UserManager");
        }


        #endregion

        // Page show not have role
        public ActionResult NoRolePage()
        {
            return View();
       
        }
        
    }
}
