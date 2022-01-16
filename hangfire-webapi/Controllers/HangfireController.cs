using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hangfire_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hangfire webapi çalıştı !");
        }

        [HttpPost]
        [Route("[action]")]
        // ****     action = metod adı !
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Hoşgeldin maili !"));

            return Ok($"Job Id:{jobId}. Kullanıcıya enqeue ile mesajı gönderildi !");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            // erteleme // delay
            var timeInSecond = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Hoşgeldin maili !"), TimeSpan.FromSeconds(timeInSecond));

            return Ok($"Job Id:{jobId}. Kullanıcıya delayed olarak {timeInSecond} 'lik mesajı gönderildi !");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("database güncellendi !"), Cron.Minutely);
            return Ok("Database check job initiated !");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            var timeInSecond = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("you asked to be unsubscribed !"), TimeSpan.FromSeconds(timeInSecond));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("you were unsubscribed !"));

            return Ok("Confirmation  job created !");
        }


        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }

    }
}
