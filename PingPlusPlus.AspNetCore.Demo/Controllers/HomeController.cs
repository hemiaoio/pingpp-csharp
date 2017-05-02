using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pingpp.Models;
using PingPlusPlus.AspNetCore.Demo.Models;

namespace PingPlusPlus.AspNetCore.Demo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<Charge> Index([FromBody]ChargeModel input) {
            Pingpp.Pingpp.SetApiKey("sk_test_ibbTe5jLGCi5rzfH4OqPW9KC");
            const string appId = "app_1Gqj58ynP0mHeX1q";

            var extra = new Dictionary<string, object>();
            if(input.Channel.Equals("alipay_wap")) {
                extra.Add("success_url", "http://www.yourdomain.com/success");
                extra.Add("cancel_url", "http://www.yourdomain.com/cancel");
            } else if(input.Channel.Equals("wx_pub")) {
                extra.Add("open_id", "asdfasdfsadfasdf");
            } else if(input.Channel.Equals("upacp_wap")) {
                extra.Add("result_url", "http://www.yourdomain.com/result");
            } else if(input.Channel.Equals("upmp_wap")) {
                extra.Add("result_url", "http://www.yourdomain.com/result?code=");
            } else if(input.Channel.Equals("bfb_wap")) {
                extra.Add("result_url", "http://www.yourdomain.com/result");
                extra.Add("bfb_login", true);
            } else if(input.Channel.Equals("wx_pub_qr")) {
                extra.Add("product_id", "asdfsadfadsf");
            } else if(input.Channel.Equals("yeepay_wap")) {
                extra.Add("product_category", "1");
                extra.Add("identity_id", "sadfsdaf");
                extra.Add("identity_type", 1);
                extra.Add("terminal_type", 1);
                extra.Add("terminal_id", "sadfsadf");
                extra.Add("user_ua", "sadfsdaf");
                extra.Add("result_url", "http://www.yourdomain.com/result");
            } else if(input.Channel.Equals("jdpay_wap")) {
                extra.Add("success_url", "http://www.yourdomain.com/success");
                extra.Add("fail_url", "http://www.yourdomain.com/fail");
                extra.Add("token", "fjdilkkydoqlpiunchdysiqkanczxude"); //32 位字符串，京东支付成功后会返回
            }

            var param = new Dictionary<string, object> {
                {"order_no", input.Order_no}, {"amount", input.Amount}, {"channel", input.Channel}, {"currency", "cny"},
                {"subject", "test"}, {"body", "tests"}, {"client_ip", "127.0.0.1"},
                {"app", new Dictionary<string, string> {{"id", appId}}}, {"extra", extra}
            };

            var charge = await Charge.Create(param);

            return charge;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
