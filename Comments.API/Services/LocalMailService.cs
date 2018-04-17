﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comments.API.Services
{
    public class LocalMailService : IMailService
    {
 
        //private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        //private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

       private string _mailTo = "admin@mycompany.com";
       private string _mailFrom = "noreply@mycompany.com";

        public void Send(String subject, string message)
        {
            // send mail - output to debug window
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");

        }
    }
    
}
