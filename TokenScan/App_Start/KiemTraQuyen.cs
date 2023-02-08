using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TokenScan.Models;

namespace TokenScan.App_Start
{
    public class KiemTraQuyen : AuthorizeAttribute
    {
        public string role { set; get; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            var db = new TSCANEntities();

            var user = HttpContext.Current.Session["user"];
            string cookieUser = CookieHelper.Get("saU");
            string cookiePass = CookieHelper.Get("saP");
            var dataUser = db.UserManagers.SingleOrDefault(m => m.email == cookieUser);
            if (dataUser != null)
            {
                if (dataUser.pass == cookiePass)
                {
                    user = dataUser.email;
                    HttpContext.Current.Session["user"] = dataUser.email;
                }
            }
            //check session, neu chua co thi can dang nhap
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {

                    controller = "InOut",
                    action = "signIn",
                    area = ""

                }));
                return;
            }
            // Neu khong dien chuc nang thi cho phep chay
            if (string.IsNullOrEmpty(role))
            {
                return;
            }
            // kiem tra phan quyen

            var checkRole = db.UserManagers.Count(m => m.email == user.ToString() & m.role == role);
            if (checkRole <=0)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
                {

                    controller = "InOut",
                    action = "NoRolePage",
                    area = ""

                }));
                return;
            }
            

        }
    }
}