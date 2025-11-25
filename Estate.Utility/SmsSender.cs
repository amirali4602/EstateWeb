using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayamakCore.Dto;
using PayamakCore.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.Utility
{
    public class SmsSender 
    {
        public string sendmessage(string phoneNumber)
        {
            var chars = "0123456789";
            var stringChars = new char[5];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            //gilpayamak
            var client = new RestClient("http://188.0.240.110/api/select");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody("{\"op\" : \"patternV2\"" +
                ",\"user\" : \"korosh55\"" +
                ",\"pass\":  \"Korosh@55\"" +
                ",\"fromNum\" : \"100020400\"" +
                ",\"toNum\": \"" + phoneNumber.ToString() + "\"" +
                ",\"patternCode\": \"aow1a83d1aea2rv\"" +
                ",\"inputData\" : {\"verification-code\": \"" + finalString + "\"}}");
            dynamic response = client.Execute(request);
            Console.WriteLine(response.Content);
            //payamito
            //var client = new RestClient("http://rest.payamak-panel.com/api/SendSMS/SendSMS");
            //var request = new RestRequest();
            //var textMessage = "سایت املاک حمید\nکد فعالسازی شما : ";
            //request.Method = Method.Post;
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            //request.AddHeader("postman-token", "fcddb5f4-dc58-c7d5-4bf9-9748710f8789");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddParameter("application/x-www-form-urlencoded", "username=0058497277&password=6NOZY&to="+ phoneNumber + "&from=50002002172116&text="+ textMessage + finalString +"&isflash=false", ParameterType.RequestBody);
            //dynamic response = client.Execute(request);


            if (response != null)
            {
                return finalString;
            }
            else
            {
                return "error";
            }

        }

    }

}





